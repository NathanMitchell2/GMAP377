using UnityEditor;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private static int popups = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popups++;
        ManageMenuState();
    }
    private void ManageMenuState()
    {
        if (popups>0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        popups--;
        ManageMenuState();

    }
}
