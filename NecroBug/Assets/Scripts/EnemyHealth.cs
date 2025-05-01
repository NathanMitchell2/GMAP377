using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] protected int health = 100;

    public void dealDamage(int dmg)
    {
        health -= dmg;
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
