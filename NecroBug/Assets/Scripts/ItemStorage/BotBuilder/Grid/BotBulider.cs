using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class BotBulider : ItemStorage
{
    private const int x = 12;
    private const int y = 12;
    private const int z = 12;

    private BotGrid grid;
    private StorageManager manager;
    //private List<BotPart> partList = new List<BotPart>();
    //private List<MediatorPart> gridParts = new List<MediatorPart>();
    //private List<MediatorPart> builtParts = new List<MediatorPart>();

    /*
    private GridDisplayCell[,,] gridDisplayCells;
    [SerializeField] GameObject cell;
    [SerializeField] Tile empty;
    [SerializeField] Transform gridTransform;

    [SerializeField] private Transform buildTransform;
    [SerializeField] private UIUpdate uiObject;
    [SerializeField] private ActionManager actionManager;
    
    public void UpdateDisplayCells()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int k = 0; k < z; k++)
                {
                    gridDisplayCells[i, j, k].ProcessStack(grid.GetCell(i,j,k));
                }
            }
        }
    }
    */

    private void Awake()
    {
        grid = new BotGrid(x, y, z);
        manager = GetComponentInParent<StorageManager>();
        //GameObject bot = Instantiate(this.bot, transform);
        //this.selectedPart = bot.GetComponent<BotPart>();
        //parts.Add(selectedPart);

        /*
        gridDisplayCells = new GridDisplayCell[x, y, z];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int k = 0; k < z; k++)
                {
                    GameObject newCell = Instantiate(cell, gridTransform);
                    newCell.transform.SetLocalPositionAndRotation(new Vector3(i, j, k), new Quaternion());
                    //Debug.Log(newCell==null);
                    gridDisplayCells[i, j, k] = newCell.GetComponent<GridDisplayCell>();
                }
            }
        }
        */



        //Destroy(playerObject.transform.GetChild(0).gameObject);
    }
    private void Start()
    {
        MediatorPart part = (MediatorPart)GetItemList()[0];

        part.CreateBotPart();
        BotPart bPart = part.GetBotPart();

        bPart.SetPos(new Vector3(4, 4, 4));
        bPart.Place(grid);

        CreateBot();
    }
    public override InventoryItem AddItem(ItemMemento item)
    {
        if (item == null)
            return null;
        InventoryItem tItem = (InventoryItem)gameObject.AddComponent(item.GetItemType());
        tItem.RestoreMemento(item);
        return tItem;
    }
    public List<BotPart> GetPartList()
    {
        List<BotPart> output = new List<BotPart>();
        grid.ClearGrid();
        foreach (MediatorPart part in GetItemList())
        {
            if (part.HasBotPart())
            {
                BotPart bPart = part.GetBotPart();
                bPart.Place(grid);
                output.Add(bPart);
            }
        }
        foreach (MediatorPart part in manager.GetItemList(StorageManager.StorageKey.BuilderBuffer))
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
    public List<ItemMemento> CreateGridMementos()
    {
        List<ItemMemento> temp = new List<ItemMemento>();

        foreach (BotPart part in GetPartList())
        {
            temp.Add(part.GetMediator().CreateMemento());
        }

        return temp;
    }

    public List<ItemMemento> CreateBuiltMementos()
    {
        List<ItemMemento> temp = new List<ItemMemento>();

        foreach(MediatorPart part in GetItemList())
        {
            temp.Add(part.CreateMemento());
        }

        return temp;
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

    //Parts lists
    /*
    public List<MediatorPart> GetGridParts()
    {
        return GetItemList();
    }

    public List<MediatorPart> GetBuiltParts()
    {
        return builtParts;
    }
    */
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
    
    public void AddPart(BotPart part)
    {
        part.Place(grid);
    }
    public MediatorPart AddPart(MediatorPart part)
    {
        //BotPart part = mPart.GetBotPart();
        MediatorPart nPart = (MediatorPart)manager.Transfer(part, StorageManager.StorageKey.Inventory, StorageManager.StorageKey.BuilderBuffer);
        if (!nPart.HasBotPart())
            nPart.CreateBotPart();
        BotPart bPart = nPart.GetBotPart();

        bPart.Place(grid);
        return nPart;
        //partList.Add(part);
        //GetItemList().Add(mPart);
    }
    
    private bool MovePart(BotPart part, Vector3 pos)
    {
        return part.Move(pos, grid);
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
            //GetComponent<InventoryManager>().RecoverActiveItem(mPart);
            Debug.LogError(manager.GetItemList(StorageManager.StorageKey.BuilderBuffer).ToCommaSeparatedString());
            Debug.LogError(part.GetMediator());
            if (manager.GetItemList(StorageManager.StorageKey.BuilderBuffer).Contains(part.GetMediator()))
            {
                part.GetMediator().DestroyBotPart();
                part.GetMediator().DestroyBugPart();
                manager.Transfer(part.GetMediator(), StorageManager.StorageKey.BuilderBuffer, StorageManager.StorageKey.Inventory);
            }
            if(part!=null)
                part.GetMediator().DestroyBotPart();
            //partList.RemoveAt(index);
            return true;
        }
        return false;

    }
    public bool RemovePart(int index)
    {
        return RemovePart(GetPartList()[index]);
    }
    /*
    public bool DestroyPart(int index)
    {
        if(index == 0)
            return false;

        gridParts[index].DestroyPart();
        return true;
        /*
        if (index != 0 && index != -1 && gridParts[index].Remove(grid)) //HARD CODED, can't remove first item in list (for car)
        {
            BotPart part2 = gridParts[index];
            actionManager.RemoveAction(index);
            gridParts.RemoveAt(index);
            builtParts[index].GetComponent<ModularBugPart>().CleanUp();
            Destroy(builtParts[index].gameObject);
            builtParts.RemoveAt(index);
            Destroy(part2.gameObject);
            return true;
        }
        return false;
        //og *
    }
    */
    /*
    public bool DestroyPart(MediatorPart part)
    {
        return DestroyPart(gridParts.IndexOf(part));
    }
    */
    /*
    public void RemoveBuiltPart(int index)
    {
        builtParts.RemoveAt(index);
    }

    public int BuiltIndexOf(MediatorPart mediatorPart)
    {
        return builtParts.IndexOf(mediatorPart);
    }
    */
    /*
    public List<InventoryItem> IdealClone()
    {
        List<InventoryItem> temp = new List<InventoryItem>();

        foreach (BotPart part in parts)
        {
            InventoryItem item = part.GetInventoryPart().GetItem();
            int count = item.GetCount();
            InventoryItem tempI = item.Copy();
            tempI.SetCount(count);
            tempI.SetHealth(tempI.GetMaxHealth());

            temp.Add(tempI);
        }
        return temp;
    }
    */

    //Should be removeable with refactor
    private void DestroyBot()
    {
        /*
        for (int i = 0; i < builtParts.Count; i++)
        {
            Debug.Log("Destroy " + i);
            var part = builtParts[i];

            //Unbind action?
            ModularBugPart bgPart = part.GetComponent<ModularBugPart>();
            if (bgPart != null) ;
                //bgPart.CleanUp();
            Destroy(part.gameObject);
        }
        
        for (int i = 0; i < builtParts.Count; i++)
        {
            Debug.Log("Destroy " + i);
            var part = builtParts[i];

            //Unbind action?
            ModularBugPart bgPart = part.GetComponent<ModularBugPart>();
            if (bgPart != null)
                bgPart.CleanUp();
            Destroy(part.gameObject);
        }
        */
    }
    public void CreateBot()
    {
        if (!grid.Check())
            return;

        GameObject player = PlayerIdentifier.GetPlayer().gameObject;

        Vector3 ogPos = Vector3.zero;

        for (int i = 0; i < player.transform.childCount; i++)
        {
            if (player.transform.GetChild(i).name == "Center")
                ogPos = player.transform.GetChild(i).transform.localPosition - player.transform.GetChild(i).GetComponent<FollowCar>().offset;
        }

        List<BotPart> snapshot = GetPartList();
        
        foreach (BotPart part in snapshot)
        {
            if (!GetItemList().Contains(part.GetMediator()))
            {
                Debug.LogError("Buffer to Builder Transfer");
                manager.Transfer(part.GetMediator(), StorageManager.StorageKey.BuilderBuffer, StorageManager.StorageKey.BotBuilder);
                Debug.LogError("Post Transfer");
            }
        }

        
        
        foreach (MediatorPart part in GetItemList())
        {
            part.DestroyBugPart();
            if (!GetPartList().Contains(part.GetBotPart()))
            {
                Debug.LogError("Builder to Inventory Transfer");
                part.DestroyBotPart();
                part.DestroyBugPart();
                manager.Transfer(part, StorageManager.StorageKey.BotBuilder, StorageManager.StorageKey.Inventory);
                //part.DestroyBugPart();
            }
        }

        //builtParts = new List<MediatorPart>();

        GameObject car = gameObject;

        for (int i = 0; i < GetItemList().Count; i++)
        {
            MediatorPart part = ((MediatorPart)GetItemList()[i]);
            part.CreateBugPart();
            if (i == 0)
            {
                car = part.GetBugPart().gameObject;
            }
            part.GetBugPart().transform.SetParent(car.transform);
        }

        car.transform.SetParent(player.transform);
        car.transform.SetLocalPositionAndRotation(ogPos, Quaternion.identity);
        car.transform.localScale = Vector3.one;
        

        /*
        Vector3 ogPos;
        if (builtParts.Count == 0)
            ogPos = playerObject.transform.GetChild(0).localPosition;
        else
            ogPos = builtParts[0].transform.localPosition;
        DestroyBot();
        PlayerStats car = null;
        builtParts = new List<GameObject>();

        for(int i = 0; i < gridParts.Count; i++)
        {
            BotPart part = gridParts[i];
            GameObject builtPart = part.BuildPart();//buildTransform.GetComponentInChildren<FollowCar>().gameObject.transform);
            //builtPart.transform.SetParent(buildTransform.transform);

            if (i==0)
            {
                car = builtPart.GetComponent<PlayerStats>();
            }
            if (builtPart.GetComponent<PlayerStats>() != null)
            {
                PlayerStats statsReference = builtPart.GetComponent<PlayerStats>();
                PlayerStats playerReference = playerObject.GetComponentInChildren<PlayerStats>();
                //car = statsReference;
                // Debug.Log(playerReference.health);
                // Debug.Log(car.health);
                statsReference.SetItem(part.GetComponent<InventoryItem>().GetItem());
                statsReference.health = (int) part.GetComponent<InventoryItem>().GetHealth(); // playerReference.health;
            }
            PlayerHealth partHealth = builtPart.GetComponent<PlayerHealth>();
            if (partHealth != null)
            {
                partHealth.Initialize(part.GetComponent<InventoryItem>().GetItem());
            }
            PartDeathHandler handler = builtPart.GetComponent<PartDeathHandler>();
            if (handler != null)
            {
                handler.Initialize(part,this);
            }

            builtParts.Add(builtPart);
        }


        for (int i = 0; i < builtParts.Count; i++)
        {
            var part = builtParts[i];

            //Debug.Log(buildTransform.GetComponent<InputManager>().name);
            //Debug.Log(part.GetComponentInChildren<InputStrategy>().name);

            //buildTransform.GetComponent<InputManager>().SetStrat(part.GetComponentInChildren<InputStrategy>());
            
            InputStrategy strat = part.GetComponent<InputStrategy>();

            part.GetComponent<Rigidbody>().centerOfMass = car.GetComponent<Rigidbody>().centerOfMass;

            if(strat != null)
            {
                buildTransform.GetComponent<InputManager>().SetStrat(strat);
            }

            if (car != null)
            {
                if (part != car)
                {
                    part.transform.SetParent(car.transform);
                }
            }
            else
            {
                Debug.Log("No NecroBug Part");
            }
        }
        actionManager.BindParts(builtParts);


        car.transform.SetParent(buildTransform);
        car.transform.SetLocalPositionAndRotation(ogPos, Quaternion.identity);
        //buildTransform.GetComponentInChildren<FollowCar>().gameObject.transform.position
        car.transform.localScale = Vector3.one;

        uiObject.StatInitialize();
        */
    }

    public int IndexOf(MediatorPart part)
    {
        return IndexOf(part);
    }
    /*
    public List<BotPart> GetBadParts()
    {
        //probs bugs here?
        Debug.LogError("will cause issues when chain destroying while builtparts != gridparts, needs memento or botpart storage");
        List<BotPart> temp = new List<BotPart>();
        foreach (MediatorPart part in gridParts)
        {
            BotPart p = part.GetBotPart();
            if (!p.Check(grid))
                temp.Add(p);
        }
        return temp;
    }
    */

    public void UpdateBuilt()
    {
        BotGrid tempBuiltGrid = new BotGrid(x, y, z);

        // Add -> Build -> Remove -> Any Part Destroyed. Results in removed part removed, not catastrophic
        foreach (MediatorPart part in GetItemList())
        {
            part.GetBotPart().Place(tempBuiltGrid);
        }

        while(!tempBuiltGrid.Check())
        {
            foreach(MediatorPart part in GetItemList())
            {
                if(!part.GetBotPart().Check(tempBuiltGrid))
                {
                    manager.Transfer(part, StorageManager.StorageKey.BotBuilder, StorageManager.StorageKey.Inventory);
                }
            }
        }
    }

}
