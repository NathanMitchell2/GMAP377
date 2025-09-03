using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int maxPrefabs = 5;
    private int currentPrefabs = 0;

    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentPrefabs < maxPrefabs)
            {
                SpawnPrefab();
            }
            else
            {
                Debug.Log("Maximum number of prefabs reached!");
            }
        }
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn != null)
        {
            Transform spawnTransform = spawnPoint != null ? spawnPoint : transform;
            Instantiate(prefabToSpawn, spawnTransform.position, spawnTransform.rotation);
            currentPrefabs++;
            Debug.Log("Spawned prefab. Current count: " + currentPrefabs);
        }
        else
        {
            Debug.LogWarning("No prefab assigned in PrefabSpawner!");
        }
    }

    public void PrefabDestroyed()
    {
        currentPrefabs = Mathf.Max(0, currentPrefabs - 1);
        Debug.Log("Prefab destroyed. Current count: " + currentPrefabs);
    }
}
