using UnityEngine;

public class InventoryItem
{
    private string itemName;
    private Sprite icon;
    private float health;
    private float maxHealth;
    private bool stackable = false;
    private int maxStack = 99;
    private int count = 1;

    public InventoryItem(string itemName, Sprite icon, float health, bool stackable, int maxStack)
    {
        this.itemName = itemName;
        this.icon = icon;
        this.health = health;
        this.maxHealth = health;
        this.stackable = stackable;
        this.maxStack = maxStack;
    }
    public InventoryItem Copy()
    {
        InventoryItem copy = new InventoryItem(itemName, icon, maxHealth, stackable, maxStack);
        copy.SetHealth(health);
        return copy;
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
    public bool SetCount(int count)
    {
        if (!stackable)
            return false;

        if (count >= maxStack || count < 1)
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
        Debug.Log("Setting Health");
        Debug.Log(health);
        this.health = health;
    }
}
