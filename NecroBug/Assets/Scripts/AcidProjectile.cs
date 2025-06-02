using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;
    public PlayerStats playerStats;

    private void Start() => Destroy(gameObject, lifeTime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) return;
        else if (other.CompareTag("Player"))
        {
            playerStats.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
