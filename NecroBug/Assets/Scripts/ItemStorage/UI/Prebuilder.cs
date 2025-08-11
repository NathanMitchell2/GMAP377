using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Prebuilder : MonoBehaviour
{
    private class PrebuildStorageMemento
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
    static Dictionary<string, PrebuildStorageMemento> storageMementos = new Dictionary<string, PrebuildStorageMemento>();


    private bool working = false;

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
        if (!storageMementos.ContainsKey(name))
            return;

        if (!working)
        {
            foreach (BotPart part in builder.GetPartList())
            {
                builder.RemovePart(builder.IndexOf(part));
            }
            StartCoroutine(FrameAdvanceTransfer(name));
        }

    }
    private IEnumerator HardCodeLockout()
    {
        working = true;
        yield return new WaitForSecondsRealtime(.5f);
        working = false;
    }
    private IEnumerator FrameAdvanceTransfer(string name)
    {
        working = true;
        yield return new WaitForEndOfFrame();

        builder.CreateBot();

        foreach (ItemMemento memItem in storageMementos[name].GetItems())
        {
            InventoryItem invItem = storageManager.GetStorage(StorageManager.StorageKey.Inventory).GetItem(memItem);
            if (invItem != null)
            {
                InventoryItem tempItem = storageManager.Transfer(invItem, StorageManager.StorageKey.Inventory, StorageManager.StorageKey.BotBuilder);
                tempItem.RestoreMemento(memItem);
            }
        }
        GetComponent<BuilderUI>().UpdateAll();

        StartCoroutine(HardCodeLockout());

        yield return null;
    }
}
