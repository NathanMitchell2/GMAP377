using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> items;
    [SerializeField] private GameObject NecroBugBotPart;
    [SerializeField] private List<InventoryThing> defaultItems;

    public List<InventoryItem> Copy(){
        return this.Clone(items);
    }

    public void Replace(List<InventoryItem> inventoryItems){
        items = this.Clone(inventoryItems);
    }

    public List<InventoryItem> Clone(List<InventoryItem> items){
        List<InventoryItem> temp = new List<InventoryItem>();

        foreach (InventoryItem item in items){
            temp.Add(item);
        }
        return temp;
    }

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
    public InventoryItem RemoveItem(InventoryItem item) 
    {
        InventoryItem temp = item.Copy();
        temp.SetCount(1);

        if (!item.RemoveCount())
            items.Remove(item);
        return temp;
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
