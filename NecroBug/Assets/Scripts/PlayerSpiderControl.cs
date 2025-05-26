using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpiderMovementController : MonoBehaviour
{
    [Header("General Settings")]
    public bool spiderModeActive = false;
    public float bodyHeight = 2f;
    public float liftSmoothing = 5f;

    [Header("Movement Settings")]
    public float moveForce = 30f;
    public float maxSpeed = 10f;
    public float turnSpeed = 5f;

    [Header("Leg References (Optional)")]
    public Transform[] legTips;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        // Input
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Toggle spider mode
        if (Input.GetKeyDown(KeyCode.T))
        {
            spiderModeActive = !spiderModeActive;
            Debug.Log("Spider Mode: " + (spiderModeActive ? "Enabled" : "Disabled"));
        }
    }

    void FixedUpdate()
    {
        if (!spiderModeActive) return;

        ApplyFloating();
        ApplyMovement();
    }

    void ApplyFloating()
    {
        Vector3 targetPosition;

        if (legTips != null && legTips.Length > 0)
        {
            Vector3 avgLegPos = Vector3.zero;
            foreach (var leg in legTips)
                avgLegPos += leg.position;
            avgLegPos /= legTips.Length;

            targetPosition = avgLegPos + Vector3.up * bodyHeight;
        }
        else
        {
            // Fall back if no legs assigned
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                targetPosition = hit.point + Vector3.up * bodyHeight;
            }
            else
            {
                targetPosition = transform.position + Vector3.up * 0.1f;
            }
        }

        // Smoothly move toward target height
        Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * liftSmoothing);
        rb.MovePosition(newPos);
    }

    void ApplyMovement()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            // Calculate move force in local space
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0;
            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0;

            Vector3 moveDir = (camForward.normalized * moveInput.z + camRight.normalized * moveInput.x).normalized;

            // Apply movement force
            if (rb.linearVelocity.magnitude < maxSpeed)
                rb.AddForce(moveDir * moveForce, ForceMode.Acceleration);

            // Rotate body toward movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * turnSpeed));
        }
        else
        {
            // Smoothly damp velocity to a stop
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);
            rb.angularVelocity = Vector3.zero;
        }
    }

}
