using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;

public class BotPart : MonoBehaviour
{
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 axis;
    [SerializeField] protected List<Block> blocks = new List<Block>();

    void Start()
    {
        blocks.AddRange(transform.GetComponentsInChildren<Block>());
    }
    public void Place(BotGrid grid)
    {
        foreach (var block in blocks)
        {
            Debug.Log("placingblock");
            block.Place(pos, axis, grid);
        }
    }

    public bool Remove(BotGrid grid)
    {
        Debug.Log("a");
        if (!CanRemove(grid)) return false;

        foreach(var block in blocks)
            block.Remove(pos,axis,grid);

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
        transform.position = pos;
    }
    public void SetAxis(Vector3 axis)
    {
        this.axis = axis;
        transform.rotation *= Quaternion.FromToRotation(Vector3.right, axis);
    }
}
