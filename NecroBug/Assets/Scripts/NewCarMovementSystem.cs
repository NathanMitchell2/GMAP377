using UnityEngine.InputSystem;
using UnityEngine;
public class NewCarMovementSystem : MonoBehaviour
{
    public WheelCollider leftFront;
    public WheelCollider leftRear;
    public WheelCollider rightFront;
    public WheelCollider rightRear;

    public float motorTorque = 1500f;
    public float brakeTorque = 3000f;
    public float maxSteerAngle = 15f; // Optional small steering if needed
    Rigidbody bodyRb ;


    private float moveInput;
    private float turnInput;


    void Start()
    {
        bodyRb = gameObject.GetComponent<Rigidbody>();
        bodyRb.centerOfMass = new Vector3(0, -1f, 0);
    }
    void Update()
    {
        moveInput = Input.GetAxis("Vertical");   // W/S keys
        turnInput = Input.GetAxis("Horizontal"); // A/D keys
    }

    void FixedUpdate()
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.1f;

        // Apply Motor Torque
        leftFront.motorTorque = moveInput * motorTorque;
        leftRear.motorTorque = moveInput * motorTorque;
        rightFront.motorTorque = moveInput * motorTorque;
        rightRear.motorTorque = moveInput * motorTorque;

        // Apply Brake Torque if not moving
        if (!isMoving)
        {
            leftFront.brakeTorque = brakeTorque;
            leftRear.brakeTorque = brakeTorque;
            rightFront.brakeTorque = brakeTorque;
            rightRear.brakeTorque = brakeTorque;
        }
        else
        {
            leftFront.brakeTorque = 0f;
            leftRear.brakeTorque = 0f;
            rightFront.brakeTorque = 0f;
            rightRear.brakeTorque = 0f;
        }

        // Turning (optional slight steering at front wheels)
        leftFront.steerAngle = turnInput * maxSteerAngle;
        rightFront.steerAngle = turnInput * maxSteerAngle;
    }
}

