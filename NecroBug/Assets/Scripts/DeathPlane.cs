using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public CheckpointSystem checkpointSystem;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            checkpointSystem.PlayerToCheckpoint();
            Debug.Log("teleported player");
        }
    }
}
