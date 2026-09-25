using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnMult;
    public float driveForce;
    public float currentTurn;

    private Rigidbody body;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();
        GameManager.current.playerMovement = this;
    }

    // Update is called once per frame
    void Update()
    {
        body.AddForce(transform.forward * driveForce);
        transform.Rotate(0, currentTurn * turnMult, 0);
        //transform.Rotate(0, Input.GetAxisRaw("Horizontal"), 0);
    }
    
    void Turn(float amount)
    {
        transform.Rotate(0, amount, 0);
    }
}
