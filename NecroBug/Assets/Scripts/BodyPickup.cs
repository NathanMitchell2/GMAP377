using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BodyPickup : MonoBehaviour
{
    [SerializeField] private GameObject pickupScreen;
    public void Pickup()
    {
        GameObject temp = Instantiate(pickupScreen);
        temp.GetComponentInChildren<PickupUIManager>().SetItems(GetItems());

        foreach(InventoryItem item in GetItems())
        {
            PlayerIdentifier.GetPlayer().GetComponent<StorageManager>().AddItem(item.CreateMemento(), StorageManager.StorageKey.Inventory);
        }

        Destroy(gameObject);
    }

    private List<InventoryItem> GetItems()
    {
        return new List<InventoryItem>(GetComponents<InventoryItem>());
    }
}
