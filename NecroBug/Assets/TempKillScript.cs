using UnityEngine;

public class TempKillScript : MonoBehaviour
{
    public bool flag = false;
    public bool flag2 = false;

    // Update is called once per frame
    void Update()
    {
        if (flag)
            PlayerIdentifier.GetPlayer().BroadcastMessage("Death");
        if (flag2)
            PlayerIdentifier.GetPlayer().GetComponentInChildren<BotBulider>().CreateBot();
    }
}
