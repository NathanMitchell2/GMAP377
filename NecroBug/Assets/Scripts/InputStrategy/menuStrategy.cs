using UnityEngine;
using UnityEngine.InputSystem;

public class menuStrategy : InputStrategy
{
    private bool flipFlop = false;
    private MenuChanger changer;
    private CameraManager camManager;
    private void Start()
    {
        changer = GetComponent<MenuChanger>();
        camManager = GetComponentInParent<CameraManager>();
    }
    public override void RunStrategy(InputValue value)
    {
        flipFlop = !flipFlop;
        if(flipFlop)
        {
            changer.SetMenu(0);
            camManager.SetCamera(1);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            changer.Off();
            camManager.SetCamera(0);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
