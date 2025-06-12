using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartDeathHandler : MonoBehaviour
{
    private BotBulider builder;
    private BotPart part;
    public void Initialize(BotPart part, BotBulider builder)
    {
        this.part = part;
        this.builder = builder;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        if (GetComponent<carControler>() != null)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        List<BotPart> initParts = builder.GetBadParts();

        //builder.RemovePart(part);
        builder.DestroyPart(part);
        
        List<BotPart> parts = builder.GetBadParts();
        foreach (BotPart p in initParts)
        {
            parts.Remove(p);
        }

        while (parts.Count > 0)
        {
            while (parts.Count > 0)
            {
                builder.DestroyPart(parts[0]);
            }
            

            parts = builder.GetBadParts();
            foreach (BotPart p in initParts)
            {
                parts.Remove(p);
            }
        }
        //builder.DestroyPart(GetComponent<ModularBugPart>());
    }
}
