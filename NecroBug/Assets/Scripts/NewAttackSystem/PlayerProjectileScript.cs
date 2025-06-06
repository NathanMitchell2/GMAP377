using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;
    public ParticleSystem particle;

    private void Start() => Destroy(gameObject, lifeTime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        else if (other.CompareTag("Enemy"))
        {
            other.transform.GetComponent<EnemyHealth>().dealDamage(damage);
        }
        // DOES NOT DESTROY THE PARTICLE, JUST STOPS IT. Persists forever in the scene, removing parent necessary for animation
        particle.Stop();
        particle.transform.parent = null;
        // Destroys the acid prefab, not the particle
        Destroy(gameObject);
    }
}
