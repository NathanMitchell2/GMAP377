using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float time = 1f;
    public float knockback = 100f;
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
            rb.AddForce(gameObject.transform.forward*knockback,ForceMode.Impulse);
        }
    }
}
