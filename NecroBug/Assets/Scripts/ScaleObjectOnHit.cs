using UnityEngine;
using System.Collections;

public class ScaleObjectOnHit : MonoBehaviour
{
    public Transform target; // The object to scale
    public float duration = 1f; // Duration of the lerp

    private Coroutine scaleCoroutine;

    void Start()
    {
        if (target == null)
        {
            target = transform; // Default to this object if no target is set
        }
    }

    public void StartScaling()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }

        scaleCoroutine = StartCoroutine(ScaleDownCoroutine());
    }

    private IEnumerator ScaleDownCoroutine()
    {
        Vector3 initialScale = target.localScale;
        Vector3 targetScale = new Vector3(0f, 0f, initialScale.z);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newX = Mathf.Lerp(initialScale.x, targetScale.x, t);
            float newY = Mathf.Lerp(initialScale.y, targetScale.y, t);
            target.localScale = new Vector3(newX, newY, initialScale.z);
            yield return null;
        }

        // Ensure final scale is exactly the target
        target.localScale = targetScale;
        scaleCoroutine = null;
        Destroy(this);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<AcidProjectile>() != null){
            StartCoroutine("ScaleDownCoroutine");
        }
    }
}