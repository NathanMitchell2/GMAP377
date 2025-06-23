
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PreBuildBot : MonoBehaviour
{
    public List<GameObject> partsP;

    [SerializeField] private BotBulider builder;
    [SerializeField] private BuilderUI UI;

    [SerializeField] private List<GameObject> parts;
    [SerializeField] private List<Vector3> poss;
    [SerializeField] private List<int> orientations;

    public void Build()
    {
        while (builder.GetCount() > 1)
        {
            builder.RemovePart(1);
        }

        for (int i = 0; i < parts.Count; i ++)
        {
            GameObject part = parts[i];
            GameObject clone = Instantiate(part, UI.GetGridTransform());
            BotPart bpart = clone.GetComponent<BotPart>();
            bpart.GetComponent<InventoryThing>().SetItem(bpart.GetComponent<InventoryThing>().GetItem());
            bpart.SetPos(poss[i]);
            builder.AddPart(bpart);
            builder.SetSelected(bpart);
            for (int j = 0; j < orientations[i]; j++)
            {
                builder.ProgressOrientationSelected();
            }
        }
        builder.SetSelected(0);
        UI.UpdateAll();
    }

    public void CustomBuild(List<BotPart> parts)
    {
        this.parts = new List<GameObject>();
        this.poss = new List<Vector3>();
        this.orientations = new List<int>();

        foreach(BotPart part in parts)
        {
            if(part.GetInventoryPart().GetName() != "Necrobug")
            {
                this.parts.Add(GetPart(part.GetInventoryPart().GetName()));
                this.poss.Add(part.GetPos());
                this.orientations.Add(part.GetOrientation());
            }    
        }
    }

    public GameObject GetPart(string option)
    {
        switch (option)
        {
            case "Necrobug":
                return partsP[0];
            case "Horn":
                return partsP[1];
            case "Jet":
                return partsP[2];
            case "Wing":
                return partsP[3];
            case "Generic":
                return partsP[4];
            case "Legs":
                return partsP[5];
            case "Leg":
                return partsP[6];
            default:
                return null;
        }
    }
}
