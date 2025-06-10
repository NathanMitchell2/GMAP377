using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerMovementSoundFMOD : MonoBehaviour
{
    [Header("FMOD Events")]
    public EventReference movementSound;
    public EventReference triggerSound;

    private EventInstance movementInstance;
    private EventInstance triggerInstance;

    private bool isMoving = false;
    private bool hasTriggered = false;

    private Vector3 lastPosition;
    private float movementThreshold = 0.01f; // Adjust as needed

    private Rigidbody trackedRigidbody;

    void Start()
    {
        Debug.Log("[FMOD] Start() called");

        trackedRigidbody = GetComponent<Rigidbody>();
        if (trackedRigidbody == null)
        {
            Debug.LogError("[FMOD] No Rigidbody found for movement tracking!");
            return;
        }

        lastPosition = trackedRigidbody.position;

        if (movementSound.IsNull)
        {
            Debug.LogError("[FMOD] Movement sound event not assigned!");
        }
        else
        {
            movementInstance = RuntimeManager.CreateInstance(movementSound);
            RuntimeManager.AttachInstanceToGameObject(movementInstance, transform);
            Debug.Log("[FMOD] Movement instance created and attached");
        }
    }

    void FixedUpdate()
    {
        if (trackedRigidbody == null)
            return;

        float distanceMoved = (trackedRigidbody.position - lastPosition).magnitude;
        bool currentlyMoving = distanceMoved > movementThreshold;

        Debug.Log($"[FMOD] Distance moved (Rigidbody): {distanceMoved}");

        if (currentlyMoving && !isMoving)
        {
            Debug.Log("[FMOD] Starting movement sound");
            if (movementInstance.isValid())
                movementInstance.start();
            isMoving = true;
        }
        else if (!currentlyMoving && isMoving)
        {
            Debug.Log("[FMOD] Stopping movement sound");
            if (movementInstance.isValid())
                movementInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isMoving = false;
        }
    }

    void LateUpdate()
    {
        if (trackedRigidbody != null)
        {
            lastPosition = trackedRigidbody.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"[FMOD] OnTriggerEnter: {other.gameObject.name}");

        if (!hasTriggered && other.CompareTag("Spooder"))
        {
            hasTriggered = true;

            if (triggerSound.IsNull)
            {
                Debug.LogError("[FMOD] Trigger sound event not assigned!");
                return;
            }

            triggerInstance = RuntimeManager.CreateInstance(triggerSound);
            RuntimeManager.AttachInstanceToGameObject(triggerInstance, transform);
            triggerInstance.start();

            Debug.Log("[FMOD] Trigger sound started and attached to object");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[FMOD] OnTriggerExit: {other.gameObject.name}");

        if (hasTriggered && other.CompareTag("Spooder"))
        {
            if (triggerInstance.isValid())
            {
                triggerInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                triggerInstance.release();
                Debug.Log("[FMOD] Trigger sound stopped and instance released");
            }

            hasTriggered = false; // Optional: allow retriggering if needed
        }
    }


    private void OnDestroy()
    {
        Debug.Log("[FMOD] OnDestroy() - cleaning up instances");

        if (movementInstance.isValid())
        {
            movementInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            movementInstance.release();
        }

        if (triggerInstance.isValid())
        {
            triggerInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            triggerInstance.release();
        }
    }
}
