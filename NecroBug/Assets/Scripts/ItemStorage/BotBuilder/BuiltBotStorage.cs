using UnityEngine;

public class BuiltBotStorage : ItemStorage
{
    public override InventoryItem AddItem(ItemMemento item)
    {
        if (item == null)
            return null;
        InventoryItem tItem = (InventoryItem)gameObject.AddComponent(item.GetItemType());
        tItem.RestoreMemento(item);
        return tItem;
    }
}
