using UnityEngine;

public class WallHealth : MonoBehaviour
{
    [SerializeField] protected int health = 50;
    [SerializeField] private HitFlash hitFlash;

    public void dealDamage(int dmg)
    {
        health -= dmg;
        if(hitFlash != null)
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
