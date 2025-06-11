using UnityEngine;

public class TutorialCheckpoint : MonoBehaviour
{
    public TutorialPanelManager panelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panelManager.ShowPanelForCheckpoint(transform);
        }
    }
}