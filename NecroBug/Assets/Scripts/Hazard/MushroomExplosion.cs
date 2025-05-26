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
            //Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(transform.gameObject);
        }
    }
    private void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Player")
                {
                    //rb.isKinematic = false;
                    //rb.AddForce((transform.position - hit.transform.position) * explosionForce);
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }

            if (hit.gameObject.tag == "Enemy")
            {
                EnemyHealth health = hit.GetComponent<EnemyHealth>();
                health.dealDamage(200);
            }

            if(hit.gameObject.tag == "Wall")
            {
                WallHealth health = hit.GetComponent<WallHealth>();
                health.dealDamage(200);
            }

            if (hit.GetComponent<MushroomExplosion>() != null)
            {
                Destroy(hit.gameObject);
            }
        }
    }
}
