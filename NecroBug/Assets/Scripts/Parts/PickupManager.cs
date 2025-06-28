using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [SerializeField] InventoryManager inventory;

    private List<Collider> pickups = new List<Collider>();

    private InputManager manager;
    private void Start()
    {
        manager = transform.parent.GetComponent<InputManager>();
        inventory = GetComponentInParent<InventoryManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PickupIdentifier>())
        {
            //Destroy(other);
            pickups.Add(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PickupIdentifier>())
        {
            pickups.Remove(other);
        }
    }

    public void pickup()
    {
        foreach(Collider pickup in pickups)
        {
            //pickups.Remove(pickup);
            GameObject item = pickup.GetComponent<PickupIdentifier>().item;
            if(item != null)
            {
                System.Type type = item.GetComponent<InventoryItem>().GetType();
                InventoryItem temp = (InventoryItem)inventory.GetInvObj().AddComponent(type);
                temp.RestoreMemento(item.GetComponent<InventoryItem>().CreateMemento());
            }
            Destroy(pickup.gameObject);
            //pickup.transform.parent = transform.parent;
            //manager.SetStrat(pickup.GetComponent<InputStrategy>());
            //pickup.GetComponent<ModularBugPart>().OffsetPosition();
            //pickups.Remove(pickup);
        }

        pickups.Clear();
    }
}
