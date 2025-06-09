using NUnit.Framework.Interfaces;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class HornDamageMultiplier : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    public void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRb = collision.rigidbody;

        if (otherRb != null)
        {
            Vector3 relativeVelocity = collision.relativeVelocity;
            float mass = rb.mass;

            // Optional: Check if the collision was with the horn part
            float impactMagnitude = mass * relativeVelocity.magnitude;
            int damage = (int)Mathf.Round(impactMagnitude / 100);
            Debug.Log("Damage: " + damage);
          /* if (impactMagnitude > 10000)  
               Destroy(gameObject);*/

           

          collision.transform.GetComponent<EnemyHealth>().dealDamage(damage);
        }


    }

}
