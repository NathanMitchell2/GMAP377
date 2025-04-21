using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputStrategy : MonoBehaviour
{
    public int position = 0;
    public abstract void RunStrategy(InputValue value);
}
