using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public Vector3 platformDelta;

    public void ApplyPlatformDelta(Vector3 delta)
    {
        platformDelta += delta;
    }

    public Vector3 ConsumePlatformDelta()
    {
        Vector3 delta = platformDelta;
        platformDelta = Vector3.zero;
        return delta;
    }
}
