using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cameras = new List<GameObject>();
    private GameObject player;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        player = ((PlayerInput)FindFirstObjectByType(typeof(PlayerInput))).gameObject;
        GetComponentInChildren<CinemachineCamera>().Target.TrackingTarget = player.GetComponentInChildren<FollowCar>().gameObject.transform;
    }
    public void setDriverSpeed(float speed)
    {
        carControler car = player.GetComponentInChildren<carControler>();
        car.setDriverSpeed(speed);
    }
    public void setSteerSpeed(float speed)
    {
        carControler car = player.GetComponentInChildren<carControler>();
        car.setSteerSpeed(speed);
    }
    public void setDashSpeed(float speed)
    {
        JetPart.setDash(speed);
    }
    
    public carControler GetCar()
    {
        carControler car = player.GetComponentInChildren<carControler>();
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
