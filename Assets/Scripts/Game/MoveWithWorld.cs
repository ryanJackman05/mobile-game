using UnityEngine;

// Simple movement script to be applied to all props, Obstacles, Zombies, and Powerups.
// Road will be handled differently
public class MoveWithWorld : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < -10 && !gameObject.CompareTag("Player")) gameObject.SetActive(false);
        
        if (!LifecycleGuard.IsPaused)
        {
            transform.Translate(0, 0, GameManager.currentSpeed * Time.deltaTime, Space.World); // move object, independent from own movement/rotation.
        }
    }
}
