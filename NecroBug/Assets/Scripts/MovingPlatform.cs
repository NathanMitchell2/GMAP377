using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 previousPosition;
    private List<PlatformMover> moversOnPlatform = new List<PlatformMover>();

    void Start()
    {
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 delta = transform.position - previousPosition;

        foreach (PlatformMover mover in moversOnPlatform)
        {
            if (mover != null)
            {
                //mover.AddPlatformDelta(delta);
            }
        }

        previousPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsPlayerOnTop(collision))
        {
            PlatformMover mover = collision.collider.GetComponent<PlatformMover>();
            if (mover != null && !moversOnPlatform.Contains(mover))
            {
                moversOnPlatform.Add(mover);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        PlatformMover mover = collision.collider.GetComponent<PlatformMover>();
        if (mover != null && moversOnPlatform.Contains(mover))
        {
            moversOnPlatform.Remove(mover);
        }
    }

    private bool IsPlayerOnTop(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return false;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                return true;
        }

        return false;
    }
}
