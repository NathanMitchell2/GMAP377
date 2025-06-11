using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance {get; private set; }

    public Transform[] checkpointList;
    public GameObject playerParent;
    public GameObject carObject;

    private static int currentCheck = 0;

    private void Awake(){

        Debug.Log("awake check is " + currentCheck);
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start(){
        PlayerToCheckpoint();
        Debug.Log("current check is " + currentCheck);
    }

    public void SetCheck(int check){
        if (check >= 0 && check < checkpointList.Length){
            currentCheck = check;
        }
    }

    public void PlayerToCheckpoint (){
        carObject.transform.position = checkpointList[currentCheck].position;
        carObject.transform.rotation = checkpointList[currentCheck].rotation;
        
        playerParent.transform.position = checkpointList[currentCheck].position;
    }
}
