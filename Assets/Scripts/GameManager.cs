using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager current; // singleton
    public enum GameState { Playing, Paused, GameOver } 
    public GameState state;
    public event System.Action<GameState> StateChanged;

    [FormerlySerializedAs("playerMovement")] public PlayerCarController playerCar; // Player will assign itself at start.
    [SerializeField] private float startSpeed;
    [SerializeField] private float startWidth;
    [SerializeField] private Vector2 spawnDistanceRange;
    
    public static float currentSpeed;
    public static float roadWidth;
    
    float zombieDistance; // distance until next zombie spawn
    float obstacleDistance; // 

    private Pool zombiePool, obstaclePool, itemPool;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject[] obstaclePrefabs; // todo perhaps convert to multiple small pools later, one for each specific obstacle type, and one large pool for basic props
    
    // public ObjectPool<item>
    // public ObjectPool<Obstacle> // 2 of each variant should be good
    
    void Awake() // Called before Start
    {
        if(current != null) Destroy((this));
        current = this;
        
        currentSpeed = startSpeed;
        roadWidth = startWidth;
        
        // initialise pools
        zombiePool = new Pool(zombiePrefab);
        zombiePool.CreatePool(10);
        obstaclePool = new Pool(obstaclePrefabs);
        obstaclePool.CreatePool(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (LifecycleGuard.IsPaused) return;
        // if in play
        // loop timers for spawning stuff
        // if spawn,
        // randomise spawning type/num based on difficulty
        
        zombieDistance += Time.deltaTime * currentSpeed;
        if(zombieDistance <= 0) SpawnZombie();
        obstacleDistance += Time.deltaTime * currentSpeed;
        if(obstacleDistance <= 0) SpawnObstacle();
    }

    public void NewGame() //TODO
    {
        // Initialise Pools
        zombiePool.Reset();
        // reset values & difficulty
        // Check playerCar exists
        // playerCar.reset
        
        // place starting obstacles / zombies
        // start obstacle interval timer
        
        // play
        state = GameState.Playing;
        StateChanged?.Invoke(GameState.Playing); 
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
        _ = SlowDown(destroyCancellationToken); // assigns awaitable to var so the following code can continue
        
        // stop object spawning (might be dependent on scrolling anyway, TBD)
        
        
        // play death animation/scene (allowing the camera to move if needed)
        
        state = GameState.GameOver;
        StateChanged?.Invoke(GameState.GameOver);
        
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

        Debug.Log("The float has finally hit 0!");
    }
    

    void SpawnZombie()
    {
        zombiePool.spawn(new Vector3(Random.Range(-roadWidth/2, roadWidth/2), 0, 20), Quaternion.identity);
        
        // reset timer
        zombieDistance = Random.Range(spawnDistanceRange.x, spawnDistanceRange.y);
    }

    public void SpawnTen()
    {
        for (int i = 0; i < 10; i++)
        {
            zombiePool.spawn(new Vector3(Random.Range(-roadWidth/2, roadWidth/2), 0, 20), Quaternion.identity);
        }
    }

    void SpawnObstacle()
    {
        obstaclePool.spawn(new Vector3(Random.Range(-roadWidth/2, roadWidth/2), 0, 20), Quaternion.identity);
        
        // reset timer
        obstacleDistance = Random.Range(spawnDistanceRange.x, spawnDistanceRange.y);
    }
}
