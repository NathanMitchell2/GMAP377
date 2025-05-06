using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotBulider : MonoBehaviour
{
    private const int x = 12;
    private const int y = 12;
    private const int z = 12;
    [SerializeField] GameObject bot;
    [SerializeField] GameObject cell;
    [SerializeField] Tile empty;
    [SerializeField] Transform camera;
    private BotGrid grid;
    private List<BotPart> parts = new List<BotPart>();
    private BotPart selectedPart;
    private GridDisplayCell[,,] gridDisplayCells;

    private void Awake()
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
                    GameObject newCell = Instantiate(cell, camera);
                    newCell.transform.SetLocalPositionAndRotation(new Vector3(i, j, k), new Quaternion());
                    //Debug.Log(newCell==null);
                    gridDisplayCells[i,j,k] = newCell.GetComponent<GridDisplayCell>();
                }
            }
        }
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
        if(part.Remove(grid))
            return parts.Remove(part);
        return false;

    }
    public bool RemovePart(int index)
    {
        if (parts[index].Remove(grid))
            return parts.Remove(parts[index]);
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
        if (selectedPart.Remove(grid))
            return parts.Remove(selectedPart);
        return false;

    }
    public BotPart GetSelected()
    {
        return selectedPart;
    }
}
