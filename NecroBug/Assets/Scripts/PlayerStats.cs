using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int health = 100;
    [SerializeField] private HitFlash hitFlash;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyReductionBar;

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"[After Damage] Health: {health}");
        hitFlash.TriggerFlash();
        CheckDeath();
        UpdateHealth();
    }
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

    }

    public void UpdateHealth()
    {
        // Debug.Log(health);
        healthBar.fillAmount = (float)health / 100;
    }
}
