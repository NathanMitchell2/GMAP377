using System.Collections.Generic;
using UnityEngine;

// Add Memento Here and maybe in subclasses for build checkpoints and prebuilds
public abstract class ItemStorage : MonoBehaviour
{
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

    private List<InventoryItem> GetSameItems(InventoryItem source)
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
