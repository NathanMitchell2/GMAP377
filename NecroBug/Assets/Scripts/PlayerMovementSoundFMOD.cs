using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(NewCarMovementSystem))]
public class PlayerMovementSoundFMOD : MonoBehaviour
{
    [SerializeField]
    private EventReference movementSoundEvent;

    public float movementThreshold = 1f; // Adjust as needed to avoid idle noise

    private EventInstance movementSoundInstance;
    private Rigidbody bodyRb;
    private bool isSoundPlaying = false;

    void Start()
    {
        bodyRb = GetComponent<Rigidbody>();

        if (movementSoundEvent.IsNull)
        {
            Debug.LogWarning("FMOD EventReference is not assigned.");
            return;
        }

        movementSoundInstance = RuntimeManager.CreateInstance(movementSoundEvent);
        movementSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    void Update()
    {
        float speed = bodyRb.velocity.magnitude;
        bool isMoving = speed > movementThreshold;

        movementSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

        if (isMoving && !isSoundPlaying)
        {
            movementSoundInstance.start();
            isSoundPlaying = true;
        }
        else if (!isMoving && isSoundPlaying)
        {
            movementSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isSoundPlaying = false;
        }
    }

    void OnDestroy()
    {
        movementSoundInstance.release();
    }
}
