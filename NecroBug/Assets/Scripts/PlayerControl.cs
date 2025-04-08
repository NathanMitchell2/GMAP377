using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    Vector2 move = new Vector2();
    Rigidbody rb;
    float friction = 0.95f;
    float moveForce = 10f;
    public bool wingsPickedUp = false;
    private void FixedUpdate()
    {
        if (move != Vector2.zero)
        {
            rb.AddForce(new Vector3(move.x, 0, move.y) * moveForce);
        }
        else
        {
            Vector3 v = rb.linearVelocity;
            v.x *= friction;
            v.z *= friction;
            rb.linearVelocity = new Vector3(v.x, rb.linearVelocity.y, v.z);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue inputValue)
    {
        move =inputValue.Get<Vector2>();
    }
   
    void OnJump(InputValue value)
    {
        if (value.isPressed && wingsPickedUp==true)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 10f, rb.linearVelocity.y);
        }
    }

}
