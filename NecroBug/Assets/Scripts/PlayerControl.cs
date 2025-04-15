using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    Vector2 moveInput = new Vector2();
    Rigidbody rb;
    float friction = .5f;
    float moveForce = 200f;
    public bool wingsPickedUp = false;
    private void FixedUpdate()
    {
        Vector2 move = RotateMoveByCamera(moveInput);
        //might want to add checks for isGrounded to have air friction
        if (move != Vector2.zero)
        {
            rb.AddForce(new Vector3(move.x, 0, move.y) * moveForce);
        }
        //else
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
        moveInput = inputValue.Get<Vector2>();
    }
    Vector2 RotateMoveByCamera(Vector2 moveInputT)
    {
        // Rotation Matrix using the main camera's forward vector
        Vector3 camFor = Camera.main.transform.forward;
        //swapped x and z and made x negative, should be a 90 degree rotation (I don't know guessed and checked)
        //its necessary because "forward" is in +z not +x
        Vector2 cam = new Vector2(camFor.z, -camFor.x);
        Vector2 m = new Vector2();

        float a=cam.x;
        float b=-cam.y;
        float c=cam.y;
        float d=cam.x;
        m.x = a*moveInputT.x+b*moveInputT.y;
        m.y = c*moveInputT.x+d*moveInputT.y;
        m.Normalize();
        
        return m;
    }
   
    void OnJump(InputValue value)
    {
        if (value.isPressed && wingsPickedUp==true)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 10f, rb.linearVelocity.y);
        }
    }

    void OnClick(InputValue value)
    {
        ModularBugPart clickPart = GetComponent<ModularBugPart>();
        if (value.isPressed && clickPart)
        {
            clickPart.Activate();
        }
    }

}
