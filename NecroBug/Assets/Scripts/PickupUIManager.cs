using System.Collections.Generic;
using UnityEngine;

public class PickupUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pickupUI;
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public void SetItems(List<InventoryItem> newList)
    {
        inventoryItems = newList;
        UpdateItems();
    }
    private void UpdateItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        foreach (InventoryItem item in inventoryItems)
        {
            GameObject temp = Instantiate(pickupUI,transform);
            temp.GetComponent<PickupUI>().SetItem(item.CreateMemento());
        }
    }
}
