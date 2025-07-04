using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    private List<Collider> pickups = new List<Collider>();

    private InputManager manager;
    private StorageManager storageManager;
    private void Start()
    {
        manager = transform.parent.GetComponent<InputManager>();
        storageManager = GetComponentInParent<StorageManager>();
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
                ItemMemento mem = item.GetComponent<InventoryItem>().CreateMemento();

                storageManager.AddItem(mem, StorageManager.StorageKey.Inventory);
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
