using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StorageManagerMemento
{
    [SerializeField] Dictionary<string, StorageMemento> storages = new Dictionary<string, StorageMemento>();

    public StorageManagerMemento(Dictionary<string, ItemStorage> storages)
    {
        foreach (var storage in storages)
        {
            this.storages.Add(storage.Key, storage.Value.CreateMemento());
        }
    }
    public Dictionary<string, StorageMemento> GetStorages() { return this.storages; }
}
public class StorageManager : MonoBehaviour
{
    public enum StorageKey
    {
        Inventory,
        BotBuilder,
        BuilderBuffer
    }

    [SerializeField] Dictionary<string, ItemStorage> storages = new Dictionary<string, ItemStorage>();
    private void Awake()
    {
        List<ItemStorage> storageList = new List<ItemStorage>(GetComponentsInChildren<ItemStorage>());
        foreach (ItemStorage storage in storageList)
        {
            storages.Add(storage.name, storage);
        }
    }
    public StorageManagerMemento CreateMemento()
    {
        return new StorageManagerMemento(storages);
    }
    public void RestoreMemento(StorageManagerMemento memento)
    {
        Dictionary<string, StorageMemento> storages = memento.GetStorages();
        foreach (var storage in storages)
        {
            this.storages[storage.Key].RestoreMemento(storage.Value);
        }
    }
    public ItemStorage GetStorage(StorageKey key)
    {
        return storages[GetKey(key)];
    }
    public List<InventoryItem> GetItemList(StorageKey key)
    {
        return storages[GetKey(key)].GetItemList();
    }
    public InventoryItem AddItem(ItemMemento item, StorageKey key)
    {
        return storages[GetKey(key)].AddItem(item);
    }
    public ItemMemento GetMemento(InventoryItem item, StorageKey key)
    {
        return storages[GetKey(key)].GetMemento(item);
    }
    public bool RemoveItem(InventoryItem item, StorageKey key)
    {
        return storages[GetKey(key)].RemoveItem(item);
    }
    public int IndexOf(InventoryItem item, StorageKey key)
    {
        return storages[GetKey(key)].IndexOf(item);
    }
    public InventoryItem Transfer(InventoryItem item, StorageKey keyFrom, StorageKey keyTo)
    {
        //For recovery... maybe save memento 
        ItemMemento mem = GetMemento(item, keyFrom);
        if (mem == null)
            return null;

        if (RemoveItem(item, keyFrom))
            if (keyFrom == StorageKey.BotBuilder && keyTo == StorageKey.BuilderBuffer)
                return ((BuiltBotStorage)GetStorage(keyTo)).AddItem(mem, false); // This is the sole situation when instant transfer is used
            else
                return AddItem(mem, keyTo);
        return null;
    }

    private string GetKey(StorageKey key)
    {
        switch (key)
        {
            case StorageKey.Inventory:
                return "Inventory";
            case StorageKey.BotBuilder:
                return "BotBuilder";
            case StorageKey.BuilderBuffer:
                return "BuilderBuffer";
        }

        return "";
    }

}
