using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager current; // singleton
    [FormerlySerializedAs("playerMovement")] public PlayerCarController playerCar; // Player will assign itself at start.
    [SerializeField] private float startSpeed;
    [SerializeField] private float startWidth;
    public static float currentSpeed;
    public static float roadWidth;
    
    float zombieDistance; // distance until next zombie spawn

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
    }

    // Update is called once per frame
    void Update()
    {
        // if in play
        // loop timers for spawning stuff
        // if spawn,
        // randomise spawning type/num based on difficulty
        
        zombieDistance += Time.deltaTime * currentSpeed;
        if(zombieDistance <= 0) SpawnZombie();
    }

    void NewGame() //TODO
    {
        // Initialise Pools
        // reset values & difficulty
        // Check playerCar exists
        // playerCar.reset
        
        // place starting obstacles / zombies
        // start obstacle interval timer
        
        // play
    }

    void GameOver()
    {
        // immediately pause scrolling
        // stop object spawning (might be dependant on scrolling anyway, TBD)
        // play death animation/scene (allowing the camera to move if needed)
        
        // bring up end screen menu
        // TODO - Decide if end screen moves straight to a new scene with car ready, or if it lingers on stopped car
    }

    void SpawnZombie()
    {
        zombiePool.spawn(new Vector3(Random.Range(-roadWidth/2, roadWidth/2), 0, 20), Quaternion.identity);
        
        // reset timer
        zombieDistance = Random.Range(20,30);
    }

    void SpawnObstacle()
    {
        
    }
}
