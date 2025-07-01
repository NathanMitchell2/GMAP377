using UnityEngine;
using UnityEngine.InputSystem;

public class UnstuckCar : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUnstuck(InputValue value)
    {

        GameObject car = GetComponentInChildren<carControler>().gameObject;
        car.transform.SetLocalPositionAndRotation(car.transform.localPosition + Vector3.up, Quaternion.identity);
    }
}
