using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cameras = new List<GameObject>();
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
    

    public void SetMenu(int index)
    {
        GetComponentInChildren<MenuChanger>().SetMenu(index);
    }
    public void SetCamera(int index)
    {
        Off();

        cameras[index].SetActive(true);

    }
    private void Off()
    {
        foreach (GameObject go in cameras)
        {
            go.SetActive(false);
        }
    }
}
