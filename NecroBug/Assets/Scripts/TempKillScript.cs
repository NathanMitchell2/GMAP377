using UnityEngine;

public class TempKillScript : MonoBehaviour
{
    public bool flag = false;
    public bool flag2 = false;
    public bool flag3 = false;

    // Update is called once per frame
    void Update()
    {
        if (flag)
            PlayerIdentifier.GetPlayer().BroadcastMessage("Death");
        if (flag2)
            PlayerIdentifier.GetPlayer().GetComponentInChildren<BotBulider>().CreateBot();
        if (flag3)
        {
            foreach (MediatorPart p in PlayerIdentifier.GetPlayer().GetComponent<StorageManager>().GetItemList(StorageManager.StorageKey.BuilderBuffer))
            {
                p.DestroyBugPart();
                p.CreateBugPart();
            }
        }
    }
}
