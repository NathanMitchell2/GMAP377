using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using System.Collections.Generic;
using System;

public class BotPart : MonoBehaviour
{
    public static float GridPositionToLocalPosition = 1f;
    [SerializeField] private GameObject bugPart;
    [SerializeField] private InventoryThing invItem;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 axis; //depreciated
    [SerializeField] private Vector3 center; //depreciated
    private int orientation = 0;
    [SerializeField] private List<GameObject> orientations = new List<GameObject>();
    public bool customBinds = true;
    public string defaultBind = "";
    //protected List<Block> blocks = new List<Block>();

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
    public GameObject BuildPart(Transform transform)
    {
        //Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.FromToRotation(Vector3.right, axis));
        //Vector3 pos = rotation.MultiplyVector(this.pos) * GridPositionToLocalPosition+transform.position;
        //Quaternion rot = transform.rotation*Quaternion.FromToRotation(Vector3.right, axis)*bugPart.transform.rotation;

        Transform storage = GetPartStorage();
        Vector3 pos = this.pos * GridPositionToLocalPosition + transform.position + storage.localPosition;
        Quaternion rot = storage.localRotation * transform.rotation;

        GameObject part = Instantiate(bugPart, pos, rot, transform);

        return part;

        return bugPart;
    }
    public GameObject BuildPart()
    {
        //Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.FromToRotation(Vector3.right, axis));
        //Vector3 pos = rotation.MultiplyVector(this.pos) * GridPositionToLocalPosition+transform.position;
        //Quaternion rot = transform.rotation*Quaternion.FromToRotation(Vector3.right, axis)*bugPart.transform.rotation;

        Transform storage = GetPartStorage();
        Vector3 pos = this.pos * GridPositionToLocalPosition + storage.localPosition;
        Quaternion rot = storage.localRotation;

        GameObject part = Instantiate(bugPart, pos, rot);

        return part;

        return bugPart;
    }

    private Transform GetPartStorage()
    {
        Transform orientation = orientations[this.orientation].transform;
        for(int i = 0; i < orientation.childCount; i++)
        {
            if (orientation.GetChild(i).name == "PartStorage")
                return orientation.GetChild(i);
        }
        return null;
    }
    public void Place(BotGrid grid)
    {
        foreach (var block in GetBlocks())
        {
            block.inBounds(pos, axis, center, grid);
        }
        foreach(var block in GetBlocks())
        {
            block.Place(pos, axis, center, grid);
        }
        transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
    }

    public bool Remove(BotGrid grid)
    {
        if (!CanRemove(grid)) return false;

        foreach (var block in GetBlocks())
        {
            block.Remove(pos, axis, center, grid);
        }

        return true;
    }

    public bool CanRemove(BotGrid grid)
    {
        foreach (var block in GetBlocks())
            if (!block.CanRemove(pos, axis, center, grid)) return false;
        return true;
    }

    public bool Move(Vector3 pos, BotGrid grid)
    {
        foreach (var block in GetBlocks())
        {
            if (!block.inBounds(pos, axis, center, grid))
                return false;
        }

        if (!Remove(grid)) return false;

        SetPos(pos);
        Place(grid);

        return true;
    }

    public bool Rotate(Vector3 axis, BotGrid grid)
    {
        foreach (var block in GetBlocks())
        {
            if (!block.inBounds(pos, axis, center, grid))
                return false;
        }

        if (!Remove(grid)) return false;


        SetAxis(axis);
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
        //this.axis = axis;
        
        //transform.rotation *= Quaternion.FromToRotation(Vector3.right, axis);
    }
    public bool ProgressOrientation(BotGrid grid)
    {
        int temp = BoundOrientation(orientation + 1);

        orientations[temp].SetActive(true);

        List<Block> blocks = new List<Block>();
        orientations[temp].GetComponentsInChildren(blocks);

        foreach (var block in blocks)
        {
            if (!block.inBounds(pos, axis, center, grid))
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
    public InventoryThing GetInventoryPart()
    {
        return invItem;
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
