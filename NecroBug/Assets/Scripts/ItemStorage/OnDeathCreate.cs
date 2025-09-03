using NUnit.Framework;
using UnityEngine;

public class OnDeathCreate : MonoBehaviour
{
    [SerializeField] private GameObject objectToCreate;

    void Death()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.name.Contains("Model"))
            {
                MeshRenderer[] renderers = child.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer renderer in renderers)
                {
                    if (renderer.gameObject.activeInHierarchy)
                    {
                        Instantiate(objectToCreate, renderer.bounds.center, Quaternion.identity);
                        return;
                    }
                }
            }
        }
        Instantiate(objectToCreate, transform.position, transform.rotation);
    }
}
