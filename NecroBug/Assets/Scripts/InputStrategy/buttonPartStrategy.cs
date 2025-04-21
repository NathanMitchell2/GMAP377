using UnityEngine;
using UnityEngine.InputSystem;

public class buttonPartStrategy : InputStrategy
{
    public override void RunStrategy(InputValue value)
    {
        ModularBugPart clickPart = GetComponentInChildren<ModularBugPart>();
        if (value.isPressed && clickPart)
        {
            clickPart.Activate();
        }
    }
}