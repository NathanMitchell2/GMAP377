using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour
{

    private float health;
    private float maxHealth;

    [SerializeField] private int priority;
    [SerializeField] private float distributeFactor;
    [SerializeField] private bool doOverdamage;

    private List<PlayerHealth> distributes;
    private PlayerHealth parent;
    private HitFlash hitFlash;
    private InventoryItem inventoryItem;

    private void Awake()
    {
        hitFlash = GetComponent<HitFlash>();
    }
    private void Start()
    {
        distributes = new List<PlayerHealth>(GetComponentsInChildren<PlayerHealth>());
        distributes.Remove(this);

        List<PlayerHealth> temp = new List<PlayerHealth>(GetComponentsInParent<PlayerHealth>());
        temp.Remove(this);

        if (temp.Count > 0)
            parent = temp[0];

        if (hitFlash != null)
            hitFlash.TriggerFlash();
        else
            Debug.LogError("No flash");
    }
    public void Initialize(InventoryItem item)
    {
        inventoryItem = item;
        health = item.GetHealth();
        maxHealth = item.GetMaxHealth();
    }
    private void Update()
    {
        if (inventoryItem != null)
        {
            inventoryItem.SetHealth(health);
            HealthCheck();
        }
    }
    public float GetHealth() { return health; }
    public void SetHealth(float health) {  this.health = health; }
    public int GetPriority() { return priority; }
    public float GetDistribute() { return distributeFactor; }
    public PlayerHealth GetParent() { return parent; }

    public void Remove(PlayerHealth distribute) { distributes.Remove(distribute); }


    public float TakeDamage(float damage)
    {
        float overdamage = DistributeDamage(damage);

        if (overdamage != 0 && doOverdamage && parent != null)
        {
            //Can fit Destroy trigger here?
            parent.TakeDamage(overdamage);
            return 0;
        }
        else
        {
            return overdamage;
        }
    }

    private float DistributeDamage(float damage)
    {
        float maxDistrubute = 0;

        foreach (PlayerHealth p in distributes)
        {
            maxDistrubute += p.GetDistribute();
        }

        float selfDistribute = Mathf.Max(1f - maxDistrubute, 0f);
        maxDistrubute += selfDistribute;

        while (damage > 0)
        {
            bool backdamageFlag = true;

            float tDamage = damage;

            foreach (PlayerHealth p in distributes)
            {
                float dDamage = tDamage * (p.GetDistribute() / maxDistrubute);

                float backdamage = p.DistributeDamage(dDamage);

                if (backdamage == 0)
                    backdamageFlag = false;

                damage = damage - dDamage + backdamage;
            }
            
            health -= tDamage * (selfDistribute / maxDistrubute);
            damage -= tDamage * (selfDistribute / maxDistrubute);

            //if (backdamageFlag)
            //    health -= tDamage * (1 - (selfDistribute / maxDistrubute));

            if (health < 0)
            {
                float temp = health;
                health = 0;
                return damage - temp;
            }
        }
        return 0;
    }

    public float Heal(float healing)
    {
        health += healing;

        if (health > maxHealth)
        {
            float overheal = health - maxHealth;
            health = maxHealth;

            float maxDistrubute = 0;

            foreach (PlayerHealth p in distributes)
            {
                maxDistrubute += p.GetDistribute();
            }

            while (overheal > 0)
            {
                bool backHealFlag = true;
                foreach (PlayerHealth p in distributes)
                {
                    float dHeal = overheal * (p.GetDistribute() / maxDistrubute);

                    float backheal = p.Heal(dHeal);

                    if(backheal == 0)
                        backHealFlag = false;

                    overheal -= dHeal + backheal;
                }

                if (backHealFlag)
                    return overheal;
            }
        }
        return 0;
    }

    private void HealthCheck()
    {
        if (health < 1)
        {
            Debug.Log("Part is deceased");
            SendMessage("Death");
            health = 0;
        }

        if(health != 0 && hitFlash != null)
        {
            hitFlash.playSpeed = GetFlashSpeed();
        }
    }

    private float GetFlashSpeed()
    {
        return (maxHealth / health) - 1;
    }

    void OnDestroy()
    {
        if (parent != null)
        {
            parent.Remove(this);
        }
    }
}
