using UnityEngine;

public class InventoryThing : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private bool stackable = false;
    [SerializeField] private int maxStack = 99;
    [SerializeField] private int count = 1;

    private InventoryItem item;
    private void Awake()
    {
        item = null;
        //item = new InventoryItem(itemName,icon,health,stackable,maxStack);
    }

    public InventoryItem GetItem() {
        if(item == null)
        {
            item = new InventoryItem(itemName, icon, maxHealth, stackable, maxStack);
            item.SetCount(count);
        }
        return item;
    }
    public void SetItem(InventoryItem item) { this.item = item; }
    public string GetName()
    {
        return item.GetName();
    }
    public Sprite GetIcon()
    {
        return item.GetIcon();
    }

    public bool AddCount()
    {
        return item.AddCount();
    }

    public bool RemoveCount()
    {
        return item.RemoveCount();
    }

    public float GetHealth()
    {
        return item.GetHealth();
    }
    public void SetHealth(float health) { item.SetHealth(health); }
    public float GetMaxHealth() { return item.GetMaxHealth(); }

    public bool GetStackable() { return item.GetStackable(); }
}

