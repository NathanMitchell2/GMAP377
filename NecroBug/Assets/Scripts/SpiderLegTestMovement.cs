using UnityEngine;

public class SpiderLegTestMovement : MonoBehaviour
{
    [Header("Floating")]
    public float floatingHeight = 2f;
    public float floatForce = 10f;
    public float floatDamping = 5f;
    public LayerMask groundMask;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float turnSpeed = 90f;

    private Rigidbody rb;
    private Vector3 moveInput;

    Vector3 lastMoveDirection = Vector3.forward;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        // Input (WASD or joystick)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Camera-relative movement
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;

        moveInput = (camForward.normalized * v + camRight.normalized * h).normalized;

       
    }

    void FixedUpdate()
    {
        HandleFloating();
        HandleMovement();
        AlignToGround();
    }

    void HandleFloating()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, floatingHeight * 2f, groundMask))
        {
            float currentHeight = hit.distance;
            float difference = floatingHeight - currentHeight;
            float verticalSpeed = rb.linearVelocity.y;
            float force = (difference * floatForce) - (verticalSpeed * floatDamping);

            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
        }
    }

    void HandleMovement()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 desiredVelocity = moveInput * moveSpeed;
            desiredVelocity.y = rb.linearVelocity.y; // preserve vertical motion from floating
            rb.linearVelocity = desiredVelocity;
        }
        else
        {
            // Stop movement instantly if no input
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    void AlignToGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, floatingHeight * 2f, groundMask))
        {
            Vector3 groundNormal = hit.normal;

            Vector3 forward;

            if (moveInput.sqrMagnitude > 0.01f)
            {
                lastMoveDirection = moveInput;
                forward = Vector3.ProjectOnPlane(moveInput, groundNormal).normalized;
            }
            else
            {
                forward = Vector3.ProjectOnPlane(lastMoveDirection, groundNormal).normalized;
            }

            if (forward.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(forward, groundNormal);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
    }
}
