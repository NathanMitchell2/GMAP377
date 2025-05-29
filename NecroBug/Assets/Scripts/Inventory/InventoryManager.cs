using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<InventoryItem> items;
    [SerializeField] private GameObject NecroBugBotPart;
    [SerializeField] private GameObject TestBotPart;

    private void Awake()
    {
        items = new List<InventoryItem>();
        items.Add(NecroBugBotPart.GetComponentInChildren<InventoryThing>().GetItem());
        items.Add(TestBotPart.GetComponentInChildren<InventoryThing>().GetItem());
    }

    public void AddItem(InventoryItem item) { items.Add(item); }
    public void RemoveItem(InventoryItem item) { items.Remove(item); }
    public InventoryItem GetItem(int index) {
        return items[index];
    }
    public int Count() { return items.Count; }
}
