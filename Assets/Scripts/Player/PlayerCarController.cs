using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    [SerializeField] private Transform roadCheck;
    public float turnMult;
    public float driveForce;
    public float currentTurn;
    
    public int health, maxHealth, startMaxHealth;
    public event System.Action<int, int> HealthChanged; // (health, maxHealth), for the HUD

    [Header("Speed penalties (share of current speed lost)")]
    [SerializeField, Range(0f, 1f)] float zombieHitPenalty = 0.15f;
    [SerializeField, Range(0f, 1f)] float scrapePenalty = 0.2f;
    [SerializeField] float scrapeCooldown = 0.5f; // one long scrape along a wall shouldn't stack penalties every frame
    float lastScrapeTime = -10f;

    [SerializeField] private Rigidbody body;
    [SerializeField] private MoveWithWorld moveWithWorld;
    
    
    void OnEnable() { Wallet.wallet.Changed += UpgradeHealth; UpgradeHealth();}
    void OnDisable() { Wallet.wallet.Changed -= UpgradeHealth; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 roadCheckLocalPos; // front axle point, relative to the car

    // Zombie landing zones: the window triggers under "Triggers". Found by name if not assigned in the Inspector.
    [SerializeField] private Transform[] windowSlots;
    private EnemyController[] windowOccupants;
    public Rigidbody Body => body;

    void Awake()
    {
        if (windowSlots == null || windowSlots.Length == 0)
        {
            string[] names = { "Triggers/FL", "Triggers/FR", "Triggers/BL", "Triggers/BR" };
            var found = new System.Collections.Generic.List<Transform>();
            foreach (var n in names)
            {
                var t = transform.Find(n);
                if (t != null) found.Add(t);
                else Debug.LogWarning($"[Car] window slot '{n}' not found");
            }
            windowSlots = found.ToArray();
        }
        windowOccupants = new EnemyController[windowSlots.Length];

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
        if(Physics.Raycast(axlePoint, Vector3.down, .6f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) // triggers (slow areas, zombies) aren't ground
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
        GameManager.current.EarnCoins(25);
        Haptics.Pulse();
        GameManager.current.ApplySpeedPenalty(zombieHitPenalty);
    }

    // Wallet.Changed fires on every coin earned as well as on upgrades, so this only recalculates the cap.
    // (It used to heal 40 each time, so running over a zombie also healed the car.) Each round starts full in ResetCar.
    void UpgradeHealth()
    {
        maxHealth = startMaxHealth + (Wallet.wallet.Level * 10);
        if (health > maxHealth) health = maxHealth;
        HealthChanged?.Invoke(health, maxHealth);
    }

    // Car body brushing past an obstacle's side. Driving up a ramp (contact normal pointing up) doesn't count.
    void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastScrapeTime < scrapeCooldown) return;
        if (!Obstacles.IsObstacle(collision.collider)) return;
        if (collision.contactCount > 0 && Mathf.Abs(collision.GetContact(0).normal.y) > 0.5f) return;
        lastScrapeTime = Time.time;
        Haptics.Pulse();
        GameManager.current.ApplySpeedPenalty(scrapePenalty);
    }

    // ---- Zombie window slots ----

    // Free window on the given side (-1 left, +1 right) whose z is closest to fromZ (world space). Returns -1 if none.
    public int FindFreeWindow(int side, float fromZ)
    {
        int best = -1;
        float bestDist = float.MaxValue;
        for (int i = 0; i < windowSlots.Length; i++)
        {
            if (IsOccupied(i)) continue;
            if (Mathf.Sign(windowSlots[i].localPosition.x) != side) continue;
            float d = Mathf.Abs(windowSlots[i].position.z - fromZ);
            if (d < bestDist) { bestDist = d; best = i; }
        }
        return best;
    }

    bool IsOccupied(int i)
    {
        var o = windowOccupants[i];
        return o != null && o.isActiveAndEnabled && o.ClaimedWindow == i; // stale claims (pooled/killed zombies) count as free
    }

    public bool ClaimWindow(int i, EnemyController zombie)
    {
        if (i < 0 || i >= windowSlots.Length || IsOccupied(i)) return false;
        windowOccupants[i] = zombie;
        return true;
    }

    public void ReleaseWindow(int i, EnemyController zombie)
    {
        if (i >= 0 && i < windowOccupants.Length && windowOccupants[i] == zombie) windowOccupants[i] = null;
    }

    public Transform GetWindow(int i) => windowSlots[i];

    public void TakeDamage(int amount)
    {
        if (GameManager.current.state != GameManager.GameState.Playing) return;
        ChangeHealth(-amount);
    }

    void ChangeHealth(int healthChange)
    {
        health += healthChange;
        if(health > maxHealth) 
            health = maxHealth;
        else if (health <= 0){
            health = 0;
            HealthChanged?.Invoke(health, maxHealth);
            Die();
            return;
        }
        HealthChanged?.Invoke(health, maxHealth);
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

    public void Crash(bool withConstraints, GameManager.DeathCause cause) // called by crash colliders on car body
    {
        body.constraints = withConstraints ? RigidbodyConstraints.FreezePositionZ : RigidbodyConstraints.None; // loosen constraints for physics
        moveWithWorld.enabled = !withConstraints;
        if (GameManager.current.state == GameManager.GameState.Playing) Haptics.Pulse();
        GameManager.current.GameOver(cause);
    }
    void Die() // called when HP is below 0
    {
        // Overrun: unlike a crash, the car isn't thrown about. Constraints stay on, steering stops with the game state,
        // and the car stalls and coasts to a halt as the world slows, with the zombies still hanging on.
        body.angularVelocity = Vector3.zero;
        GameManager.current.GameOver(GameManager.DeathCause.Overrun);
    }

    public void ResetCar()
    {
        body.constraints = RigidbodyConstraints.FreezePositionZ;
        moveWithWorld.enabled = false;
        
        transform.SetPositionAndRotation(Vector3.up * 1.2f, Quaternion.identity);
        Physics.SyncTransforms(); // push the teleport into physics now, so forces added before the next FixedUpdate use the correct centre of mass

        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        // zombies drain health now, so each round must start full (may run before OnEnable on the first round)
        if (Wallet.wallet != null) maxHealth = startMaxHealth + Wallet.wallet.Level * 10;
        health = maxHealth;
        HealthChanged?.Invoke(health, maxHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("OutOfBounds"))
            GameManager.current.GameOver(GameManager.DeathCause.OutOfBounds);
    }
}
