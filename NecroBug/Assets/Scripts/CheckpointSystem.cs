using UnityEngine;
using System.Collections.Generic;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance {get; private set; }

    public Transform[] checkpointList;
    public GameObject playerParent;
    public GameObject carObject;

    public static List<InventoryItem> newInventoryItems;

    private static int currentCheck = 0;

    private void Awake(){
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start(){
        PlayerToCheckpoint();
    }

    public void SetCheck(int check){
        if (check >= 0 && check < checkpointList.Length){
            currentCheck = check;
        }

        newInventoryItems = playerParent.transform.GetComponent<InventoryManager>().Copy();
    }

    public void PlayerToCheckpoint (){
        playerParent.transform.position = checkpointList[currentCheck].position;
        playerParent.transform.rotation = checkpointList[currentCheck].rotation;

        playerParent.transform.GetComponent<InventoryManager>().Replace(newInventoryItems);
    }
}

