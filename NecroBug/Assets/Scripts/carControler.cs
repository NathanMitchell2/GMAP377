
using System.Collections;
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
    public SpiderLegTestMovement legScript;
    public carControler carScript;

    public List<GameObject> wheelList;

    private bool legsDetected = false;

    public float totalMass;

    private Coroutine slowRoutine;
    void Awake()
    {
        baseDriverSpeed = driverSpeed;
        baseSteerSpeed = steerSpeed;
        rigid.centerOfMass = new Vector3(0, -0.5f, 0);
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
    void Update()
    {
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        if (GetComponentInChildren<LegsPartIdentifier>() != null && legsDetected == false)
        {
            Debug.Log("Leg Found");
            legScript.enabled = true;
            for (int i = 0; i < wheelList.Count; i++)
            {
                Debug.Log(wheelList[i] + " disabled");
                wheelList[i].SetActive(false);
            }
            carScript.enabled = false;
            legsDetected = true;
        }
    }
    public void SetInputs(float hInput, float vInput)
    {
        horizontalInput = hInput;
        verticalInput = vInput;
    }

    void FixedUpdate()
    {
        float totalMass = 0;
        List<InventoryItem> parts = PlayerIdentifier.GetPlayer().GetComponent<StorageManager>().GetItemList(StorageManager.StorageKey.BuilderBuffer);

        foreach (InventoryItem part in parts)
        {
            totalMass += ((MediatorPart)part).GetMass();
        }



        float motor = verticalInput * driverSpeed * ogMass / totalMass;
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


    public void ApplySlow(float duration, float slowFactor)
    {
        // Stop any existing slow effect
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowEffect(duration, slowFactor));
    }

    private IEnumerator SlowEffect(float duration, float slowFactor)
    {
        float originalDrive = driverSpeed;
        float originalSteer = steerSpeed;

        driverSpeed *= slowFactor;
        steerSpeed *= slowFactor;

        yield return new WaitForSeconds(duration);

        driverSpeed = baseDriverSpeed;
        steerSpeed = baseSteerSpeed;

        slowRoutine = null;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MovingPlat"))
        {
            transform.SetPositionAndRotation(transform.position + other.gameObject.GetComponent<MovingPlatform>().GetVelocity(), transform.rotation);
            
            MovingPlatform platform = other.gameObject.GetComponent<MovingPlatform>();
        }
    }
}
