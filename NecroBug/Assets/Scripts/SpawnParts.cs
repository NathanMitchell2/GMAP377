using System.Collections.Generic;
using UnityEngine;

public class SpawnParts : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    void OnDeath()
    {
        foreach (GameObject obj in prefabs)
        {
            Instantiate(obj,transform.position,transform.rotation);
        }
    }
}
