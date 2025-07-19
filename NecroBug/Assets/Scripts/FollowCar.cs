using UnityEngine;

public class FollowCar : MonoBehaviour
{
    [SerializeField] public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        carControler car = transform.parent.GetComponentInChildren<carControler>();
        transform.SetPositionAndRotation(car.transform.position+offset, Quaternion.identity);
    }
}
