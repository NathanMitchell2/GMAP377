using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartDeathHandler : MonoBehaviour
{
    private BotBulider builder;
    private ModularBugPart part;
    private GameObject cameraObject;
    private MenuChanger menuChanger;

    private void Awake()
    {
        builder = PlayerIdentifier.GetPlayer().GetComponentInChildren<BotBulider>();
        part = GetComponent<ModularBugPart>();
        cameraObject = GameObject.Find("/Camera");
        menuChanger = cameraObject.GetComponentInChildren<MenuChanger>();
    }
    public void Death()
    {
        if (GetComponent<carControler>() != null)
            menuChanger.SetMenu(5);

        part.GetMediator().DestroyItem();
        builder.UpdateBuilt();
        /*
        List<BotPart> initParts = builder.GetBadParts();

        //builder.RemovePart(part);
        builder.DestroyPart(part.GetMediator());
        
        List<BotPart> parts = builder.GetBadParts();
        foreach (BotPart p in initParts)
        {
            parts.Remove(p);
        }

        while (parts.Count > 0)
        {
            while (parts.Count > 0)
            {
                builder.DestroyPart(parts[0].GetMediator());
            }
            

            parts = builder.GetBadParts();
            foreach (BotPart p in initParts)
            {
                parts.Remove(p);
            }
        }
        */
        //builder.DestroyPart(GetComponent<ModularBugPart>());
    }
}
