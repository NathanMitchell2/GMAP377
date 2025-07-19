using UnityEngine;

public class TutorialPopupTrigger : MonoBehaviour
{
    public GameObject popupUI; 
    private bool firstPass = true;

    void Start()
    {
        if (popupUI != null)
            popupUI.SetActive(false); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && popupUI != null && firstPass)
        {
            firstPass = false;
            popupUI.SetActive(true);
            Time.timeScale = 0f;
        }

        EnableMouseControl();
    }

    public void closePopup(){
        popupUI.SetActive(false);
        DisableMouseControl();
        Time.timeScale = 1f;
    }

    void OnTriggerExit(Collider other){
        firstPass = false;
    }

    public void EnableMouseControl() 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined; 
    }
    public void DisableMouseControl() 
    { 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }

}