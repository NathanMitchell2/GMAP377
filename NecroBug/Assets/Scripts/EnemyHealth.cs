using UnityEngine;
using FMODUnity;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] private HitFlash hitFlash;

    [SerializeField]
    private EventReference damageSound;

    public void dealDamage(int dmg)
    {
        health -= dmg;
        hitFlash.TriggerFlash();

        if(damageSound.IsNull == false)
        {
            RuntimeManager.PlayOneShot(damageSound, Camera.main.transform.position);
            //Debug.Log("Damage sound played");
        }
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
