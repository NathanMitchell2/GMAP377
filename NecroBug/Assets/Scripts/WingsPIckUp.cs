using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class WingsPIckUp : MonoBehaviour
{
    public bool allowPickup=false;
    public GameObject pickUpText;
    bool pickedUp = false;
    public Transform target;   
    public Vector3 offset = new Vector3(0,0.3f,0);
    public PlayerControl playerControl;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            allowPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            allowPickup = false;
        }
    }

    void OnPickUp(InputValue value)
    {
        if (value.isPressed && allowPickup)
        {
            pickedUp = true;
            playerControl.wingsPickedUp = true;
            GetComponent<Collider>().enabled = false;
            allowPickup = false;
        }
    }

    void Update()
    {
        gameObject.transform.Rotate(0, 1, 0);
        pickUpText.SetActive(allowPickup);
        if (pickedUp)
        {
            transform.position = target.position + offset;
        }
    }
}
