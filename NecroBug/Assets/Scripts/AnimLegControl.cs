using UnityEngine;

public class AnimLegControl : MonoBehaviour
{
    [Header("Variables")]

    public float upwardRaycast;
    public float downwardRaycast;
    public float distanceBeforeSnap;
    public float moveTime;

    private float distance = 1;
    private bool movementCompleted = true;

    [field: Header("References")]

    public GameObject tip;
    public GameObject tipController;

    void Update()
    {
        TrackDistance();
        StickToGround();

        // Visualize the ray in Scene view
        Debug.DrawRay(transform.position + Vector3.up * upwardRaycast, Vector3.down * downwardRaycast, Color.red);
    }

    void StickToGround()
    {
        RaycastHit hit;
        // Start the ray higher to avoid hitting the object itself
        if (Physics.Raycast(transform.position + Vector3.up * upwardRaycast, Vector3.down, out hit, downwardRaycast))
        {
            transform.position = new Vector3(
                transform.position.x,
                hit.point.y,
                transform.position.z
            );
            // Debug.Log("Raycast hit: " + hit.point);
        }
    }

    void TrackDistance()
    {
        distance = Vector3.Distance(transform.position, tipController.transform.position);
        if (distance > distanceBeforeSnap)
        {
            MoveLegController();
        }
    }

    void MoveLegController()
    {
        tipController.transform.position = transform.position;
    }
}
