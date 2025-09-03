using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 velocity;
    public Vector3 deltaPosition { get; private set; }
    public Quaternion deltaRotation { get; private set; }

    private void Start()
    {
        previousPosition = transform.position;
        previousRotation = transform.rotation;
        BugopediaEvents.frenepedeDiscovered = true;
    }

    private void Update()
    {
        velocity = transform.position - previousPosition;
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        deltaPosition = transform.position - previousPosition;
        deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);

        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    // player script gets the platform's velocity from here
    public Vector3 GetVelocity()
    {
        return velocity;
    }
}
