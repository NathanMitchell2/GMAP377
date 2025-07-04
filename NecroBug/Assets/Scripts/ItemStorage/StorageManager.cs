using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        ItemMemento mem = GetMemento(item, keyFrom);
        if (mem == null)
            return null;

        Debug.Log("Into Remove");
        if (RemoveItem(item, keyFrom))
        {
            Debug.Log("Into Add");
            return AddItem(mem, keyTo);
        }
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
