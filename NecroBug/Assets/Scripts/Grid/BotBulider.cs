using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BotBulider : MonoBehaviour
{
    private BotGrid grid = new BotGrid(12, 12, 12);
    private List<BotPart> parts = new List<BotPart>();

    public int GetCount()
    {
        return parts.Count;
    }
    public BotPart GetIndex(int index)
    {
        return parts[index];
    }
    public bool AddPart(BotPart part)
    {
        if (part == null)
            return false;
        if (!CheckPart(part)) 
            return false;

        for (int i = 0; i < part.GetCount(); i++)
        {
            Block block = part.GetIndex(i);
            grid.SetBlock(block);
        }
        parts.Add(part);
        return true;

    }
    public bool MovePart(BotPart part, Vector3 pos)
    {
        Vector3 ogPos = part.GetPos();
        RemovePart(part);
        part.SetPos(pos);
        if(!AddPart(part))
        {
            part.SetPos(ogPos);
            AddPart(part);
            return false;
        }
        return true;
    }
    public bool MovePart(BotPart part, int x, int y, int z)
    {
        Vector3 ogPos = part.GetPos();
        RemovePart(part);
        part.SetPos(new Vector3(x,y,z));
        if (!AddPart(part))
        {
            part.SetPos(ogPos);
            AddPart(part);
            return false;
        }
        return true;
    }

    public bool RotatePart(BotPart part, Vector3 axis)
    {
        Vector3 ogAxis = part.GetAxis();
        RemovePart(part);
        part.SetAxis(axis);
        if (!AddPart(part))
        {
            part.SetAxis(ogAxis);
            AddPart(part);
            return false;
        }
        return true;
    }
    public bool RotatePart(BotPart part, int x, int y, int z)
    {
        Vector3 ogAxis = part.GetAxis();
        RemovePart(part);
        part.SetAxis(new Vector3(x, y, z));
        if (!AddPart(part))
        {
            part.SetAxis(ogAxis);
            AddPart(part);
            return false;
        }
        return true;
    }

    public bool RemovePart(BotPart part)
    {
        if (part == null) return false;
        parts.Remove(part);
        RebuildGrid();
        return true;
    }

    public bool RemovePart(int index)
    {
        if (index == null) return false;
        parts.RemoveAt(index);
        RebuildGrid();
        return true;
    }
    private bool CheckPart(BotPart part)
    {
        for (int i = 0; i < part.GetCount(); i++) {
            Block block = part.GetIndex(i);
            if(!grid.CheckBlock(block)) return false;
        }
        return true;
    }

    private void RebuildGrid()
    {
        grid.ClearGrid();
        foreach (BotPart part in parts)
        {
            for (int i = 0; i < part.GetCount(); i++)
            {
                Block block = part.GetIndex(i);
                grid.SetBlock(block);
            }
        }
    }
}
