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
            popupUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void closePopup(){
        popupUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnTriggerExit(Collider other){
        firstPass = false;
    }
}