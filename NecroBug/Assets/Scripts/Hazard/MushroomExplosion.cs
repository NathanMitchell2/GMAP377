using UnityEngine;

public class MushroomExplosion : MonoBehaviour
{
    public GameObject explosion;
    public float explosionForce = 5000f;
    public float explosionRadius = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }

            Destroy(gameObject);
        }
    }
}
