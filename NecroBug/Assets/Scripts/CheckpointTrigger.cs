using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointNum;
    public CheckpointSystem checkpointSystem;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            checkpointSystem.SetCheck(checkpointNum);
            Debug.Log("this is checkpoint " + checkpointNum);
        }
    }
}
