using System.Collections;
using UnityEngine;
using FMODUnity; 

public class PlayerIdleSound : MonoBehaviour
{
    [SerializeField] private EventReference idleSoundEvent; 
    [SerializeField] private float idleTime = 5f; 

    private Vector3 lastPosition;
    private float idleTimer = 0f;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        CheckPlayerMovement();
    }

    private void CheckPlayerMovement()
    {
        if (transform.position == lastPosition)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTime)
            {
                PlayFMODSound();
                idleTimer = 0f; 
            }
        }
        else
        {
            idleTimer = 0f; 
            lastPosition = transform.position;
        }
    }

    private void PlayFMODSound()
    {
        if (idleSoundEvent.IsNull)
        {
            Debug.LogWarning("FMOD Event Reference is not assigned.");
            return;
        }

        RuntimeManager.PlayOneShotAttached(idleSoundEvent, gameObject); 
    }
}
