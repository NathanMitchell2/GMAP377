
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class carControler : MonoBehaviour
{
    private const float ogMass = 1500;
    public Rigidbody rigid;
    public WheelCollider wheel1, wheel2, wheel3, wheel4;
    public float driverSpeed, steerSpeed;
    public float baseDriverSpeed, baseSteerSpeed;
    float horizontalInput, verticalInput;
    public bool wingsPickedUp = false;

    public float totalMass;

    void Awake()
    {
        baseDriverSpeed = driverSpeed;
        baseSteerSpeed = steerSpeed;
    }

    public void TerrainSpeedDown(float drive, float steer)
    {
        if (GetComponentInChildren<LegsPartIdentifier>() == null)
        {
            driverSpeed = baseDriverSpeed * drive;
            steerSpeed = baseSteerSpeed * steer;
        }
    }
    public void TerrainSpeedReset()
    {
        driverSpeed = baseDriverSpeed;
        steerSpeed = baseSteerSpeed;
    }
    public void setDriverSpeed(float speed)
    {
        driverSpeed = speed;
    }
    public void setSteerSpeed(float speed)
    {
        steerSpeed = speed;
    }

    // Update is called once per frame
    void Update() {
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
    }
    public void SetInputs(float hInput, float vInput)
    {
        horizontalInput = hInput;
        verticalInput = vInput;
    }

    void FixedUpdate() {
        float totalMass = 0;
        List<Rigidbody> bodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        for (int i = 0; i < bodies.Count; i++)
        {
            totalMass += bodies[i].mass;
        }



        float motor = verticalInput * driverSpeed * ogMass/totalMass;
        wheel1.motorTorque = motor;
        wheel2.motorTorque = motor;
        wheel3.motorTorque = motor;
        wheel4.motorTorque = motor;
        wheel1.steerAngle = steerSpeed * horizontalInput;
        wheel2.steerAngle = steerSpeed * horizontalInput;
    }

    void OnJump(InputValue value)
    {
        /*
        if (value.isPressed && wingsPickedUp==true)
        {
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 10f, rigid.linearVelocity.y);
        }
        */
    }

    void OnClick(InputValue value)
    {
        /*
        ModularBugPart clickPart = GetComponentInChildren<ModularBugPart>();
        if (value.isPressed && clickPart)
        {
            clickPart.Activate();
        }
        */
    }

}
