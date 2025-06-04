using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<InventoryItem> items;
    [SerializeField] private GameObject NecroBugBotPart;
    [SerializeField] private List<InventoryThing> defaultItems;

    private void Awake()
    {
        items = new List<InventoryItem>();
        items.Add(NecroBugBotPart.GetComponentInChildren<InventoryThing>().GetItem().Copy());
        foreach (InventoryThing inventoryThing in defaultItems)
        {
            items.Add(inventoryThing.GetItem());
        }
    }

    public void AddItem(InventoryItem item) {
        List<InventoryItem> compare = GetItems(item);
        foreach (InventoryItem compareItem in compare)
        {
            //some type of while count of item >=1 loop
            if (compareItem.AddCount())
                return;
        }
        items.Add(item);
    }
    public void RemoveItem(InventoryItem item) 
    {
        if (!item.RemoveCount())
            items.Remove(item);
    }

    private List<InventoryItem> GetItems(InventoryItem source)
    {
        List<InventoryItem> sameItems = new List<InventoryItem>();
        foreach (var item in this.items)
        {
            if(source.Equals(item))
                sameItems.Add(item);
        }
        return sameItems;
    }
    public InventoryItem GetItem(int index) {
        return items[index];
    }
    public int Count() { return items.Count; }
}
