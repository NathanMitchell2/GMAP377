using System.Collections.Generic;
using UnityEngine;

public class HornDamageMultiplier : MonoBehaviour
{
    [SerializeField] private float cooldown = .05f;
    private float timer = 0;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRb = collision.rigidbody;

        if (timer > cooldown)//otherRb != null)
        {
            timer = 0;
            Vector3 relativeVelocity = collision.relativeVelocity;
            float mass = 1500;//rb.mass;

            // Optional: Check if the collision was with the horn part
            float impactMagnitude = mass * relativeVelocity.magnitude;
            int damage = (int)Mathf.Round(impactMagnitude / 50);
            damage = Mathf.Max(damage, 10);
            if(damage > 10)
                Debug.Log("Damage: " + damage);
            if (damage >= 100)
            {
                GetComponent<PartDeathHandler>().Death();
            }
            else
            {
                GetComponent<DamageObject>().Damage(DamageObject.GetPlayerHealths(new List<GameObject> { gameObject }));
            }
            if (collision.transform.GetComponent<EnemyHealth>()) 
                collision.transform.GetComponent<EnemyHealth>().dealDamage(damage);
            if (collision.transform.GetComponent<WallHealth>())
                collision.transform.GetComponent<WallHealth>().dealDamage(damage);
        }
        /*
        if (collision.transform.GetComponent<WallHealth>())
        {
            Vector3 relativeVelocity = collision.relativeVelocity;
            float mass = rb.mass;

            float impactMagnitude = mass * relativeVelocity.magnitude;
            int damage = (int)Mathf.Round(impactMagnitude / 100);
            damage = Mathf.Max(damage, 10);
            Debug.Log("Damage: " + damage);
            collision.transform.GetComponent<WallHealth>().dealDamage(damage);

        }
        */

    }

}
