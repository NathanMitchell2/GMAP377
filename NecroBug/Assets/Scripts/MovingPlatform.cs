using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 velocity;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        velocity = transform.position - previousPosition;
        previousPosition = transform.position;
    }

    // player script gets the platform's velocity from here
    public Vector3 GetVelocity()
    {
        return velocity;
    }
}
