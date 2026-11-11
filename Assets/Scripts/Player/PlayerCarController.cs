using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    [SerializeField] private Transform roadCheck;
    public float turnMult;
    public float driveForce;
    public float currentTurn;
    
    public int health, maxHealth, startMaxHealth; // todo consider adding / changing to FUEL

    [SerializeField] private Rigidbody body;
    
    
    void OnEnable() { Wallet.wallet.Changed += UpgradeHealth; UpgradeHealth();}
    void OnDisable() { Wallet.wallet.Changed -= UpgradeHealth; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(LifecycleGuard.IsPaused) return;
        // check that front wheels are on ground
        if(Physics.Raycast(roadCheck.position, Vector3.down, .6f))
        {
            Vector3 transformForward = transform.forward;
            transformForward.Scale(new Vector3(1, 0, 1));
            roadCheck.localEulerAngles = new Vector3(0, currentTurn * 60, 0);
            body.AddForceAtPosition(transformForward * -GameManager.currentSpeed, roadCheck.position); // car rolls regardless of game state
            
            if (GameManager.current.state == GameManager.GameState.Playing) // car can only be steered in play
                body.AddForceAtPosition((roadCheck.forward * turnMult /* * -GameManager.currentSpeed*/), roadCheck.position);
            //transform.Rotate(0, currentTurn * turnMult, 0);
        }

        // TODO use addForceAtpoint instead? OR manipulate car rig via scripts, depending on how car ends up being built.
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
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.position = Vector3.up * 1.2f;
        body.rotation = Quaternion.identity;
    }
}
