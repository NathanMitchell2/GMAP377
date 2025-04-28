using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class menuStrategy : InputStrategy
{
    private bool flipFlop = false;
    public override void RunStrategy(InputValue value)
    {
        flipFlop = !flipFlop;
        gameObject.SetActive(flipFlop);
        if(flipFlop)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
