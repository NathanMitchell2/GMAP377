using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuChanger : MonoBehaviour
{
    [SerializeField] private List<GameObject> menus = new List<GameObject>();
    private void Awake()
    {
        // Mouse is enabled in CameraManager at start
        // EnableMouseControl();
    }
    public void SetMenu(int index)
    {
        Off();

        menus[index].SetActive(true);
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
