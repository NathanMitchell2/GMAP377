using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuChanger : MonoBehaviour
{
    [SerializeField] private List<GameObject> menus = new List<GameObject>();
    [SerializeField] private List<int> cameras = new List<int>();

    private bool flipFlop = false;
    private bool activePopup = true;
    private CameraManager camManager;
    private void Awake()
    {
        // Mouse is enabled in CameraManager at start
        // EnableMouseControl();

        camManager = GetComponentInParent<CameraManager>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void FlipFlopMenus(int menu = 0)
    {
        flipFlop = !flipFlop;
        if (flipFlop)
        {
            Time.timeScale = 0;
            camManager.SetMenu(menu);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Time.timeScale = 1;
            camManager.MenuOff();
            camManager.SetCamera(0);
            if(activePopup)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    public void SetActivePopup(bool activePopup)
    {
        this.activePopup = activePopup;
    }
    public void SetMenu(int index)
    {
        Off();

        menus[index].SetActive(true);
        //this sets the cam to the build cam when the build menu is selected
        //for now its hard coded that the build menu is at index two
        //maybe get better comparison later
        if (index == 2){
            camManager.SetCamera(1);
        }
        else{
            camManager.SetCamera(0);
        }
       
    }
    public void Off()
    {
        foreach (var menu in menus)
        {
            menu.SetActive(false);
        }
    }

    // Controls for enabling and disable UI elements and mouse controls
    public void DisableUI(GameObject UIObject) { UIObject.SetActive(false);}
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
