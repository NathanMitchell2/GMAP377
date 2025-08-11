using UnityEngine;
using UnityEngine.InputSystem;

public class GoToBuilderMenu : MonoBehaviour
{
    public GameObject referenceObject; 
    private MenuChanger changer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        changer = referenceObject.GetComponent<MenuChanger>();
    }

    void OnGoToBuilderMenu(){
        Debug.Log("Go to Builder Menu called");
        changer.FlipFlopMenus(2);
        //changer.SetMenu(2);

    }
}
