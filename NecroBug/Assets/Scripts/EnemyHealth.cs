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
    public string enemyType;

    private BugopediaEvents events = new BugopediaEvents();


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
            setKilledStatus(enemyType);
            BroadcastMessage("OnDeath");
            Destroy(gameObject);
        }
    }
    private void setKilledStatus(string enemyType){
        if(enemyType == "jetBeetle"){
            BugopediaEvents.jetBeetleKilled = true;
        }
        else if(enemyType == "acidBeetle"){
            BugopediaEvents.acidBeetleKilled = true;
        }
        else if (enemyType == "spider"){
            BugopediaEvents.spiderKilled = true;
        }
        else if (enemyType== "bee"){
            BugopediaEvents.beeKilled = true;
        }
    }
}
