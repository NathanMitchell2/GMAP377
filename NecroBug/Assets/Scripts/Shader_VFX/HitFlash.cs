using UnityEngine;
using System.Collections;

public class HitFlash : MonoBehaviour
{
    public Material hitFlashMaterial;
    public float flashDuration = 0.5f;
    public float playSpeed = 1.0f;

    private MeshRenderer flashRenderer;
    private GameObject flashMeshObject;
    private Material runtimeMaterial;
    private Coroutine flashCoroutine;

    void Start()
    {
        // New flash mesh created as child of original object
        flashMeshObject = new GameObject("HitFlashMesh");
        flashMeshObject.transform.SetParent(transform);
        flashMeshObject.transform.localPosition = Vector3.zero;
        flashMeshObject.transform.localRotation = Quaternion.identity;
        flashMeshObject.transform.localScale = Vector3.one;

        // Copy mesh from original object
        MeshFilter originalFilter = GetComponent<MeshFilter>();
        if (originalFilter == null)
        {
            Debug.LogError("No MeshFilter found on current object");
            return;
        }

        // Copies mesh filter onto secondary mesh
        MeshFilter flashFilter = flashMeshObject.AddComponent<MeshFilter>();
        flashFilter.sharedMesh = originalFilter.sharedMesh;

        // Copies mesh renderer and material onto secondary mesh
        flashRenderer = flashMeshObject.AddComponent<MeshRenderer>();
        runtimeMaterial = Instantiate(hitFlashMaterial);
        flashRenderer.material = runtimeMaterial;
        flashRenderer.receiveShadows = false;

        // Disable until used
        flashRenderer.enabled = false;
        runtimeMaterial.SetFloat("_FlashAmount", 0f);
    }

    // Call Trigger Flash to trigger the effect
    public void TriggerFlash()
    {
        if (flashRenderer == null || runtimeMaterial == null) return;

        if(flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        flashRenderer.enabled = true;
        runtimeMaterial.SetFloat("_FlashAmount", 1f);

        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - ((playSpeed * (elapsed / flashDuration)) % 1);
            runtimeMaterial.SetFloat("_FlashAmount", t);
            yield return null;
        }

        runtimeMaterial.SetFloat("_FlashAmount", 0f);
        flashRenderer.enabled = false;
    }
}
