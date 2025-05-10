using UnityEngine;

public class AutoTriggerFlash : MonoBehaviour
{
    public HitFlash flashEffect; // Reference to the HitFlashEffect script on the same or another object
    public float interval = 1f;        // Time between flashes in seconds

    void Start()
    {
        // Start calling TriggerFlash() every `interval` seconds
        InvokeRepeating(nameof(CallFlash), 0f, interval);
    }

    void CallFlash()
    {
        if (flashEffect != null)
        {
            flashEffect.TriggerFlash();
        }
    }
}
