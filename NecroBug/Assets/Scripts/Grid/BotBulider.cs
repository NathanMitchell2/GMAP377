using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BotBulider : MonoBehaviour
{
    private const int x = 12;
    private const int y = 12;
    private const int z = 12;
    //[SerializeField] GameObject bot;
    [SerializeField] GameObject cell;
    [SerializeField] Tile empty;
    [SerializeField] Transform gridTransform;
    private BotGrid grid;
    private List<BotPart> parts = new List<BotPart>();
    private BotPart selectedPart;
    private GridDisplayCell[,,] gridDisplayCells;
    [SerializeField] private Transform buildTransform;
    [SerializeField] private GameObject playerCar;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private UIUpdate uiObject;
    private List<GameObject> builtParts = new List<GameObject>();
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

    public int GetCount()
    {
        return parts.Count;
    }
    public BotPart GetIndex(int index)
    {
        return parts[index];
    }
    public void AddPart(BotPart part)
    {
        part.Place(grid);
        //Instantiate(part.gameObject, gridTransform);
        actionManager.AddAction(part.defaultBind);
        parts.Add(part);
    }
    public bool MovePart(BotPart part, Vector3 pos)
    {
        return part.Move(pos, grid);
    }
    public bool MovePart(BotPart part, int x, int y, int z)
    {
        return part.Move(new Vector3(x,y,z), grid);
    }
    
    public bool RotatePart(BotPart part, Vector3 axis)
    {
        return part.Rotate(axis, grid);
    }
    public bool RotatePart(BotPart part, int x, int y, int z)
    {
        return part.Rotate(new Vector3(x, y, z), grid);
    }
    public bool RemovePart(BotPart part)
    {
        if(parts.IndexOf(part)!=0 && part.Remove(grid)) //HARD CODED, can't remove first item in list (for car)
        {
            int index = parts.IndexOf(part);
            if (index != -1)
            {
                BotPart part2 = parts[index];
                actionManager.RemoveAction(index);
                parts.RemoveAt(index);
                Destroy(part2.gameObject);
                return true;
            }
        }
        return false;

    }
    public bool RemovePart(int index)
    {
        if (index != 0&&parts[index].Remove(grid)) //HARD CODED, can't remove first item in list (for car)
        {
            if (index != -1)
            {
                BotPart part2 = parts[index];
                actionManager.RemoveAction(index);
                parts.RemoveAt(index);
                Destroy(part2.gameObject);
                return true;
            }
        }
        return false;
    }


    public void SetSelected(int index)
    {
        selectedPart = parts[index];
    }
    public void SetSelected(BotPart part)
    {
        if(parts.Contains(part))
            selectedPart = part;
    }
    public bool MoveSelected(Vector3 pos)
    {
        return selectedPart.Move(pos, grid);
    }
    public bool MoveSelected(int x, int y, int z)
    {
        return selectedPart.Move(new Vector3(x, y, z), grid);
    }
    public bool RotateSelected(Vector3 axis)
    {
        return selectedPart.Rotate(axis, grid);
    }
    public bool RotateSelected(int x, int y, int z)
    {
        return selectedPart.Rotate(new Vector3(x, y, z), grid);
    }
    public bool RemoveSelected()
    {
        return RemovePart(selectedPart);
    }
    public BotPart GetSelected()
    {
        return selectedPart;
    }

    private void DestroyBot()
    {
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
    }
    public void CreateBot()
    {
        if (!grid.Check())
            return;
        Vector3 ogPos;
        if (builtParts.Count == 0)
            ogPos = playerObject.transform.GetChild(0).localPosition;
        else
            ogPos = builtParts[0].transform.localPosition;
        DestroyBot();
        PlayerStats car = null;
        builtParts = new List<GameObject>();

        for(int i = 0; i < parts.Count; i++)
        {
            BotPart part = parts[i];
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
                statsReference.SetItem(part.GetComponent<InventoryThing>().GetItem());
                statsReference.health = (int) part.GetComponent<InventoryThing>().GetHealth(); // playerReference.health;
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
        car.transform.SetLocalPositionAndRotation(ogPos/*buildTransform.GetComponentInChildren<FollowCar>().gameObject.transform.position*/, Quaternion.identity);
        car.transform.localScale = Vector3.one;

        uiObject.StatInitialize();
    }
    public bool ProgressOrientationSelected()
    {
        return selectedPart.ProgressOrientation(grid);
    }


    public void BindAction(int index)
    {
        actionManager.RebindAction(index);
    }

    public string GetKeybindText(int index)
    {
        return actionManager.GetKeybindText(index);
    }

    public int IndexOf(BotPart part)
    {
        return parts.IndexOf(part);
    }
}
