using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    private carControler car;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        car = (carControler)FindFirstObjectByType(typeof(carControler));
        GetComponentInChildren<CinemachineCamera>().Target.TrackingTarget = car.transform;
    }
    public void setDriverSpeed(float speed)
    {
        car.setDriverSpeed(speed);
    }
    public void setSteerSpeed(float speed)
    {
        car.setSteerSpeed(speed);
    }
    public void setDashSpeed(float speed)
    {
        JetPart.setDash(speed);
    }
    public carControler GetCar()
    {
        return car;
    }
}
