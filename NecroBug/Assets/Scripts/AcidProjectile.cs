using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;

    private void Start() => Destroy(gameObject, lifeTime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) return;
        if (other.CompareTag("Player"))
        {
            // damage logic here
        }
        Destroy(gameObject);
    }
}
