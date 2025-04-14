using UnityEngine;

public class ProjectileDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }
    }
}
