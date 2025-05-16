using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(NewCarMovementSystem))]
public class PlayerMovementSoundFMOD : MonoBehaviour
{
    [SerializeField]
    private EventReference movementSoundEvent;
    [SerializeField] 
    private EventReference idleSoundEvent;
    private float idleTime = 5f;
    private float idleTimer = 0f;
    public float movementThreshold = 1f; // Adjust as needed to avoid idle noise

    private EventInstance movementSoundInstance;
    private EventInstance idleSoundInstance;

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
        idleSoundInstance = RuntimeManager.CreateInstance(idleSoundEvent);
        idleSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    }

    void Update()
    {
        float speed = bodyRb.linearVelocity.magnitude;
        bool isMoving = speed > movementThreshold;

        movementSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        idleSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

        if (isMoving && !isSoundPlaying)
        {
            movementSoundInstance.start();
            isSoundPlaying = true;
            idleTimer = 0f;
        }
        else if (!isMoving && isSoundPlaying)
        {
            movementSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isSoundPlaying = false;
        }
        else if(!isMoving && !isSoundPlaying)
        { 
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime)
            {
                idleSoundInstance.start();
                idleTimer = 0f;
            }
        }
    }




    void OnDestroy()
    {
        movementSoundInstance.release();
    }
}
