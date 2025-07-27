using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Prebuilder : MonoBehaviour
{
    private class PrebuildStorageMemento : MonoBehaviour
    {
        private List<ItemMemento> items;
        public PrebuildStorageMemento(List<ItemMemento> items)
        {
            this.items = items;
        }
        public List<ItemMemento> GetItems() { return items; }
    }
    BotBulider builder;
    StorageManager storageManager;
    Dictionary<string, PrebuildStorageMemento> storageMementos = new Dictionary<string, PrebuildStorageMemento>();

    private void Awake()
    {
        GameObject player = PlayerIdentifier.GetPlayer().gameObject;
        builder = player.GetComponentInChildren<BotBulider>();
        storageManager = player.GetComponentInChildren<StorageManager>();
    }
    private PrebuildStorageMemento CreateStorageMemento()
    {
        List<BotPart> parts = builder.GetPartList();
        List<ItemMemento> items = new List<ItemMemento>();

        foreach (BotPart part in parts)
        {
            items.Add(part.GetMediator().CreateMemento());
        }

        return new PrebuildStorageMemento(items);
    }
    public void ReSetPrebuild(string name)
    {
        if (storageMementos.ContainsKey(name))
            storageMementos[name] = CreateStorageMemento();
        else
            storageMementos.Add(name, CreateStorageMemento());
    }
    public bool AddPrebuild(string name)
    {
        if (storageMementos.ContainsKey(name))
            return false;

        storageMementos.Add(name, CreateStorageMemento());

        return true;
    }

    public bool RemovePrebuild(string name)
    {
        return storageMementos.Remove(name);
    }

    public void UsePrebuild(string name)
    {
        Debug.LogError("InPrebuild");
        if (!storageMementos.ContainsKey(name))
            return;
        Debug.LogError("past return");

        foreach (BotPart part in builder.GetPartList())
        {
            Debug.LogError("removing");
            builder.RemovePart(builder.IndexOf(part));
        }
        builder.CreateBot();

        foreach(ItemMemento memItem in storageMementos[name].GetItems())
        {
            InventoryItem invItem = storageManager.GetStorage(StorageManager.StorageKey.Inventory).GetItem(memItem);
            if (invItem != null)
            {
                Debug.LogError("adding");
                InventoryItem tempItem = storageManager.Transfer(invItem, StorageManager.StorageKey.Inventory, StorageManager.StorageKey.BotBuilder);
                tempItem.RestoreMemento(memItem);
            }
        }

        Debug.LogError("EndPrebuild");
    }
}
