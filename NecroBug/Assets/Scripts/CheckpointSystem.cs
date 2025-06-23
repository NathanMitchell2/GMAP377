using UnityEngine;
using System.Collections.Generic;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance {get; private set; }

    public Transform[] checkpointList;
    public GameObject playerParent;
    public GameObject carObject;
    public BotBulider botBuilder;

    public static List<InventoryItem> newInventoryItems;
    public static List<InventoryItem> parts;

    private static int currentCheck = 0;

    private void Awake(){
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayerToCheckpoint();
        foreach (InventoryItem part in parts)
        {
            if (part.GetName() == "Necrobug")
                continue;
            playerParent.GetComponent<InventoryManager>().AddItem(part);
        }
    }

    public void SetCheck(int check){
        if (check >= 0 && check < checkpointList.Length){
            currentCheck = check;
            newInventoryItems = playerParent.GetComponent<InventoryManager>().Copy();
            parts = botBuilder.IdealClone();
        }

    }

    public void PlayerToCheckpoint (){
        playerParent.GetComponentInChildren<carControler>().transform.position = checkpointList[currentCheck].position;
        playerParent.GetComponentInChildren<carControler>().transform.rotation = checkpointList[currentCheck].rotation;

        
        playerParent.GetComponent<InventoryManager>().Replace(newInventoryItems);
        /*
        PreBuildBot pre = GetComponent<PreBuildBot>();
        pre.CustomBuild(parts);
        pre.Build();
        */
    }
}

