using UnityEngine;

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
            if (damage >= 100)
            {
                gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
            else
            {
                gameObject.GetComponent<PlayerHealth>().TakeDamage(damage/2);
            }
            collision.transform.GetComponent<EnemyHealth>().dealDamage(damage);
        }


    }

}
