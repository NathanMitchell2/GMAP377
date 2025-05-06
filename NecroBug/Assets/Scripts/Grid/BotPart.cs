using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;

public class BotPart : MonoBehaviour
{
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 axis;
    protected List<Block> blocks = new List<Block>();

    void Awake()
    {
        blocks.AddRange(transform.GetComponentsInChildren<Block>());
        //For some reason when this is Start not Awake when Place is called this code hasnt been, but it has been for remove?? lol
    }
    public void Place(BotGrid grid)
    {
        foreach (var block in blocks)
        {
            block.Place(pos, axis, grid);
        }
    }

    public bool Remove(BotGrid grid)
    {
        if (!CanRemove(grid)) return false;

        foreach (var block in blocks)
        {
            block.Remove(pos, axis, grid);
        }

        return true;
    }

    public bool CanRemove(BotGrid grid)
    {
        foreach (var block in blocks)
            if (!block.CanRemove(pos, axis, grid)) return false;
        return true;
    }

    public bool Move(Vector3 pos, BotGrid grid)
    {
        if (!Remove(grid)) return false;

        SetPos(pos);
        Place(grid);

        return true;
    }

    public bool Rotate(Vector3 axis, BotGrid grid)
    {
        if (!Remove(grid)) return false;

        SetAxis(axis);
        Place(grid);

        return true;
    }


    public int GetCount()
    {
        return blocks.Count;
    }
    public Block GetIndex(int index)
    {
        return blocks[index];
    }
    public Vector3 GetPos()
    {
        return pos;
    }
    public Vector3 GetAxis()
    {
        return axis;
    }
    public void SetPos(Vector3 pos)
    {
        this.pos = pos;
        //transform.position = pos;
    }
    public void SetAxis(Vector3 axis)
    {
        this.axis = axis;
        //transform.rotation *= Quaternion.FromToRotation(Vector3.right, axis);
    }
}
