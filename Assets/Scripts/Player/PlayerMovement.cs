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
        // TODO use addForceAtpoint instead? OR manipulate car rig via scripts, depending on how car ends up being built.
    }
    // might be needed to assist lifting the car on a jump ramp
    void Jump()
    {
        // apply upward force at front of car to lift front
        // possibly remove mass/gravity temporarily
    }

    void StopJump() // alternative is to set a timer or coroutine
    {
        // reset gravity values and begin correcting rotation of car, perhaps dampen the landing movement with more mass
    }
    
    void Turn(float amount)
    {
        transform.Rotate(0, amount, 0);
    }
}
