using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] private HitFlash hitFlash;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyReductionBar;

    public void takeDamage(int dmg)
    {
        health -= dmg;
        hitFlash.TriggerFlash();
        checkDeath();
        updateHealth();
    }
    private void checkDeath()
    {
        if (health <= 0)
        {
            BroadcastMessage("OnDeath");
            killPlayer();
        }
    }

    private void killPlayer()
    {

    }

    public void updateHealth()
    {
        healthBar.fillAmount = health / 100;
    }
}
