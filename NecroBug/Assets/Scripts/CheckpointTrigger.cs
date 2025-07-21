using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointNum;
    public CheckpointSystem checkpointSystem;

    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player")){
            checkpointSystem.SetCheck(checkpointNum);
            Debug.Log("this is checkpoint " + checkpointNum);
        }
    }
}
