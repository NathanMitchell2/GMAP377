using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : ItemStorage
{
    public List<InventoryItem> FilterInUse()
    {
        List<InventoryItem> temp = GetItemList();
        List<InventoryItem> output = new List<InventoryItem>();

        foreach(InventoryItem item in temp)
        {
            if (!item.GetUse())
                output.Add(item);
        }
        return output;
    }
    /*
    public List<InventoryItem> items;
    [SerializeField] private GameObject invObj;
    [SerializeField] private GameObject activatedItemsObj;


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
    public void RemoveItem(InventoryItem item) 
    {
        if (!items.Contains(item))
            return;

        items.Remove(item);

        //Commented out for compile, is part of original code
        /*
        InventoryItem temp = item.Copy();

        temp.SetCount(1);

        if (!item.RemoveCount())
            items.Remove(item);
        return temp;
        
    }

    public InventoryItem ActivateItem(InventoryItem item)
    {
        ItemMemento temp = item.CreateMemento();

        System.Type type = item.GetType();
        InventoryItem activeItem = (InventoryItem)activatedItemsObj.AddComponent(type);

        if (!item.RemoveCount())
            items.Remove(item);

        activeItem.RestoreMemento(temp);
        activeItem.SetCount(1);

        return activeItem;
    }
    public void RecoverActiveItem(InventoryItem item)
    {
        bool flag = true;
        ItemMemento recoverMemento = item.CreateMemento();
        for (int i = 0; i < items.Count; i++)
        {
            if (recoverMemento.Combine(items[i].CreateMemento()))
            {
                flag = false;
                break;
            }
        }

        if(flag)
        {
            InventoryItem invTrans = (InventoryItem)invObj.AddComponent(item.GetType());
            invTrans.RestoreMemento(recoverMemento);
        }

        Debug.LogError("InventoryManager RecoverActiveItem will destroy any active bug parts");

        Destroy(item);
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
    */

}
