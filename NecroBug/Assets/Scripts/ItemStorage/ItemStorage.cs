using System.Collections.Generic;
using UnityEngine;

public class StorageMemento
{
    private List<ItemMemento> items;
    public StorageMemento(List<ItemMemento> items)
    {
        this.items = items;
    }
    public List<ItemMemento> GetItems() { return items; }
}
public abstract class ItemStorage : MonoBehaviour
{
    public StorageMemento CreateMemento()
    {
        List<ItemMemento> items = new List<ItemMemento>();
        foreach (InventoryItem item in GetItemList())
        {
            items.Add(item.CreateMemento());
        }
        return new StorageMemento(items);
    }
    public void RestoreMemento(StorageMemento memento)
    {
        List<InventoryItem> items = GetItemList();
        foreach (InventoryItem item in items)
        {
            item.DestroyItem();
        }
        List<ItemMemento> mItems = memento.GetItems();
        foreach (var item in mItems)
        {
            InventoryItem tItem = (InventoryItem)gameObject.AddComponent(item.GetItemType());
            tItem.RestoreMemento(item);
        }
    }
    public List<InventoryItem> GetItemList()
    {
        return new List<InventoryItem>(GetComponents<InventoryItem>());
    }
    public virtual InventoryItem AddItem(ItemMemento item)
    {
        if (item == null)
            return null;
        InventoryItem tItem = (InventoryItem)gameObject.AddComponent(item.GetItemType());
        tItem.RestoreMemento(item);

        List<InventoryItem> compare = GetSameItems(tItem);
        foreach (InventoryItem compareItem in compare)
        {
            if (compareItem.Combine(tItem))
            {
                if (tItem == null)
                    return compareItem;
            }
        }
        return tItem;
    }
    public ItemMemento GetMemento(InventoryItem item)
    {
        if (!GetItemList().Contains(item))
            return null;
        ItemMemento mItem = item.CreateMemento();
        mItem.Reduce();
        return mItem;
    }
    public bool RemoveItem(InventoryItem item)
    {
        if (!GetItemList().Contains(item))
            return false;
        item.RemoveCount();
        return true;
        
    }
    public int IndexOf(InventoryItem item)
    {
        return GetItemList().IndexOf(item);
    }

    protected List<InventoryItem> GetSameItems(InventoryItem source)
    {
        List<InventoryItem> sameItems = new List<InventoryItem>();
        foreach (var item in GetItemList())
        {
            if (source.CompareTypes(item) && source != item)
                sameItems.Add(item);
        }
        return sameItems;
    }

}
