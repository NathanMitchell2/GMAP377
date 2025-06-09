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

        // Optional: rotate to face move direction
        if (moveInput.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }
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
            float verticalSpeed = rb.velocity.y;
            float force = (difference * floatForce) - (verticalSpeed * floatDamping);

            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
        }
    }

    void HandleMovement()
    {
        Vector3 moveForce = moveInput * moveSpeed;
        rb.AddForce(moveForce, ForceMode.Acceleration);
    }

    void AlignToGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, floatingHeight * 2f, groundMask))
        {
            Vector3 groundNormal = hit.normal;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
