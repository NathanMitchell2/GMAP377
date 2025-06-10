using UnityEngine;

public class TargetStickToGround : MonoBehaviour
{
    public float upwardRaycast = 1f;
    public float downwardRaycast = 4f;
    public LayerMask groundMask;

    // Update is called once per frame
    void Update()
    {
        // StickToGround();
    }
    void StickToGround()
    {
        RaycastHit hit;
        // Start the ray higher to avoid hitting the object itself
        if (Physics.Raycast(transform.position + transform.up * upwardRaycast, -transform.up, out hit, downwardRaycast, groundMask))
        {
            transform.position = new Vector3(
                transform.position.x,
                hit.point.y,
                transform.position.z
            );
        }
    }
}
