using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class BotBulider : ItemStorage
{
    private const int x = 6;
    private const int y = 6;
    private const int z = 6;

    private BotGrid grid;
    private StorageManager manager;

    private Dictionary<string, List<BotPartMemento>> savedPartPositions = new Dictionary<string, List<BotPartMemento>>();

    private void Awake()
    {
        grid = new BotGrid(x, y, z);
        manager = GetComponentInParent<StorageManager>();
    }
    private void Start()
    {
        CreateBot();
        /*
        MediatorPart part = (MediatorPart)GetItemList()[0];

        
        if(!part.HasBotPart())
        {
            part.CreateBotPart();
            BotPart bPart = part.GetBotPart();

            bPart.SetPos(new Vector3(4, 4, 4));
            bPart.Place(grid);

            CreateBot();
        }
        */
    }
    public override InventoryItem AddItem(ItemMemento item)
    {
        if (item == null || item.GetItemType() != typeof(MediatorPart))
            return null;

        MediatorPart tItem = (MediatorPart)gameObject.AddComponent(item.GetItemType());
        tItem.RestoreMemento(item);

        if (!tItem.HasBotPart())
            tItem.CreateBotPart();
        RestoreSavedPos(tItem.GetBotPart());
        CreateBot(true);
        tItem.GetBotPart().transform.SetLocalPositionAndRotation(Vector3.down * -10, Quaternion.identity);
        return tItem;
    }
    public override bool RemoveItem(InventoryItem item)
    {
        if (item == null || item.GetType() != typeof(MediatorPart))
            return false;

        MediatorPart tItem = (MediatorPart)item;
        item.DestroyItem();
        MaintainSavedParts();
        return true;
        /*
        if (tItem.HasBotPart())
        {
            BotPart part = tItem.GetBotPart();

            //BotPart part = mPart.GetBotPart();
            int index = GetPartList().IndexOf(part);

            if (part.Remove(grid)) //HARD CODED, can't remove first item in list (for car)
            {
                //GetComponent<InventoryManager>().RecoverActiveItem(mPart);
                Debug.LogError(manager.GetItemList(StorageManager.StorageKey.BuilderBuffer).ToCommaSeparatedString());
                Debug.LogError(part.GetMediator());
                if (GetItemList().Contains(part.GetMediator()))
                {
                    //manager.Transfer(part.GetMediator(), StorageManager.StorageKey.BotBuilder, StorageManager.StorageKey.Inventory);
                }
                if (part != null)
                    //part.GetMediator().DestroyBotPart();
                //partList.RemoveAt(index);
                return true;
            }
            return false;
        }
        */


    }

    public List<BotPart> GetPartList()
    {
        List<BotPart> output = new List<BotPart>();
        grid.ClearGrid();
        foreach (MediatorPart part in manager.GetItemList(StorageManager.StorageKey.BuilderBuffer))
        {
            if (part.HasBotPart())
            {
                BotPart bPart = part.GetBotPart();
                bPart.Place(grid);
                output.Add(bPart);
            }
        }
        foreach (MediatorPart part in GetItemList())
        {
            if (part.HasBotPart())
            {
                BotPart bPart = part.GetBotPart();
                bPart.Place(grid);
                output.Add(bPart);
            }
        }
        return output;
    }

    public bool Check()
    {
        return grid.Check();
    }
    public Vector3 GetSize()
    {
        return new Vector3(x, y, z);
    }
    public List<Tile> GetCell(int x, int y, int z)
    {
        return grid.GetCell(x, y, z);
    }
    public int GetPartCount()
    {
        return GetPartList().Count;
    }
    public BotPart GetPart(int i)
    {
        return GetPartList()[i];
    }
    public int IndexOf(BotPart part)
    {
        return GetPartList().IndexOf(part);
    }
    
    private bool MovePart(BotPart part, Vector3 pos)
    {
        int index = GetPartList().IndexOf(part);

        if (index != 0 && index != -1) //HARD CODED, can't remove first item in list (for car)
        {
            MaintainSavedParts();
            return part.Move(pos, grid);
        }
        return false;
    }
    public bool MovePart(int i, Vector3 pos)
    {
        return MovePart(GetPartList()[i], pos);
    }
    private bool ProgressOrientation(BotPart part)
    {
        return part.ProgressOrientation(grid);
    }
    public bool ProgressOrientation(int i)
    {
        return ProgressOrientation(GetPartList()[i]);
    }

    private bool RemovePart(BotPart part)
    {
        //BotPart part = mPart.GetBotPart();
        int index = GetPartList().IndexOf(part);

        if (index != 0 && index != -1 && part.Remove(grid)) //HARD CODED, can't remove first item in list (for car)
        {
            if (GetItemList().Contains(part.GetMediator()))
            {
                manager.Transfer(part.GetMediator(), StorageManager.StorageKey.BotBuilder, StorageManager.StorageKey.Inventory);
            }
            if(part!=null)
                part.GetMediator().DestroyBotPart();

            MaintainSavedParts();
            //partList.RemoveAt(index);
            return true;
        }
        return false;

    }
    public bool RemovePart(int index)
    {
        return RemovePart(GetPartList()[index]);
    }
    public void CreateBot()
    {
        CreateBot(false);
    }
    public void CreateBot(bool hideTransferedPart)
    {
        List<BotPart> snapshot = GetPartList();
        if (!grid.Check())
            return;

        foreach (BotPart part in snapshot)
        {
            if (GetItemList().Contains(part.GetMediator()))
            {
                InventoryItem temp = manager.Transfer(part.GetMediator(), StorageManager.StorageKey.BotBuilder, StorageManager.StorageKey.BuilderBuffer);
                if(hideTransferedPart)
                    ((MediatorPart) temp).GetBotPart().transform.SetLocalPositionAndRotation(Vector3.down * -10, Quaternion.identity);
            }
        }

        foreach (MediatorPart part in manager.GetItemList(StorageManager.StorageKey.BuilderBuffer))
        {
            if (!part.HasBotPart())
            {
                manager.Transfer(part, StorageManager.StorageKey.BuilderBuffer, StorageManager.StorageKey.Inventory);
                //part.DestroyBugPart();
            }
        }
        ((BuiltBotStorage)manager.GetStorage(StorageManager.StorageKey.BuilderBuffer)).AlignAll();
        //transform.parent.GetComponentInChildren<BuiltBotStorage>().AlignAll();
    }

    public int IndexOf(MediatorPart part)
    {
        return IndexOf(part);
    }

    public void UpdateBuilt()
    {
        BotGrid tempBuiltGrid = new BotGrid(x, y, z);

        // Add -> Build -> Remove -> Any Part Destroyed. Results in removed part removed, not catastrophic
        foreach (MediatorPart part in manager.GetItemList(StorageManager.StorageKey.BuilderBuffer))
        {
            part.GetBotPart().Place(tempBuiltGrid);
        }

        while(!tempBuiltGrid.Check())
        {
            foreach(MediatorPart part in manager.GetItemList(StorageManager.StorageKey.BuilderBuffer))
            {
                if(!part.GetBotPart().Check(tempBuiltGrid))
                {
                    manager.Transfer(part, StorageManager.StorageKey.BuilderBuffer, StorageManager.StorageKey.Inventory);
                }
            }
        }
    }

    private void RestoreSavedPos(BotPart addedPart)
    {
        string key = addedPart.GetMediator().GetName();
        
        if (!savedPartPositions.ContainsKey(key))
            return;

        BotGrid tempBuiltGrid = new BotGrid(x, y, z);
        List<BotPart> parts = GetPartList();

        foreach (BotPart part in parts)
        {
            part.Place(tempBuiltGrid);
        }

        int index = 0;
        List<BotPartMemento> savedPoses = savedPartPositions[key];

        addedPart.RestoreMemento(savedPoses[index]);
        addedPart.Place(tempBuiltGrid);
        index++;

        while (!tempBuiltGrid.Check() && index < savedPoses.Count)
        {
            addedPart.Remove(tempBuiltGrid);

            addedPart.RestoreMemento(savedPoses[index]);
            addedPart.Place(tempBuiltGrid);
            index++;
        }
    }
    private void MaintainSavedParts()
    {
        List<BotPart> parts = GetPartList();
        Dictionary<string, int> partTypeToCurrentIndexPairs = new Dictionary<string, int>();

        foreach (BotPart part in parts)
        {
            string key = part.GetMediator().GetName();

            if (savedPartPositions.ContainsKey(key))
            {
                if (!partTypeToCurrentIndexPairs.ContainsKey(key))
                    partTypeToCurrentIndexPairs.Add(key, 0);

                int curIndex = partTypeToCurrentIndexPairs[key];
                List<BotPartMemento> savedPoses = savedPartPositions[key];

                if (curIndex < savedPartPositions[key].Count)
                {
                    savedPartPositions[key][curIndex] = part.CreateMemento();
                }
                else
                {
                    savedPartPositions[key].Add(part.CreateMemento());
                }
            }
            else
            {
                List<BotPartMemento> newPartPosList = new List<BotPartMemento>();
                newPartPosList.Add(part.CreateMemento());

                savedPartPositions.Add(key, newPartPosList);
                partTypeToCurrentIndexPairs.Add(key, 1);
            }
        }
    }
}
