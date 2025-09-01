using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour
{

    public float health;
    private float maxHealth;

    [SerializeField] private int priority;
    [SerializeField] private float distributeFactor;
    [SerializeField] private bool doOverdamage;

    private GameObject cameraObject;

    private List<PlayerHealth> distributes;
    private PlayerHealth parent;
    private HitFlash hitFlash;
    private InventoryItem inventoryItem;

    private MenuChanger menuChanger;


    [SerializeField] private bool damage;
    [SerializeField] private bool kill;

    private void Awake()
    {
        hitFlash = GetComponent<HitFlash>();
        cameraObject = GameObject.Find("/Camera");
        menuChanger = cameraObject.GetComponentInChildren<MenuChanger>();
        
    }
    private void Start()
    {
        distributes = new List<PlayerHealth>(GetComponentsInChildren<PlayerHealth>());
        distributes.Remove(this);

        List<PlayerHealth> temp = new List<PlayerHealth>(GetComponentsInParent<PlayerHealth>());
        List<PlayerHealth> temptemp = new List<PlayerHealth>();

        foreach (PlayerHealth item in temp)
        {
            if (item.gameObject == this.gameObject)
                temptemp.Add(item);
        }
        foreach(PlayerHealth item in temptemp)
        {
            temp.Remove(item);
        }


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
        if (damage)
        {
            health--;
            damage = false;
        }
        if (kill)
        {
            DoDeath();
            kill = false;
        }
    }
    public float GetHealth() { return health; }
    public void SetHealth(float health) {  this.health = health; }
    public float GetMaxHealth() { return maxHealth; }
    public int GetPriority() { return priority; }
    public float GetDistribute() { return distributeFactor; }
    public PlayerHealth GetParent() { return parent; }

    public void Remove(PlayerHealth distribute) { distributes.Remove(distribute); }
    public bool Has(PlayerHealth distribute) { return distributes.Contains(distribute); }


    public float TakeDamage(float damage)
    {
        float overdamage = DistributeDamage(damage);

        if (overdamage != 0 && doOverdamage && parent != null)
        {
            //Can fit Destroy trigger here?
            //parent.Remove(this);
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

            int intialCount = distributes.Count;

            for(int i = 0; i < distributes.Count; i++) {
                if(distributes.Count != intialCount)
                {
                    i -= intialCount - distributes.Count;
                    intialCount = distributes.Count;
                }
                PlayerHealth p = distributes[i];

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
                DoDeath();
                return 0;
                return damage - temp;
            }
        }
        if (health < 0)
        {
            DoDeath();
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

                float tHeal = overheal;

                foreach (PlayerHealth p in distributes)
                {
                    float dHeal = tHeal * (p.GetDistribute() / maxDistrubute);

                    float backheal = p.Heal(dHeal);

                    if (backheal == 0)
                    {
                        backHealFlag = false;
                    }

                    overheal = overheal - dHeal + backheal;
                }

                if (backHealFlag)
                {
                    return overheal;
                }
            }
        }
        return 0;
    }

    private void HealthCheck()
    {
        if (health < 1)
        {
            DoDeath();
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
    void DoDeath()
    {
        Debug.Log("Part is deceased");
        SendMessage("Death");
        menuChanger.SetMenu(6);
        health = 0;
        if (parent != null && parent.Has(this))
        {
            parent.Remove(this);
        }
    }
}
