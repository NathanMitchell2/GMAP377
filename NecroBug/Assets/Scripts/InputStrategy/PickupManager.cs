using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    private Dictionary<GameObject, int> pickups = new Dictionary<GameObject, int>();

    private InputManager manager;
    private StorageManager storageManager;
    private void Start()
    {
        manager = transform.parent.GetComponent<InputManager>();
        storageManager = GetComponentInParent<StorageManager>();
    }
    void OnTriggerEnter(Collider c)
    {
        GameObject other = c.gameObject;
        if(other.GetComponent<PickupIdentifier>())
        {
            if (pickups.ContainsKey(other))
                pickups[other]++;
            else
                pickups.Add(other, 1);
        }
    }
    void OnTriggerExit(Collider c)
    {
        GameObject other = c.gameObject;
        if (other.GetComponent<PickupIdentifier>())
        {
            if (pickups[other] == 0)
                pickups.Remove(other);
            else
                pickups[other]--;
        }
    }

    public void pickup()
    {
        foreach(var kvp in pickups)
        {
            //pickups.Remove(pickup);
            GameObject pickup = kvp.Key;
            GameObject item = pickup.GetComponent<PickupIdentifier>().item;
            if (item != null)
            {
                ItemMemento mem = item.GetComponent<InventoryItem>().CreateMemento();

                storageManager.AddItem(mem, StorageManager.StorageKey.Inventory);
            }
            else if (pickup.GetComponent<BodyPickup>() != null) {
                pickup.GetComponent<BodyPickup>().Pickup();
                continue;
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
