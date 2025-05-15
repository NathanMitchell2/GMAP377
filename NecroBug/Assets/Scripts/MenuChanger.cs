using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuChanger : MonoBehaviour
{
    [SerializeField] private List<GameObject> menus = new List<GameObject>();
    [SerializeField] private List<int> cameras = new List<int>();

    private bool flipFlop = true;
    private CameraManager camManager;
    private void Awake()
    {
        // Mouse is enabled in CameraManager at start
        // EnableMouseControl();

        camManager = GetComponentInParent<CameraManager>();
    }
    public void FlipFlopMenus()
    {
        flipFlop = !flipFlop;
        if (flipFlop)
        {
            SetMenu(0);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Off();
            camManager.SetCamera(0);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void SetMenu(int index)
    {
        Off();

        menus[index].SetActive(true);
        camManager.SetCamera(cameras[index]);
    }
    public void Off()
    {
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }
    }

    // Controls for enabling and disable UI elements and mouse controls
    public void DisableUI(GameObject UIObject) { UIObject.SetActive(false); }
    public void EnableUI(GameObject UIObject) { UIObject.SetActive(true); }
    public void EnableMouseControl() 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; 
    }
    public void DisableMouseControl() 
    { 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }
}
