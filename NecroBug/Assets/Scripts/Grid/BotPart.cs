using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;

public class BotPart : MonoBehaviour
{
    [SerializeField] private Vector3 pos;
    [SerializeField] protected List<Block> blocks = new List<Block>();
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
        return blocks[0].GetAxis();
    }
    public void SetPos(Vector3 pos)
    {
        this.pos = pos;
    }
    public void SetAxis(Vector3 axis)
    {
        foreach (var block in blocks)
        {
            block.SetAxis(axis);
        }
    }
}
