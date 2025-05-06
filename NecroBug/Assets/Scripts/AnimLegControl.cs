using UnityEngine;
using UnityEngine.UIElements;

public class AnimLegControl : MonoBehaviour
{
    [Header("Variables")]

    public float upwardRaycast;
    public float downwardRaycast;
    public float distanceBeforeSnap;
    public float distanceFromRoot;
    public float moveTime;
    public AnimationCurve heightCurve;

    private float tipDistance = 1;
    private bool isMoving = false;
    // private Vector3 direction;
    private Vector3 startPosition;
    private float timer = 0;

    [field: Header("References")]

    public GameObject root;
    public GameObject tip;
    public GameObject tipController;

    void Update()
    {
        // MoveLegController() triggered in TrackDistance()
        TrackDistance();
        StickToGround();

        if (isMoving)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / moveTime);

            Vector3 flatPosition = Vector3.Lerp(startPosition, transform.position, t);
            float heightOffset = heightCurve.Evaluate(t);

            Vector3 finalPosition = new Vector3(flatPosition.x, flatPosition.y + heightOffset, flatPosition.z);

            tipController.transform.position = finalPosition;

            if (t >= 1f)
            {
                isMoving = false;
            }
        }

        timer += 1 * Time.deltaTime;
        // Visualize the ray in Scene view
        Debug.DrawRay(transform.position + transform.up * upwardRaycast, -transform.up * downwardRaycast, Color.red);
    }

    void StickToGround()
    {
        RaycastHit hit;
        // Start the ray higher to avoid hitting the object itself
        if (Physics.Raycast(transform.position + transform.up * upwardRaycast, -transform.up, out hit, downwardRaycast))
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
        tipDistance = Vector3.Distance(transform.position, tipController.transform.position);
        if (tipDistance > distanceBeforeSnap && isMoving == false)
        {
            // direction = (tipController.transform.position - transform.position) / moveTime;
            // Debug.Log(direction);
            // MoveLegController(direction);
            MoveLegController();
        }

        if ( Vector3.Distance(transform.position, root.transform.position) != distanceFromRoot)
        {
            // Vector3.MoveTowards(transform.position, root.transform.position, 1);
        }
    }

    void MoveLegController()
    {
        startPosition = tipController.transform.position;
        timer = 0f;
        isMoving = true;

        if (Vector3.Distance(startPosition, transform.position) > distanceBeforeSnap + 0.1f)
        {
            moveTime = moveTime / (Vector3.Distance(startPosition, transform.position) * 2);
            distanceBeforeSnap = distanceBeforeSnap / (Vector3.Distance(startPosition, transform.position) * 2);
            // Debug.Log(moveTime);
        }
        else
        {
            // Because of this statement, you cannot control the moveTime and distanceBeforeSnap in the unity editor public variables
            moveTime = 0.15f;
            distanceBeforeSnap = 0.7f;
        }
    }
}
