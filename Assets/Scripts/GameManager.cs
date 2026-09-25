using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current; // singleton
    public PlayerMovement playerMovement;
    
    void Awake() // Called before Start
    {
        if(current != null) Destroy((this));
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
