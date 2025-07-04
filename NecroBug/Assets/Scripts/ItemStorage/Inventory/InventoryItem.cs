using UnityEditorInternal.Profiling.Memory.Experimental;
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
    public virtual System.Type GetItemType()
    {
        return typeof(InventoryItem);
    }
    public void Reduce()
    {
        count = 1;
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
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite icon;
    protected float health = 100;
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected bool stackable = false;
    [SerializeField] protected int maxStack = 99;
    [SerializeField] protected int count = 1;
    private bool inUse = false;

    public void SetUse(bool use)
    {
        this.inUse = use;
    }
    public bool GetUse()
    {
        return inUse;
    }

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

    public virtual ItemMemento CreateMemento()
    {
        return new ItemMemento(itemName, icon, health, maxHealth, stackable, maxStack, count);
    }
    public virtual void RestoreMemento(ItemMemento item)
    {
        SuperItemRestore(item);
    }
    protected void SuperItemRestore(ItemMemento item)
    {
        SetName(item.GetName());
        SetIcon(item.GetIcon());
        SetMaxHealth(item.GetMaxHealth());
        SetHealth(item.GetHealth());
        SetStackable(item.GetStackable());
        SetMaxStack(item.GetMaxStack());
        SetCount(item.GetCount());
    }
    public bool CompareTypes(InventoryItem other)
    {
        return
            GetName() == other.GetName() &&
            GetIcon() == other.GetIcon() &&
            GetHealth() == other.GetHealth() &&
            GetMaxHealth() == other.GetMaxHealth() &&
            GetStackable() == other.GetStackable() &&
            GetMaxStack() == other.GetMaxStack();
    }

    public bool Combine(InventoryItem other)
    {
        if (!GetStackable() || !CompareTypes(other))
            return false;

        int maxCountInc = Mathf.Min(GetMaxStack() - GetCount(), other.GetCount());
        AddCount(maxCountInc);
        other.RemoveCount(maxCountInc);

        return true;
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

        if (count > maxStack || count < 0)
            return false;

        if (count <= 0)
            DestroyItem();

        this.count = count;
        return true;

    }
    public bool AddCount()
    {
        return AddCount(1);
    }
    public bool AddCount(int amount)
    {
        if (stackable == true && count+amount <= maxStack)
        {
            count += amount;
            return true;
        }
        return false;
    }

    public bool RemoveCount()
    {
        return RemoveCount(1);
    }

    public bool RemoveCount(int amount)
    {
        if (count-amount < 0)
        {
            return false;
        }
        count -= amount;
        if (count <= 0)
            DestroyItem();
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
    public virtual void DestroyItem()
    {
        Destroy(this);
    }
    private void SetMaxHealth(float maxHealth) { this.maxHealth = maxHealth; }

    public bool GetStackable() { return this.stackable; }
    private void SetStackable(bool stackable) { this.stackable = stackable; }

    public int GetMaxStack() { return maxStack; }
    private void SetMaxStack(int mStack) { this.maxStack = mStack; }

}

