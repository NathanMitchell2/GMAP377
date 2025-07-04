using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    private static PlayerIdentifier player = null;
    public static PlayerIdentifier GetPlayer()
    {
        if(player == null)
        {
            player = (PlayerIdentifier)FindFirstObjectByType(typeof(PlayerIdentifier));
        }

        return player;
    }
}
