using UnityEngine;

public class OnDeathCreate : MonoBehaviour
{
    [SerializeField] private GameObject objectToCreate;
    void Death()
    {
        Instantiate(objectToCreate, transform.position, transform.rotation);
    }
}
