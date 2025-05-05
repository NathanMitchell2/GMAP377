using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class menuStrategy : InputStrategy
{
    private bool flipFlop = false;
    private MenuChanger changer;
    private void Start()
    {
        changer = GetComponent<MenuChanger>();
    }
    public override void RunStrategy(InputValue value)
    {
        flipFlop = !flipFlop;
        if(flipFlop)
        {
            changer.SetMenu(0);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            changer.Off();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
