using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    [SerializeField] private Transform roadCheck;
    public float turnMult;
    public float driveForce;
    public float currentTurn;
    
    public int health, maxHealth, startMaxHealth; // todo consider adding / changing to FUEL

    [SerializeField] private Rigidbody body;
    [SerializeField] private MoveWithWorld moveWithWorld;
    
    
    void OnEnable() { Wallet.wallet.Changed += UpgradeHealth; UpgradeHealth();}
    void OnDisable() { Wallet.wallet.Changed -= UpgradeHealth; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 roadCheckLocalPos; // front axle point, relative to the car

    void Awake()
    {
        // Smooth the rendered car between physics steps so anything attached to it (e.g. zombies on the windows) doesn't jitter.
        body.interpolation = RigidbodyInterpolation.Interpolate;
        roadCheckLocalPos = roadCheck.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(LifecycleGuard.IsPaused) return;
        roadCheck.localEulerAngles = new Vector3(0, currentTurn * 60, 0); // visual/debug only, forces use the body pose below
    }

    // Forces go on the physics step so drive/steer strength doesn't depend on frame rate.
    // With interpolation on, transform is the smoothed render pose, so read the pose from the Rigidbody instead.
    void FixedUpdate()
    {
        if(LifecycleGuard.IsPaused) return;

        Quaternion rot = body.rotation;
        Vector3 axlePoint = body.position + rot * roadCheckLocalPos;

        // check that front wheels are on ground
        if(Physics.Raycast(axlePoint, Vector3.down, .6f))
        {
            Vector3 flatForward = rot * Vector3.forward;
            flatForward.y = 0;
            body.AddForceAtPosition(flatForward * -GameManager.currentSpeed, axlePoint); // car rolls regardless of game state

            if (GameManager.current.state == GameManager.GameState.Playing) // car can only be steered in play
            {
                Vector3 steerDir = rot * Quaternion.Euler(0, currentTurn * 60, 0) * Vector3.forward;
                body.AddForceAtPosition(steerDir * turnMult, axlePoint);
            }
        }
    }

    void HitEnemy()
    {
        if (Wallet.wallet != null)
        {
            Wallet.wallet.Earn(25);
            
        }
    }
    void UpgradeHealth()
    {
        maxHealth = startMaxHealth + (Wallet.wallet.Level * 10);
        ChangeHealth(40);
    }

    void ChangeHealth(int healthChange)
    {
        health += healthChange;
        if(health > maxHealth) 
            health = maxHealth;
        else if (health <= 0){
            health = 0;
            Die();
        }
    }
    
    // might be needed to assist lifting the car on a jump ramp
    void Jump()
    {
        // apply upward force at front of car to lift front
        // possibly remove mass/gravity temporarily
    }

    void StopJump() // alternative is to set a timer or coroutine
    {
        // reset gravity values and begin correcting rotation of car, perhaps dampen the landing movement with more mass
    }
    
    void Turn(float amount)
    {
        transform.Rotate(0, amount, 0);
    }

    void Crash(bool withConstraints) // called by crash colliders on car body
    {
        body.constraints = withConstraints ? RigidbodyConstraints.FreezePositionZ : RigidbodyConstraints.None; // loosen constraints for physics
        moveWithWorld.enabled = !withConstraints;
        GameManager.current.GameOver();
    }
    void Die() // called when HP is below 0
    {
        body.constraints = RigidbodyConstraints.None; // loosen constraints for physics
        GameManager.current.GameOver();
        // todo unique death
    }

    public void ResetCar()
    {
        body.constraints = RigidbodyConstraints.FreezePositionZ;
        moveWithWorld.enabled = false;
        
        transform.SetPositionAndRotation(Vector3.up * 1.2f, Quaternion.identity);
        Physics.SyncTransforms(); // push the teleport into physics now, so forces added before the next FixedUpdate use the correct centre of mass

        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("OutOfBounds"))
            GameManager.current.GameOver();
    }
}
