using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager current; // singleton
    [FormerlySerializedAs("playerMovement")] public PlayerCarController playerCar; // Player will assign itself at start.
    [SerializeField] private float startSpeed;
    public static float currentSpeed;

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
        
        // initialise pools
        zombiePool = new Pool(zombiePrefab);
        zombiePool.CreatePool(10);
        obstaclePool = new Pool(obstaclePrefabs);
    }

    // Update is called once per frame
    void Update()
    {
        // if in play
        // for each item{
        // scroll (move) item
        // REPEAT FOR ZOMBIES. Their AI should handle their movement relative to the road
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
}
