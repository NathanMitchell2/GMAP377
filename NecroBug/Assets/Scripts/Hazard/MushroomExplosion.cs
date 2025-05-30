using UnityEngine;
using FMODUnity;

public class MushroomExplosion : MonoBehaviour
{
    public GameObject explosion;
    public float explosionForce = 5000f;
    public float explosionRadius = 5f;

    [SerializeField]
    private string explosionSound = "event:/Other/mushroom explosion";

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            RuntimeManager.PlayOneShot(explosionSound, Camera.main.transform.position);
            Debug.Log("bomb sound played");

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
           if (other.gameObject.tag == "Enemy"){
                EnemyHealth health = other.GetComponent<EnemyHealth>();
                health.dealDamage(200);
                
           }
            Destroy(transform.parent.gameObject);
        }
    }
}
