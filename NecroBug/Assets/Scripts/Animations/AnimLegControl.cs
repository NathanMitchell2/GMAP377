using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class AnimLegControl : MonoBehaviour
{
    [Header("Variables")]

    public float upwardRaycast;
    public float downwardRaycast;
    public float defaultSnapDistance;
    public float distanceFromRoot;
    public float defaultMoveTime;
    public AnimationCurve heightCurve;
    public float postResetDelay = 0.05f;
    [SerializeField] private float snapDistanceVariance = 0f;
    public float overshootDistance = 0.4f;

    public int legIndex; // 0,1,2,3,4,5,6,7 for spiders

    private float resetCooldown = 0f;
    private float tipDistance = 1;
    public bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 defaultTipLocalPosition;
    private float timer = 0;
    private float resetTimer = 0f;
    private float distanceBeforeSnap;
    private float moveTime;

    [field: Header("Raycast Settings")]
    public LayerMask groundMask;

    [field: Header("References")]

    public GameObject root;
    public GameObject tip;
    public GameObject tipController;
    public GameObject car;

    private void Awake()
    {
        if (tipController != null)
        {
            // Debug.Log("tip_controller not null");
            defaultTipLocalPosition = transform.localPosition;
        }
        else
        {
            defaultTipLocalPosition = transform.localPosition;
        }
        distanceBeforeSnap = defaultSnapDistance + defaultSnapDistance*Random.Range(-snapDistanceVariance,snapDistanceVariance);
        moveTime = defaultMoveTime;
        root.GetComponentInChildren<RigBuilder>().Build();
    }
    void Update()
    {
        // MoveLegController() triggered in TrackDistance()
        TrackDistance();
        StickToGround();

        if (isMoving)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / moveTime);

            // Direction of the step in world space
            Vector3 moveDirection = (transform.position - startPosition).normalized;

            // Overshoot target
            Vector3 overshootTarget = transform.position + moveDirection * overshootDistance;

            // Lerp toward overshoot position instead of exact position
            Vector3 flatPosition = Vector3.Lerp(startPosition, overshootTarget, t);
            float heightOffset = heightCurve.Evaluate(t);

            Vector3 finalPosition = new Vector3(flatPosition.x, flatPosition.y + heightOffset, flatPosition.z);

            tipController.transform.position = finalPosition;

            if (t >= 1f)
            {
                isMoving = false;
            }
        }

        timer += 1 * Time.deltaTime;

        if(resetTimer >= 1)
        {
            ResetTipController();
            resetTimer = 0;
        }
        // Visualize the ray in Scene view
        // Debug.DrawRay(transform.position + transform.up * upwardRaycast, -transform.up * downwardRaycast, Color.red);


        if (resetCooldown > 0f)
        {
            resetCooldown -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetTipController();
        }
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

    void TrackDistance()
    {
        tipDistance = Vector3.Distance(transform.position, tipController.transform.position);
        if (tipDistance > distanceBeforeSnap && isMoving == false && resetCooldown <= 0f)
        {
            MoveLegController();
        }

        if (Vector3.Distance(transform.localPosition, defaultTipLocalPosition) > distanceBeforeSnap)
        {
            // Debug.Log(transform.localPosition + " + " + defaultTipLocalPosition);
            resetTimer += 1 * Time.deltaTime;
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
        }
        else
        {
            moveTime = defaultMoveTime;
            distanceBeforeSnap = defaultSnapDistance + defaultSnapDistance * Random.Range(-snapDistanceVariance, snapDistanceVariance);
        }
    }

    public void ResetTipController()
    {

        transform.SetParent(root.transform);
        transform.localPosition = defaultTipLocalPosition;
        transform.localRotation = Quaternion.identity;
        isMoving = false;
        timer = 0f;
        resetCooldown = postResetDelay;
    }
}
