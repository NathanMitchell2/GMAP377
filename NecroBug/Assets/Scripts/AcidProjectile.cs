using System.Collections.Generic;
using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;
    public float splashRange = .5f;
    public PlayerStats playerStats;
    public ParticleSystem particle;
    public GameObject splashEffect;
    public EnemyAI acidBeetle;
    public bool baseProjectile = true;
    public bool isPlayer = false;
    public int acidDamage = 35;

    private void Start() => Destroy(gameObject, lifeTime);

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
        if (other.gameObject.tag == "Enemy" && isPlayer)
        {
            EnemyHealth enemy = other.transform.GetComponent<EnemyHealth>();
            enemy.dealDamage(acidDamage);
        }
        else if (other.gameObject.tag == "Player")
        {
            // Debug.Log(other.gameObject);

            List<GameObject> targets = new List<GameObject>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, splashRange);

            foreach (Collider hit in colliders)
            {
                if (hit.gameObject.tag == "Player")
                {
                    targets.Add(hit.gameObject);
                }
            }
            GetComponent<DamageObject>().Damage(DamageObject.GetPlayerHealths(targets));
            //PlayerStats playerStats = other.gameObject.GetComponentInParent<PlayerStats>();
            //acidBeetle.DealAcidDamage(damage, playerStats);
        }
        // DOES NOT DESTROY THE PARTICLE, JUST STOPS IT. Persists forever in the scene, removing parent necessary for animation
        
        particle.Stop();
        Instantiate(splashEffect, gameObject.transform.position, Quaternion.identity);
        particle.transform.parent = null;
        
        
        // Destroys the acid prefab, not the particle
        // Debug.Log("collision");
        Destroy(gameObject);
    }
}
