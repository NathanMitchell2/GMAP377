using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int maxPrefabs = 2;
    private int currentPrefabs = 0;

    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Ray ray = new Ray(other.transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Floor") && currentPrefabs < maxPrefabs)
                {
                    SpawnPrefab(hit.point.y);
                }
            }
        }
    }

    private void SpawnPrefab(float floorY)
    {
        if (prefabToSpawn != null)
        {
            Transform spawnTransform = spawnPoint != null ? spawnPoint : transform;

            Vector3 spawnPos = new Vector3(
                spawnTransform.position.x,
                floorY,
                spawnTransform.position.z
            );

            Instantiate(prefabToSpawn, spawnPos, spawnTransform.rotation);
            currentPrefabs++;
            Debug.Log("Spawned bees. Current count: " + currentPrefabs);
        }
    }

    public void PrefabDestroyed()
    {
        currentPrefabs = Mathf.Max(0, currentPrefabs - 1);
        Debug.Log("Bees destroyed. Current count: " + currentPrefabs);
    }
}
