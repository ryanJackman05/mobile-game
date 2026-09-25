using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current; // singleton
    public PlayerMovement playerMovement;
    public float currentSpeed;
    // public ObjectPool<Zom>
    // public ObjectPool<item>
    // public ObjectPool<Obstacle> // 2 of each variant should be good
    
    void Awake() // Called before Start
    {
        if(current != null) Destroy((this));
        current = this;
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
