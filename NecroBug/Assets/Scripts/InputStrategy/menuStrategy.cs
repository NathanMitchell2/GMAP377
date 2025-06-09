using UnityEngine;
using UnityEngine.InputSystem;

public class menuStrategy : InputStrategy
{
    private MenuChanger changer;
    private void Awake()
    {
        changer = GetComponent<MenuChanger>();
    }
    public override void RunStrategy(InputValue value)
    {
        changer.FlipFlopMenus();
    }
}
