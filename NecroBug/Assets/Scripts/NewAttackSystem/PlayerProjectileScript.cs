using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;

    private void Start() => Destroy(gameObject, lifeTime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        else if (other.CompareTag("Enemy"))
        {
            other.transform.GetComponent<EnemyHealth>().dealDamage(damage);
        }
        Destroy(gameObject);
    }
}
