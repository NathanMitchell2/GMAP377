using UnityEngine;
using FMODUnity;
using System.Collections.Generic;

public class MushroomExplosion : MonoBehaviour
{
    public GameObject explosion;
    public float explosionForce = 5000f;
    public float explosionRadius = 5f;

    private DamageObject dmgObj;

    private bool beingDestroyedFlag = false;

    [SerializeField]
    private string bombSound = "event:/Other/mushroom explosion";

    private void Awake()
    {
        dmgObj = GetComponent<DamageObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            //Instantiate(explosion, transform.position, Quaternion.identity);
            SafeDestroy();
        }
    }

    public void SafeDestroy()
    {
        if (beingDestroyedFlag)
            return;
        beingDestroyedFlag = true;

        Instantiate(explosion, transform.position, Quaternion.identity);
        RuntimeManager.PlayOneShot(bombSound, Camera.main.transform.position);
        Debug.Log("bomb sound played");
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        List<GameObject> targets = new List<GameObject>();
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
                        targets.Add(hit.gameObject);
                        if (hit.GetComponent<carControler>() != null)
                            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                    else
                    {
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    }
                }
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
                hit.GetComponent<MushroomExplosion>().SafeDestroy();
            }
        }
        if(dmgObj != null)
            dmgObj.Damage(DamageObject.GetPlayerHealths(targets));

        Destroy(gameObject);
    }
    private void OnDestroy()
    {
    }
}
