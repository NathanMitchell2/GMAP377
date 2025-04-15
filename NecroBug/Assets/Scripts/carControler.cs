using UnityEngine;
using UnityEngine.InputSystem;

public class carControler : MonoBehaviour
{
    public Rigidbody rigid;
    public WheelCollider wheel1, wheel2, wheel3, wheel4;
    public float driverSpeed, steerSpeed;
    float horizontalInput, verticalInput;
    public bool wingsPickedUp = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate() {
        float motor = Input.GetAxis("Vertical") * driverSpeed;
        wheel1.motorTorque = motor;
        wheel2.motorTorque = motor;
        wheel3.motorTorque = motor;
        wheel4.motorTorque = motor;
        wheel1.steerAngle = steerSpeed * horizontalInput;
        wheel2.steerAngle = steerSpeed * horizontalInput;
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed && wingsPickedUp==true)
        {
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 10f, rigid.linearVelocity.y);
        }
    }

    void OnClick(InputValue value)
    {
        ModularBugPart clickPart = GetComponentInChildren<ModularBugPart>();
        if (value.isPressed && clickPart)
        {
            clickPart.Activate();
        }
    }

}
