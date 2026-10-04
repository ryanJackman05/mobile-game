using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!Debug.isDebugBuild)
        {
            this.enabled = false;
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        GameManager.current.playerCar.currentTurn = move.x;
    }
}
