using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float time = 1f;
    public float knockback = 100f;
    public int dmg = 25;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
          time -= Time.deltaTime;
          if(time < 0)
          {
              Destroy(gameObject);
          }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            Rigidbody rb = other.transform.GetComponent<Rigidbody>();
            rb.AddForce(gameObject.transform.forward * knockback, ForceMode.Impulse);
        }
        if (other.tag == "Enemy")
        {
            Rigidbody rb = other.transform.GetComponent<Rigidbody>();
            rb.AddForce(gameObject.transform.forward * knockback, ForceMode.VelocityChange);
            other.transform.GetComponent<EnemyHealth>().dealDamage(dmg);
        }
        if (other.tag == "Wall")
        {
            Rigidbody rb = other.transform.GetComponent<Rigidbody>();
            other.transform.GetComponent<WallHealth>().dealDamage(dmg);
        }
    }
}
