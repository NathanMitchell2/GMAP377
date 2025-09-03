using UnityEngine;

public class MultiButton : MonoBehaviour
{
    private MultiButtonController controller;

    public Material defaultMat;
    public Material pressedMat;

    private Renderer rend;
    private bool isPressed = false;

    private void Start()
    {
        controller = FindObjectOfType<MultiButtonController>();
        rend = GetComponent<Renderer>();

        if (rend != null && defaultMat != null)
        {
            rend.material = defaultMat;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed && other.gameObject.CompareTag("Player"))
        {
            isPressed = true;

            if (rend != null && pressedMat != null)
            {
                rend.material = pressedMat;
            }
            
            controller.ButtonPressed(gameObject);
            Debug.Log(gameObject.name + " pressed");
        }
    }
}
