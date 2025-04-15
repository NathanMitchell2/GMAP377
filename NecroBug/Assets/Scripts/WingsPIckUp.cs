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
    public Vector3 rotOffset = new Vector3();
    public PlayerControl playerControl;
    public carControler car;
    
    public Vector3 rotate = new Vector3(0,1,0);
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
            car.wingsPickedUp = true;
            playerControl.wingsPickedUp = true;
            GetComponent<Collider>().enabled = false;
            allowPickup = false;
            gameObject.transform.parent = target;
            gameObject.transform.position = target.position + offset;
            gameObject.transform.SetLocalPositionAndRotation(offset, Quaternion.Euler(rotOffset));
            rotate = new Vector3(0,0,0);
        }
    }

    void Update()
    {
        gameObject.transform.Rotate(rotate);
        pickUpText.SetActive(allowPickup);
        if (pickedUp)
        {
            //transform.position = target.position + offset;
            //transform.rotation = target.rotation;
        }
    }
}
