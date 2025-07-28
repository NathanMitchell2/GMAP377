using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using System.Collections.Generic;
using System;
using TMPro.EditorUtilities;

public class BotPartMemento
{
    Vector3 pos;
    int ori;
    public BotPartMemento(Vector3 pos, int orientation)
    {
        this.pos = pos;
        ori = orientation;
    }
    public Vector3 GetPos()
        { return pos; }
    public int GetOri()
        { return ori; }
}
public class BotPart : MonoBehaviour
{
    public static float GridPositionToLocalPosition = 1f;
    public static Transform gridTransform;
    [SerializeField] private Vector3 pos;
    private int orientation = 0;
    private List<GameObject> orientations = new List<GameObject>();

    private MediatorPart mediator;
    //protected List<Block> blocks = new List<Block>();

    public void Initialize(MediatorPart mediator)
    {
        this.mediator = mediator;
        transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
    }
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            orientations.Add(transform.GetChild(i).gameObject);
        }
        ResetOrientations();
        orientations[orientation].SetActive(true);
        //For some reason when this is Start not Awake when Place is called this code hasnt been, but it has been for remove?? lol
    }
    private void Update()
    {
        transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
    }

    public BotPartMemento CreateMemento()
    {
        return new BotPartMemento(pos, orientation);
    }
    public void RestoreMemento(BotPartMemento mem)
    {
        SetPos(mem.GetPos());
        orientation = mem.GetOri();
        ResetOrientations();
        orientations[orientation].SetActive(true);
        transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
    }

    public MediatorPart GetMediator()
        { return mediator; }
    public Transform GetPartStorage()
    {
        Transform orientation = orientations[this.orientation].transform;
        for(int i = 0; i < orientation.childCount; i++)
        {
            if (orientation.GetChild(i).name == "PartStorage")
                return orientation.GetChild(i);
        }
        return null;
    }
    
    public bool IsInCell(Vector3 pos)
    {
        foreach (var block in GetBlocks())
        {
            if (block.IsInCell(this.pos, pos))
                return true;
        }
        return false;
    }

    public bool Check(BotGrid grid)
    {
        foreach (var block in GetBlocks())
        {
            if (!block.Check(pos, grid))
                return false;
        }
        return true;
    }
    public void Place(BotGrid grid)
    {
        foreach (var block in GetBlocks())
        {
            block.inBounds(pos, grid);
        }
        foreach(var block in GetBlocks())
        {
            block.Place(pos, grid);
        }
        transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
    }

    public bool Remove(BotGrid grid)
    {
        if (!CanRemove(grid)) return false;

        foreach (var block in GetBlocks())
        {
            block.Remove(pos, grid);
        }

        return true;
    }

    public bool CanRemove(BotGrid grid)
    {
        foreach (var block in GetBlocks())
            if (!block.CanRemove(pos, grid)) return false;
        return true;
    }

    public bool Move(Vector3 pos, BotGrid grid)
    {
        foreach (var block in GetBlocks())
        {
            if (!block.inBounds(pos, grid))
                return false;
        }

        if (!Remove(grid)) return false;

        SetPos(pos);
        Place(grid);

        return true;
    }

    public int GetCount()
    {
        return GetBlocks().Count;
    }
    public Block GetIndex(int index)
    {
        return GetBlocks()[index];
    }
    public Vector3 GetPos()
    {
        return pos;
    }
    public int GetOrientation()
    {
        return orientation;
    }
    public void SetPos(Vector3 pos)
    {
        this.pos = pos;
        
    }
    public bool ProgressOrientation(BotGrid grid)
    {
        int temp = BoundOrientation(orientation + 1);

        orientations[temp].SetActive(true);

        List<Block> blocks = new List<Block>();
        orientations[temp].GetComponentsInChildren(blocks);

        foreach (var block in blocks)
        {
            if (!block.inBounds(pos, grid))
                return false;
        }

        if (!Remove(grid)) return false;

        orientation = temp;
        ResetOrientations();
        orientations[orientation].SetActive(true);
        Place(grid);

        return true;

    }
    private void ResetOrientations()
    {
        foreach (var obj in orientations)
        {
            obj.SetActive(false);
        }
    }
    private int BoundOrientation(int ori)
    {
        if (ori < 0)
            return orientations.Count-1;
        else if (ori > orientations.Count - 1)
            return 0;
        return ori;
    }

    private List<Block> GetBlocks()
    {
        List<Block> blocks = new List<Block>();
        if (orientation < orientations.Count)
        {
            orientations[orientation].GetComponentsInChildren<Block>(blocks);
            return blocks;
        }
        else
        {
            GetComponentsInChildren<Block>(blocks);
            return blocks;
        }
    }
}
