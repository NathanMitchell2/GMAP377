using UnityEngine;
using UnityEngine.InputSystem;

public class menuStrategy : InputStrategy
{
    private bool flipFlop = false;
    public override void RunStrategy(InputValue value)
    {
        flipFlop = !flipFlop;
        gameObject.SetActive(flipFlop);
    }
}
