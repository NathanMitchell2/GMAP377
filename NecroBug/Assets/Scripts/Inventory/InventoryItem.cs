using UnityEngine;

public class ItemMemento
{
    private string itemName;
    private Sprite icon;
    private float health;
    private float maxHealth;
    private bool stackable;
    private int maxStack;
    private int count;
    public ItemMemento(string itemName, Sprite icon, float health, float maxHealth, bool stackable, int maxStack, int count)
    {
        this.itemName = itemName;
        this.icon = icon;
        this.health = health;
        this.maxHealth = maxHealth;
        this.stackable = stackable;
        this.maxStack = maxStack;
        this.count = count;
    }
    public string GetName()
    {
        return itemName;
    }

    public Sprite GetIcon()
    {
        return icon;
    }
    public int GetCount()
    {
        return count;
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public bool GetStackable() { return this.stackable; }

    public int GetMaxStack() { return maxStack; }
}
public class InventoryItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private bool stackable = false;
    [SerializeField] private int maxStack = 99;
    [SerializeField] private int count = 1;

    void Awake()
    {
        health = maxHealth;
    }
    /*public void SetItem(InventoryItem item)
    {
        SetName(item.GetName());
        SetIcon(item.GetIcon());
        SetMaxHealth(item.GetMaxHealth());
        SetHealth(item.GetHealth());
        SetStackable(item.GetStackable());
        SetMaxStack(item.GetMaxStack());
        SetCount(item.GetCount());
    }*/

    public ItemMemento CreateMemento()
    {
        return new ItemMemento(itemName,icon,health,maxHealth,stackable,maxStack,count);
    }
    public void RestoreMemento(ItemMemento item)
    {
        SetName(item.GetName());
        SetIcon(item.GetIcon());
        SetMaxHealth(item.GetMaxHealth());
        SetHealth(item.GetHealth());
        SetStackable(item.GetStackable());
        SetMaxStack(item.GetMaxStack());
        SetCount(item.GetCount());
    }
    public InventoryItem GetItem() {
        return this;
    }

    /*public InventoryItem Copy()
    {
        InventoryItem copy = new InventoryItem(itemName, icon, maxHealth, stackable, maxStack);
        copy.SetHealth(health);
        return copy;
    }*/
    public string GetName()
    {
        return itemName;
    }
    private void SetName(string name) { itemName = name; }

    public Sprite GetIcon()
    {
        return icon;
    }
    private void SetIcon(Sprite sprite) { icon = sprite; }
    public int GetCount()
    {
        return count;
    }
    public bool SetCount(int count)
    {
        if (!stackable)
            return false;

        if (count > maxStack || count < 1)
            return false;

        this.count = count;
        return true;

    }
    public bool AddCount()
    {
        if (stackable == true && count < maxStack)
        {
            count++;
            return true;
        }
        return false;
    }

    public bool RemoveCount()
    {
        if (count <= 1)
        {
            return false;
        }
        count--;
        return true;
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetHealth(float health)
    {
        if (health > maxHealth || health < 0)
            return;
        //Debug.Log("Setting Health");
        //Debug.Log(health);
        this.health = health;
    }

    private void SetMaxHealth(float maxHealth) { this.maxHealth = maxHealth; }

    public bool GetStackable() { return this.stackable; }
    private void SetStackable(bool stackable) { this.stackable = stackable; }

    public int GetMaxStack() { return maxStack; }
    private void SetMaxStack(int mStack) { this.maxStack = mStack; }

}

