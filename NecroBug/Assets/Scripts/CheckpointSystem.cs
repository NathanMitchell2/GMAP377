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
        if(managerMemento != null)
        {
            BuiltBotStorage.sourcePos = checkpointList[currentCheck].position;
            BuiltBotStorage.sourceRot = checkpointList[currentCheck].rotation;

            PlayerIdentifier.GetPlayer().GetComponent<StorageManager>().RestoreMemento(managerMemento);
        }
        PlayerIdentifier.GetPlayer().GetComponentInChildren<BuiltBotStorage>().AlignAll();
    }
}

