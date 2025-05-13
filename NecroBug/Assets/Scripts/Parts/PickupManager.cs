using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    private List<Collider> pickups = new List<Collider>();

    private InputManager manager;
    private void Start()
    {
        manager = transform.parent.GetComponent<InputManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ModularBugPart>())
        {
            //Destroy(other);
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
            Destroy(pickup.gameObject);
            //pickup.transform.parent = transform.parent;
            //manager.SetStrat(pickup.GetComponent<InputStrategy>());
            //pickup.GetComponent<ModularBugPart>().OffsetPosition();
            //pickups.Remove(pickup);
        }
    }
}
