using UnityEngine;
using UnityEngine.InputSystem;

public class carStrategy : InputStrategy
{
    public override void RunStrategy(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();

        
        GetComponentInChildren<carControler>().SetInputs(v.x,v.y);
    }
}
