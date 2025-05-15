using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] private HitFlash hitFlash;

    public void dealDamage(int dmg)
    {
        health -= dmg;
        hitFlash.TriggerFlash();
        checkDeath();
    }
    private void checkDeath()
    {
        if(health <= 0)
        {
            BroadcastMessage("OnDeath");
            Destroy(gameObject);
        }
    }
}
