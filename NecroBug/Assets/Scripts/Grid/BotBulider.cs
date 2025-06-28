using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class BotBulider : MonoBehaviour
{
    private const int x = 12;
    private const int y = 12;
    private const int z = 12;

    private BotGrid grid;
    private List<MediatorPart> gridParts = new List<MediatorPart>();
    private List<MediatorPart> builtParts = new List<MediatorPart>();

    private GridDisplayCell[,,] gridDisplayCells;

    [SerializeField] GameObject cell;
    [SerializeField] Tile empty;
    [SerializeField] Transform gridTransform;

    [SerializeField] private Transform buildTransform;
    [SerializeField] private GameObject playerCar;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private UIUpdate uiObject;
    [SerializeField] private ActionManager actionManager;



    public void SetUp()
    {
        grid = new BotGrid(x, y, z, empty);
        //GameObject bot = Instantiate(this.bot, transform);
        //this.selectedPart = bot.GetComponent<BotPart>();
        //parts.Add(selectedPart);

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
                    gridDisplayCells[i,j,k] = newCell.GetComponent<GridDisplayCell>();
                }
            }
        }
        Destroy(playerObject.transform.GetChild(0).gameObject);
    }
    private void Awake()
    {
        SetUp();

    }
    private void Start()
    {
        MediatorPart part = (MediatorPart)GetComponent<InventoryManager>().items[0];

        part.CreateBotPart();
        BotPart bPart = part.GetBotPart();

        bPart.SetPos(new Vector3(4, 4, 4));
        AddPart(part);

        CreateBot();
    }

    public List<ItemMemento> CreateGridMementos()
    {
        List<ItemMemento> temp = new List<ItemMemento>();

        foreach (MediatorPart part in gridParts)
        {
            temp.Add(part.CreateMemento());
        }

        return temp;
    }

    public List<ItemMemento> CreateBuiltMementos()
    {
        List<ItemMemento> temp = new List<ItemMemento>();

        foreach(MediatorPart part in builtParts)
        {
            temp.Add(part.CreateMemento());
        }

        return temp;
    }

    public bool Check()
    {
        return grid.Check();
    }
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

    //Parts lists
    public List<MediatorPart> GetGridParts()
    {
        return gridParts;
    }

    public List<MediatorPart> GetBuiltParts()
    {
        return builtParts;
    }

    public int GetCount()
    {
        return gridParts.Count;
    }
    public void AddPart(MediatorPart mPart)
    {
        BotPart part = mPart.GetBotPart();

        part.Place(grid);
        gridParts.Add(mPart);
    }
    private bool MovePart(BotPart part, Vector3 pos)
    {
        return part.Move(pos, grid);
    }
    public bool MovePart(int i, Vector3 pos)
    {
        return MovePart(gridParts[i].GetBotPart(), pos);
    }
    private bool ProgressOrientation(BotPart part)
    {
        return part.ProgressOrientation(grid);
    }
    public bool ProgressOrientation(int i)
    {
        return ProgressOrientation(gridParts[i].GetBotPart());
    }

    private bool RemovePart(MediatorPart mPart)
    {
        BotPart part = mPart.GetBotPart();
        int index = gridParts.IndexOf(mPart);

        if (index != 0 && index != -1 && part.Remove(grid)) //HARD CODED, can't remove first item in list (for car)
        {
            mPart.DestroyBotPart();
            return true;
        }
        return false;

    }
    public bool RemovePart(int index)
    {
        return RemovePart(gridParts[index]);
    }
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
        */
    }
    public bool DestroyPart(MediatorPart part)
    {
        return DestroyPart(gridParts.IndexOf(part));
    }
    public void RemoveBuiltPart(int index)
    {
        builtParts.RemoveAt(index);
    }

    public int BuiltIndexOf(MediatorPart mediatorPart)
    {
        return builtParts.IndexOf(mediatorPart);
    }

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
        /*
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
                ogPos = player.transform.GetChild(i).transform.localPosition;

        }

        foreach (MediatorPart part in builtParts)
        {
            if(!gridParts.Contains(part))
            {
                part.DestroyBugPart();
            }
        }
        builtParts = new List<MediatorPart>();

        GameObject car = gameObject;

        for (int i = 0; i < gridParts.Count;i++)
        {
            gridParts[i].CreateBugPart();
            if (i == 0)
            {
                car = gridParts[i].GetBugPart().gameObject;
            }
            gridParts[i].GetBugPart().transform.SetParent(car.transform);
            builtParts.Add(gridParts[i]);
        }

        car.transform.SetParent(buildTransform);
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


    public void BindAction(int index)
    {
        gridParts[index].RebindAction();
    }

    public string GetKeybindText(int index)
    {
        return gridParts[index].GetKeybindText();
    }

    public int IndexOf(MediatorPart part)
    {
        return gridParts.IndexOf(part);
    }

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

}
