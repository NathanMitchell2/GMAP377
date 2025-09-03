using UnityEngine;
using System.Collections.Generic;

public class MushroomExplosion : MonoBehaviour
{
    public GameObject explosion;
    public float explosionForce = 5000f;
    public float explosionRadius = 5f;
    private bool exploding = false;



    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            //Instantiate(explosion, transform.position, Quaternion.identity);
            Explode();
        }
    }
    public void Explode()
    {
        if (exploding)
            return;
        exploding = true;

        Instantiate(explosion, transform.position, Quaternion.identity);
 
        List<GameObject> targets = new List<GameObject>();
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
                    if (hit.gameObject.tag == "Player")
                    {
                        if (hit.GetComponent<carControler>() != null)
                            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                    else
                    {
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                }
            }
            if (hit.gameObject.tag == "Player")
            {
                targets.Add(hit.gameObject);
            }

            if (hit.gameObject.tag == "Enemy")
            {
                EnemyHealth health = hit.GetComponent<EnemyHealth>();
                health.dealDamage(200);
            }

            if (hit.gameObject.tag == "Wall")
            {
                WallHealth health = hit.GetComponent<WallHealth>();
                health.dealDamage(200);
            }

            if (hit.GetComponent<MushroomExplosion>() != null)
            {
                hit.GetComponent<MushroomExplosion>().Explode();
            }
        }

        GetComponent<DamageObject>().Damage(DamageObject.GetPlayerHealths(targets));
        Destroy(gameObject);
    }
    
}
