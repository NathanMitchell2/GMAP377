using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMove : MonoBehaviour
{
    Vector2 move = new Vector2();
    Rigidbody rb;
    
    private void Update()
    {
        rb.AddForce(move.x,0,move.y);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue inputValue)
    {
        move =inputValue.Get<Vector2>();
    }
  
 
}
