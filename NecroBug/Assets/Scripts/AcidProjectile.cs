using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;
    public PlayerStats playerStats;
    public ParticleSystem particle;
    public GameObject splashEffect;
    public EnemyAI acidBeetle;
    public bool baseProjectile = true;

    private void Start() => Destroy(gameObject, lifeTime);

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy") return;
        else if (other.gameObject.tag == "Player")
        {
            // Debug.Log(other.gameObject);
            PlayerStats playerStats = other.gameObject.GetComponentInParent<PlayerStats>();
            acidBeetle.DealAcidDamage(damage, playerStats);
        }
        // DOES NOT DESTROY THE PARTICLE, JUST STOPS IT. Persists forever in the scene, removing parent necessary for animation
        if (baseProjectile)
        {
            particle.Stop();
            Instantiate(splashEffect, gameObject.transform.position, Quaternion.identity);
            particle.transform.parent = null;
        }
        
        // Destroys the acid prefab, not the particle
        // Debug.Log("collision");
        Destroy(gameObject);
    }
}
