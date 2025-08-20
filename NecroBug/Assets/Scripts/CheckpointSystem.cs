using UnityEngine;
using System.Collections.Generic;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance {get; private set; }

    public Transform[] checkpointList;

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

    public static void ResetStatics(bool fullReset)
    {
        if (fullReset)
            managerMemento = null;
        currentCheck = 0;
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

        BuiltBotStorage.sourcePos = checkpointList[currentCheck].position;
        BuiltBotStorage.sourceRot = checkpointList[currentCheck].rotation;


        if (managerMemento != null)
        {
            //BuiltBotStorage. = checkpointList[currentCheck].position;
            //BuiltBotStorage.sourceRot = checkpointList[currentCheck].rotation;

            /*
            foreach (var storages in managerMemento.GetStorages())
            {
                Debug.LogError("Storage Recorder " + storages.Key);
                foreach(ItemMemento item in storages.Value.GetItems())
                {
                    Debug.LogError("Item Recorder " + item.GetName());
                }
            }
            */
            player.GetComponent<StorageManager>().RestoreMemento(managerMemento);
        }
        //storage.GetSource().SetPositionAndRotation(checkpointList[currentCheck].position, checkpointList[currentCheck].rotation);
        //player.transform.InverseTransformPoint(checkpointList[currentCheck].position);
        //BuiltBotStorage.sourceRot = player.transform.rotat checkpointList[currentCheck].rotation;
        storage.AlignAll();

        /*
        Debug.LogError("Checkpoint list start");
        foreach(InventoryItem item in storage.GetItemList())
        {
            Debug.LogError("Checkpoint List " + item.GetName());
        }
        Debug.LogError("Checkpoint list end");
        Debug.Log(InstantAddItem.doCreate);
        */
        
    }
}

