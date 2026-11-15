using UnityEngine;

// Zombie behaviour. The world scrolls toward -z (GameManager.currentSpeed is negative) and the car sits near z = 0.
// Zombies spawn ahead, ride the ground with MoveWithWorld, then run alongside the car, leap onto a free window
// trigger (FL/FR/BL/BR on the car) and drain health until the car shakes them off with hard steering
// or scrapes them off against an obstacle.
//
// Waiting -> Chasing -> Leaping -> Attached -> Falling -> Down
//                 \-> GaveUp (passed the car without a free window)
[RequireComponent(typeof(MoveWithWorld))]
public class EnemyController : MonoBehaviour
{
    public enum State { Waiting, Chasing, GaveUp, Leaping, Attached, Falling, Down }

    [Header("Chase")]
    [SerializeField] float noticeDistance = 30f;   // starts running when this far ahead of the car (z)
    [SerializeField] float lateralRunSpeed = 4f;   // sideways speed toward the car's flank
    [SerializeField] float forwardRunSpeed = 3f;   // runs the same way as the car, so it "keeps up" for longer
    [SerializeField] float flankGap = 1.6f;        // how far out from the car's centre line it runs

    [Header("Leap")]
    [SerializeField] float leapDuration = 0.45f;
    [SerializeField] float leapHeight = 1.2f;
    [SerializeField] float maxLeapSideDistance = 3f;
    [SerializeField] Vector3 hangOffset = new Vector3(0.35f, -0.2f, 0f); // x is mirrored per side, relative to the window trigger

    [Header("Attached")]
    [SerializeField] float damagePerSecond = 4f;
    [SerializeField] float shakeOffTurnRate = 1.5f;  // car yaw speed (rad/s) that counts as shaking
    [SerializeField] float shakeOffTime = 0.6f;      // seconds of hard steering needed to throw it off

    [Header("Scrape")]
    [SerializeField] Vector2 scrapeVelocity = new Vector2(6f, 4f); // outward, up: harder throw than shaking off

    [Header("Fall")]
    [SerializeField] float gravity = 20f;
    [SerializeField] Vector2 throwVelocity = new Vector2(4f, 3f); // outward, up

    [Header("Animation (optional)")]
    [SerializeField] Animator animator;              // int parameter "State" = (int)State, skipped if missing
    static readonly int StateParam = Animator.StringToHash("State");
    bool hasStateParam;

    public State CurrentState { get; private set; }
    public int ClaimedWindow { get; private set; } = -1;

    MoveWithWorld moveWithWorld;
    Collider col;
    PlayerCarController car;
    Transform window;
    int side;               // -1 left of the car, +1 right
    float groundY;
    Vector3 leapStart;
    float leapT, damageAcc, shake;
    Vector3 fallVelocity;

    void Awake()
    {
        moveWithWorld = GetComponent<MoveWithWorld>();
        col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true; // the car's Front trigger still sees it, but the car body no longer bumps into it
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (animator != null && animator.runtimeAnimatorController != null)
            foreach (var p in animator.parameters)
                if (p.nameHash == StateParam && p.type == AnimatorControllerParameterType.Int) hasStateParam = true;
    }

    void OnEnable()
    {
        // Pool.spawn has already placed us on the ground
        groundY = transform.position.y;
        transform.rotation = Quaternion.LookRotation(Vector3.back); // face the oncoming car
        moveWithWorld.enabled = true; // may have been switched off mid-leap/hang before this zombie was pooled
        if (col != null) col.enabled = true;
        ClaimedWindow = -1;
        window = null;
        SetState(State.Waiting);
    }

    void OnDisable()
    {
        ReleaseWindow(); // killed by the car or returned to the pool while hanging on
    }

    void Update()
    {
        if (LifecycleGuard.IsPaused) return;
        if (car == null)
        {
            if (GameManager.current == null || GameManager.current.playerCar == null) return;
            car = GameManager.current.playerCar;
        }

        float dt = Time.deltaTime;
        bool playing = GameManager.current.state == GameManager.GameState.Playing;

        switch (CurrentState)
        {
            case State.Waiting:
                if (playing && transform.position.z - car.transform.position.z < noticeDistance)
                {
                    SetState(State.Chasing);
                    AudioManager.Play(Sfx.ZombieNotice);
                }
                break;
            case State.Chasing:
                if (playing) Chase(dt);
                break;
            case State.Leaping:
                Leap(dt);
                break;
            case State.Attached:
                Hang(dt, playing);
                break;
            case State.Falling:
                Fall(dt);
                break;
            // GaveUp / Down: just ride the world until MoveWithWorld sends us back to the pool
        }
    }

    void Chase(float dt)
    {
        Vector3 pos = transform.position;
        Vector3 carPos = car.transform.position;

        if (pos.z < carPos.z - 3f) { SetState(State.GaveUp); return; } // fell behind

        if (side == 0) side = pos.x >= carPos.x ? 1 : -1;
        float targetX = carPos.x + side * flankGap;
        float dx = Mathf.MoveTowards(pos.x, targetX, lateralRunSpeed * dt) - pos.x;
        pos.x += dx;
        pos.z += forwardRunSpeed * dt; // MoveWithWorld adds the world scroll on top
        transform.position = pos;

        Vector3 runDir = new Vector3(dx / Mathf.Max(dt, 1e-5f), 0f, forwardRunSpeed);
        if (runDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(runDir), 10f * dt);

        // Leap when we'll meet a free window on our side within the leap time
        int w = car.FindFreeWindow(side, pos.z);
        if (w < 0) return;
        Transform slot = car.GetWindow(w);
        float closing = -(GameManager.currentSpeed + forwardRunSpeed); // how fast the window approaches us in z
        if (closing <= 0.1f) return;
        float timeToMeet = (pos.z - slot.position.z) / closing;
        if (timeToMeet < -0.1f || timeToMeet > leapDuration) return;
        if (Mathf.Abs(pos.x - slot.position.x) > maxLeapSideDistance) return;

        if (car.ClaimWindow(w, this))
        {
            ClaimedWindow = w;
            window = slot;
            side = slot.localPosition.x >= 0 ? 1 : -1;
            leapStart = pos;
            leapT = 0f;
            moveWithWorld.enabled = false; // we move the start point with the world ourselves
            if (col != null) col.enabled = false; // don't touch the car's crash triggers (Top) on the way in
            SetState(State.Leaping);
            AudioManager.Play(Sfx.ZombieLeap);
        }
    }

    Vector3 HangLocalPosition => new Vector3(hangOffset.x * side, hangOffset.y, hangOffset.z);

    void Leap(float dt)
    {
        if (window == null) { Drop(); return; }

        leapStart.z += GameManager.currentSpeed * dt; // take-off point keeps scrolling with the ground
        leapT = Mathf.Min(1f, leapT + dt / leapDuration);

        // Re-aim at the window every frame so steering mid-leap doesn't make it miss
        Vector3 target = window.TransformPoint(HangLocalPosition);
        float t = leapT * leapT * (3f - 2f * leapT); // smoothstep
        Vector3 pos = Vector3.Lerp(leapStart, target, t);
        pos.y += Mathf.Sin(leapT * Mathf.PI) * leapHeight;
        transform.position = pos;

        Quaternion faceCar = Quaternion.LookRotation(window.TransformDirection(Vector3.right * -side));
        transform.rotation = Quaternion.Slerp(transform.rotation, faceCar, 12f * dt);

        if (leapT >= 1f) Attach();
    }

    void Attach()
    {
        transform.SetParent(window, false);
        transform.localPosition = HangLocalPosition;
        transform.localRotation = Quaternion.LookRotation(Vector3.right * -side); // face the car
        // Collider back on: it's now part of the car's Rigidbody, so it only reports what the zombie's body brushes past
        // (obstacles, see OnTriggerEnter). Colliders on the same Rigidbody never trigger each other, so Top/Front stay quiet.
        if (col != null) col.enabled = true;
        damageAcc = 0f;
        shake = 0f;
        Haptics.Pulse();
        AudioManager.Play(Sfx.ZombieLand);
        SetState(State.Attached);
    }

    void Hang(float dt, bool playing)
    {
        if (!playing) return;

        damageAcc += damagePerSecond * dt;
        int whole = (int)damageAcc;
        if (whole > 0)
        {
            damageAcc -= whole;
            car.TakeDamage(whole);
            AudioManager.Play(Sfx.ZombieClaw); // minInterval on the clip keeps this from firing every tick
        }

        // Hard steering builds up "shake"; easing off lets it recover
        float yawRate = Mathf.Abs(car.Body.angularVelocity.y);
        shake = yawRate > shakeOffTurnRate ? shake + dt : Mathf.Max(0f, shake - 0.5f * dt);
        if (shake >= shakeOffTime) Drop(throwVelocity);
    }

    // Scraped off: the car drove close enough to an obstacle that it hit the hanging zombie
    void OnTriggerEnter(Collider other)
    {
        if (CurrentState != State.Attached || !Obstacles.IsObstacle(other)) return;
        Haptics.Pulse();
        Drop(scrapeVelocity);
    }

    public void Drop() => Drop(throwVelocity);

    // Let go of the car (shaken off, scraped off, or lost the window mid-leap)
    void Drop(Vector2 throwVel)
    {
        ReleaseWindow();
        transform.SetParent(null, true);
        moveWithWorld.enabled = true;
        if (col != null) col.enabled = false; // no second hit (or coins) from the car's Front trigger while falling
        fallVelocity = new Vector3(side * throwVel.x, throwVel.y, 0f);
        AudioManager.Play(Sfx.ZombieKnockedOff);
        SetState(State.Falling);
    }

    void Fall(float dt)
    {
        Vector3 pos = transform.position;
        fallVelocity.y -= gravity * dt;
        pos.x += fallVelocity.x * dt;
        pos.y += fallVelocity.y * dt; // z comes from MoveWithWorld
        transform.Rotate(0f, 0f, -side * 360f * dt, Space.World); // tumble away from the car

        if (pos.y <= groundY)
        {
            pos.y = groundY;
            transform.rotation = Quaternion.Euler(0f, 0f, -side * 90f); // lying on the road
            SetState(State.Down);
        }
        transform.position = pos;
    }

    void ReleaseWindow()
    {
        if (ClaimedWindow >= 0 && car != null) car.ReleaseWindow(ClaimedWindow, this);
        ClaimedWindow = -1;
        window = null;
    }

    void SetState(State s)
    {
        CurrentState = s;
        if (s == State.Waiting) side = 0;
        if (hasStateParam) animator.SetInteger(StateParam, (int)s);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
