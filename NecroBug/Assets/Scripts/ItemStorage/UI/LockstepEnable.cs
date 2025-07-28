using System.Collections.Generic;
using UnityEngine;

public class LockstepEnable : MonoBehaviour
{
    [SerializeField] private List<GameObject> followers;

    private void OnEnable()
    {
        foreach (GameObject go in followers)
        {
            go.SetActive(true);
        }
    }
    private void OnDisable()
    {
        foreach (GameObject go in followers)
        {
            go.SetActive(false);
        }
    }

}
