using UnityEngine;

public class AcidSplashProjectile : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 3f;
    public PlayerStats playerStats;
    public ParticleSystem particle;
    public GameObject splashEffect;
    public EnemyAI acidBeetle;
    public bool baseProjectile = false;
    public LayerMask targetLayerMask;

    private void Start() => Destroy(gameObject, lifeTime);

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject);

        if (other.CompareTag("Player"))
        {
            // Debug.Log("Splash Damage");
            PlayerStats playerStats = other.gameObject.GetComponentInParent<PlayerStats>();
            acidBeetle.DealAcidDamage(damage, playerStats);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy")) return;

        // DOES NOT DESTROY THE PARTICLE, JUST STOPS IT. Persists forever in the scene, removing parent necessary for animation
        if (baseProjectile)
        {
            particle.Stop();
            particle.transform.parent = null;
        }

        // Destroys the acid prefab, not the particle
        // Debug.Log("collision");
    }
}
