using UnityEngine;

public class WallHealth : MonoBehaviour
{
    [SerializeField] protected int health = 25;
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
            Destroy(gameObject);
        }
    }
}
