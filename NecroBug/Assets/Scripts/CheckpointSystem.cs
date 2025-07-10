using UnityEngine;
using System.Collections.Generic;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance {get; private set; }

    public Transform[] checkpointList;
    public GameObject carObject;

    public static StorageManagerMemento managerMemento;

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
    }

    public void SetCheck(int check){
        if (check >= 0 && check < checkpointList.Length)
        {
            currentCheck = check;
            managerMemento = PlayerIdentifier.GetPlayer().GetComponent<StorageManager>().CreateMemento();
            InstantAddItem.doCreate = false;
        }

    }

    public void PlayerToCheckpoint ()
    {
        BuiltBotStorage storage = PlayerIdentifier.GetPlayer().GetComponentInChildren<BuiltBotStorage>();
        GameObject player = PlayerIdentifier.GetPlayer().gameObject;
        if (managerMemento != null)
        {
            //BuiltBotStorage. = checkpointList[currentCheck].position;
            //BuiltBotStorage.sourceRot = checkpointList[currentCheck].rotation;

            player.GetComponent<StorageManager>().RestoreMemento(managerMemento);
        }
        //storage.GetSource().SetPositionAndRotation(checkpointList[currentCheck].position, checkpointList[currentCheck].rotation);
        BuiltBotStorage.sourcePos = checkpointList[currentCheck].position;//player.transform.InverseTransformPoint(checkpointList[currentCheck].position);
        //BuiltBotStorage.sourceRot = player.transform.rotat checkpointList[currentCheck].rotation;
        storage.AlignAll();
        Debug.LogError("Out of plyaer checkpoint");
        foreach(MediatorPart p in storage.GetItemList())
        {
            ModularBugPart obj = p.GetBugPart();
            Debug.LogError("Checkpoint obst part at " + obj.transform.localPosition + " and " + obj.transform.localRotation);
        }

        //PlayerIdentifier.GetPlayer().GetComponentInChildren<BotBulider>().CreateBot();
        
    }
}

