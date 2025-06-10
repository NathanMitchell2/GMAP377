using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public Transform[] checkpointList;
    public GameObject playerParent;
    public GameObject carObject;

    private int currentCheck = 0;

    public void SetCheck(int check){
        if (check >= 0 && check < checkpointList.Length){
            currentCheck = check;
        }
    }

    public void PlayerToCheckpoint (){
        //playerParent= getComponent
        carObject.transform.position = checkpointList[currentCheck].position;
        // make default rotation or sum
    }
}
