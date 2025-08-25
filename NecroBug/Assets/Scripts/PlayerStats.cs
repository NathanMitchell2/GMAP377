using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    /*
    public float health = 100;
    [SerializeField] private HitFlash hitFlash;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyReductionBar;
    private InventoryItem inventoryItem;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void Initialize(Image healthBar, Image energyBar, Image energyReductionBar)
    {
        this.healthBar = healthBar;
        this.energyBar = energyBar;
        this.energyReductionBar = energyReductionBar;

        if (inventoryItem != null)
            playerHealth.SetHealth(inventoryItem.GetHealth());
        else
            Debug.LogWarning("PlayerStats: inventoryItem not set before Initialize().");

        UpdateHealth();
    }
    private void Update()
    {
        health = playerHealth.GetHealth();
        UpdateHealth();
    }
    */
    
    // this should be // out I'm just lazy
    public void TakeDamage(int dmg)
    {
        //DamageObject.Damage(dmg, playerHealth);
        /*
        health -= dmg;
        Debug.Log($"[After Damage] Health: {health}");

        if (hitFlash != null)
        {
            hitFlash.TriggerFlash();
        }
        CheckDeath();
        UpdateHealth();
        */
    }

    /*
    
    private void CheckDeath()
    {
        if (health <= 0)
        {
            // BroadcastMessage("OnDeath");
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateHealth()
    {
        //Debug.Log("Updated Health: " + health);
        inventoryItem.SetHealth(health);
    }

    public void SetItem(InventoryItem item)
        { this.inventoryItem = item; }
    */
}
