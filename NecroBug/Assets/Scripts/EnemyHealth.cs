using UnityEngine;
using FMODUnity;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] private HitFlash hitFlash;

    [SerializeField]
    private EventReference damageSound;
    public event Action<int> Damaged;


    public void dealDamage(int dmg)
    {
        health -= dmg;
        hitFlash.TriggerFlash();

        Damaged?.Invoke(dmg);   

        if (damageSound.IsNull == false)
        {
            RuntimeManager.PlayOneShot(damageSound, Camera.main.transform.position);
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
