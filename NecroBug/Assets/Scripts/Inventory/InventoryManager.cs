using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> items;
    [SerializeField] private GameObject invObj;


    private void Awake()
    {
        items = new List<InventoryItem>();
        //items.Add(NecroBugBotPart.GetComponentInChildren<InventoryItem>().GetItem());
        List<InventoryItem> defaultItems = new List<InventoryItem>(invObj.GetComponents<InventoryItem>());
        foreach (InventoryItem inventoryThing in defaultItems)
        {
            items.Add(inventoryThing.GetItem());
        }
    }
    public List<InventoryItem> Copy()
    {
        Debug.LogError("InventoryManager Copy Unimplemented");
        return new List<InventoryItem>();//this.IdealClone(items);
    }

    public void Replace(List<InventoryItem> inventoryItems)
    {
        Debug.LogError("InventoryManager Replace Unimplemented");
        //items = this.IdealClone(inventoryItems);
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
        Debug.LogError("InventoryManager RemoveItem Unimplemented");
        return item;

        //Commented out for compile, is part of original code
        /*
        InventoryItem temp = item.Copy();

        temp.SetCount(1);

        if (!item.RemoveCount())
            items.Remove(item);
        return temp;
        */
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

    public GameObject GetInvObj()
    {
        return invObj;
    }
}
