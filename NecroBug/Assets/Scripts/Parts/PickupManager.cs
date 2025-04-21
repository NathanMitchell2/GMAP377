using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    private List<Collider> pickups = new List<Collider>();
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ModularBugPart>())
        {
            pickups.Add(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<ModularBugPart>())
        {
            pickups.Remove(other);
        }
    }

    public void pickup()
    {
        foreach(Collider pickup in pickups)
        {
            pickup.transform.parent = transform.parent;
            InputManager manager = transform.parent.GetComponent<InputManager>();
            manager.SetStrat(pickup.GetComponent<InputStrategy>());
            pickup.GetComponent<ModularBugPart>().OffsetPosition();
        }
    }
}
