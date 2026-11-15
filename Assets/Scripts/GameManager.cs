using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager current; // singleton
    public enum GameState { Playing, Paused, GameOver } 
    public GameState state;
    public event System.Action<GameState> StateChanged;
    private CancellationTokenSource _cts;
    private LifecycleGuard _lifecycleGuard;

    public TextMeshProUGUI scoreText;
    public PlayerCarController playerCar; // Player will assign itself at start.
    [SerializeField] private float startSpeed;
    [SerializeField] private float startWidth;
    [SerializeField] private Vector2 spawnDistanceRange;
    [SerializeField] private float interPoleDistance;
    
    public static float currentSpeed; // what the world actually scrolls at (negative)
    private float baseSpeed;          // the difficulty ramp; currentSpeed recovers toward this after penalties

    [Header("Speed penalties")]
    [SerializeField] private float speedRampPerMinute = 3f;
    [SerializeField] private float speedRecoveryPerSecond = 2f; // how fast currentSpeed climbs back to baseSpeed
    [SerializeField, Range(0f, 1f)] private float minSpeedFraction = 0.35f; // penalties can't drop below this share of baseSpeed
    private static double currentDistance;
    public static float roadWidth;
    
    float zombieDistance; // distance until next zombie spawn
    float obstacleDistance; // 
    float poleDistance; // 
    
    private float roundStartTime;
    private int roundCoinsEarned;
    private int attempt;

    private Pool zombiePool, obstaclePool, itemPool, propPool, telephonePolePool;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject[] obstaclePrefabs; // todo perhaps convert to multiple small pools later, one for each specific obstacle type, and one large pool for basic props
    [SerializeField] GameObject telephonePolePrefab;
    
    // public ObjectPool<item>
    // public ObjectPool<Obstacle> // 2 of each variant should be good
    
    void Awake() // Called before Start
    {
        if(current != null) Destroy((this));
        current = this;
        _lifecycleGuard = FindAnyObjectByType<LifecycleGuard>();
        
        // initialise pools
        zombiePool = new Pool(zombiePrefab);
        zombiePool.CreatePool(10);
        obstaclePool = new Pool(obstaclePrefabs);
        obstaclePool.CreatePool(10);
        
        telephonePolePool = new Pool(telephonePolePrefab);
        telephonePolePool.CreatePool(3);
        
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (LifecycleGuard.IsPaused) return;
        float dist = Time.deltaTime * currentSpeed; // negative
        // if in play
        // loop timers for spawning stuff
        // if spawn,
        // randomise spawning type/num based on difficulty
        
        zombieDistance += dist;
        if(zombieDistance <= 0) SpawnZombie();
        obstacleDistance += dist;
        if(obstacleDistance <= 0) SpawnObstacle();
        poleDistance += dist;
        if(poleDistance <= 0) SpawnPole();
        
        currentDistance -= dist;
        if (state == GameState.Playing) // during GameOver the SlowDown routine owns currentSpeed
        {
            baseSpeed -= speedRampPerMinute * Time.deltaTime / 60f;
            currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, speedRecoveryPerSecond * Time.deltaTime);
        }
        scoreText.text = currentDistance.ToString("F2");
    }

    public void NewGame() //TODO
    {
        // Initialise Pools
        zombiePool.Reset();
        obstaclePool.Reset();
        // todo itemPool.Reset();
        
        // reset values & difficulty
        if (_cts != null){
            _cts.Cancel(); _cts.Dispose(); _cts = null; } // completely reset the cancel token

        currentSpeed = baseSpeed = startSpeed;
        roadWidth = startWidth;
        currentDistance = 0;
        roundStartTime = Time.time;
        roundCoinsEarned = 0;
        
        // Check playerCar exists
        // playerCar.reset
        playerCar.ResetCar();
        
        // place starting obstacles / zombies
        // start obstacle interval timer
        
        // play
        state = GameState.Playing;
        StateChanged?.Invoke(GameState.Playing);
        _lifecycleGuard.SetPaused(false);
        
        Telemetry.Log("level_start",
            ("level_id", "Game"),
            ("attempt", ++attempt),
            ("coins", Wallet.wallet.Coins));
    }

    // Knock a share of the current speed off (scrapes, slow areas, zombie hits). It recovers back to baseSpeed over time.
    public void ApplySpeedPenalty(float fraction)
    {
        if (state != GameState.Playing) return;
        float floor = baseSpeed * minSpeedFraction; // speeds are negative, so the floor is the value closest to 0
        currentSpeed = Mathf.Min(currentSpeed * (1f - fraction), floor);
    }

    public void Pause() // can be called by LifeCycleGuard to trigger pause state. 
    {
        state = GameState.Paused;
        StateChanged?.Invoke(GameState.Paused); // todo check if this state will be used by anything
    }

    public void GameOver()
    {
        if (state == GameState.GameOver)  return;
        
        // immediately slow down scrolling
        _cts = new CancellationTokenSource();
        _ = SlowDown(_cts.Token); // assigns awaitable to var so the following code can continue
        
        // stop object spawning (might be dependent on scrolling anyway, TBD)
        
        
        // play death animation/scene (allowing the camera to move if needed)
        
        state = GameState.GameOver;
        StateChanged?.Invoke(GameState.GameOver);
        Telemetry.Log("level_gameOver",
            ("level_id", "Game"),
            ("time_s", Time.time - roundStartTime),
            ("coins_earned", roundCoinsEarned),
            ("score", currentDistance));
        // todo add cause ^^^ 
        
        // bring up end screen menu, lingering on stopped scene todo might be called by StateChanged elsewhere
        
        Debug.Log("Game Over");
    }
    private async Awaitable SlowDown(CancellationToken cancellationToken)
    {
        float rate = currentSpeed / 5;
        Debug.Log("Slowing down");

        while (currentSpeed < 0) // remember speed is stored in negatives
        {
            if (cancellationToken.IsCancellationRequested) return;

            currentSpeed -= rate * Time.deltaTime;
            if (currentSpeed > 0) currentSpeed = 0; 

            // Pauses *only* this method until the next frame
            await Awaitable.NextFrameAsync(cancellationToken);
        }
        
        if (_cts != null && _cts.Token == cancellationToken)
        {
            _cts.Dispose();
            _cts = null; 
        }

        Debug.Log("The float has finally hit 0!");
    }
    

    void SpawnZombie()
    {
        zombiePool.spawn(new Vector3(Random.Range(-roadWidth/2, roadWidth/2), 1, 50), Quaternion.identity);
        
        // reset timer
        zombieDistance = Random.Range(spawnDistanceRange.x, spawnDistanceRange.y);
    }

    public void SpawnTen()
    {
        for (int i = 0; i < 10; i++)
        {
            zombiePool.spawn(new Vector3(Random.Range(-roadWidth/2, roadWidth/2), 1, 50), Quaternion.identity);
        }
    }

    void SpawnObstacle()
    {
        obstaclePool.spawn(new Vector3(Random.Range(-roadWidth/2, roadWidth/2), 0, 50), Quaternion.identity);
        
        // reset timer
        obstacleDistance = Random.Range(spawnDistanceRange.x, spawnDistanceRange.y);
    }
    
    void SpawnPole()
    {
        telephonePolePool.spawn(new Vector3(-roadWidth*1.3f, 0, 50), Quaternion.identity);
        // reset timer
        poleDistance = interPoleDistance;
    }
}

// TODO s
/* 
 * Reset the wallet (PlayerPrefs.DeleteAll() from a debug button or the editor menu Edit > Clear All PlayerPrefs), then play on the phone for three minutes with a stopwatch.
 * Note: time of the first upgrade, coins at three minutes, upgrades bought.
 * Target: first upgrade inside 2 minutes, roughly the fifth by the end of a 10-minute session on the 100 * 1.5^level curve (100, 150, 225, 337, 506). Tune the earn amount, not the curve, until the first target holds.
 * Journal: earn amount before and after, the numbers above, and one sentence on why the upgrade you chose is felt by the player. Lab B replaces the stopwatch with telemetry.
 * switch off Adaptive performance logging before the CA2 build).
 */
//