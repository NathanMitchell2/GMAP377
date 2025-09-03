using UnityEngine;

public class PartFlash : MonoBehaviour
{
    [SerializeField] private MeshFilter originalFilter;
    [SerializeField] private Material hitFlashMaterial;
    [SerializeField] private float maxFlashPerSecond = 10;

    private PlayerHealth health;

    private MeshRenderer flashRenderer;
    private GameObject flashMeshObject;
    private Material runtimeMaterial;

    void Start()
    {
        // New flash mesh created as child of original object
        flashMeshObject = new GameObject("HitFlashMesh");

        // Copy mesh from original object
        if (originalFilter == null)
            originalFilter = GetComponentInChildren<MeshFilter>();
        if (originalFilter == null)
        {
            Debug.LogError("No MeshFilter found on current object");
            return;
        }
        flashMeshObject.transform.SetParent(originalFilter.transform);
        flashMeshObject.transform.localPosition = Vector3.zero;
        flashMeshObject.transform.localRotation = Quaternion.identity;
        flashMeshObject.transform.localScale = Vector3.one;

        // Copies mesh filter onto secondary mesh
        MeshFilter flashFilter = flashMeshObject.AddComponent<MeshFilter>();
        flashFilter.sharedMesh = originalFilter.sharedMesh;

        // Copies mesh renderer and material onto secondary mesh
        flashRenderer = flashMeshObject.AddComponent<MeshRenderer>();
        runtimeMaterial = Instantiate(hitFlashMaterial);
        flashRenderer.material = runtimeMaterial;
        flashRenderer.receiveShadows = false;

        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (!(flashRenderer == null || runtimeMaterial == null || health == null))
        {
            float ratio = health.GetHealth() / health.GetMaxHealth();

            float fps = Mathf.Lerp(maxFlashPerSecond, 0, ratio);

            runtimeMaterial.SetFloat("_fps", fps);
        }
    }
}
