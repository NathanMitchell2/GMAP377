using UnityEngine;

public class wheelRotation : MonoBehaviour
{
    public WheelCollider wheelCollider; 
    public Transform wheelMesh; 
    public bool wheelTurn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
         if(wheelTurn == true) {
            wheelMesh.localEulerAngles = new Vector3(wheelMesh.localEulerAngles.x, wheelCollider.steerAngle - wheelMesh.localEulerAngles.z, wheelMesh.localEulerAngles.z);
        }
        wheelMesh.Rotate(0, wheelCollider.rpm / 60 * 360 * Time.deltaTime, 0);
    }
}
