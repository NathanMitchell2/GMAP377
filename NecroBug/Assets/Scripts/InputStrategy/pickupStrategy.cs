using UnityEngine;
using UnityEngine.InputSystem;
public class pickupStrategy : InputStrategy
{
    public override void RunStrategy(InputValue value)
    {
        PickupManager clickPart = GetComponentInChildren<PickupManager>();
        if (value.isPressed && clickPart)
        {
            clickPart.pickup();
        }
    }
}
