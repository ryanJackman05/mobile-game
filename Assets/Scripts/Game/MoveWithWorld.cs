using UnityEngine;

// Simple movement script to be applied to all props, Obstacles, Zombies, and Powerups.
// Road will be handled differently
public class MoveWithWorld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!LifecycleGuard.IsPaused)
        {
            transform.Translate(0, 0, GameManager.currentSpeed * Time.deltaTime); // move object, independent from own movement.
        }
    }
}
