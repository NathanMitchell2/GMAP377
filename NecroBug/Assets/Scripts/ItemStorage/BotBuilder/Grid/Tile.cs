using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Tile
{
    private TileType type;
    protected Tile dependent;
    private Block parent; // Isn't used

    public enum TileType
    {
        Empty,
        neededSoDoesNotBreak,
        Solid,
        SolidCheck,
        Transparent
    }
    public Tile(Tile dependent, Block parent, TileType type)
    {
        this.dependent = dependent;
        this.parent = parent;
        this.type = type;
    }

    public static bool CheckCell(List<Tile> cellOG)
    {
        List<Tile> cell = new List<Tile> ();
        cell.AddRange(cellOG);

        SortCell(cell);

        Tile prev = null;
        for(int i = cell.Count - 1; i >= 0; i--)
        {

            if(!cell[i].CheckTile(prev))
                return false;
            prev = cell[i];
        }
        return true;
    }
    private static void SortCell(List<Tile> cell)
    {
        int partition = 0;
        for (int i = 0; i < cell.Count; i++)
        {
            Tile min = cell[partition];
            int minPos = partition;
            for(int j = partition+1; j < cell.Count; j++)
            {
                if(cell[j].GetTileValue() < min.GetTileValue())
                {
                    min = cell[j];
                    minPos = j;
                }
            }
            cell.RemoveAt(minPos);
            cell.Insert(partition, min);
            partition++;
        }
        //Debug.Log(cell.ToCommaSeparatedString());
        // Debug.Log(cell.ToCommaSeparatedString());
        return;
    }

    public int GetTileValue()
    {
        return (int)GetTileType();
    }

    public Block GetParent()
    {
        return parent;
    }

    public bool CheckTile()
    {
        if (dependent == null) return true;

        return dependent.CheckTile(this);
    }
    public bool CheckTile(Tile other)
    {
        if (other == null) return true;
        return internalCheck(other);// && CheckTile();
    }

    public TileType GetTileType()
    {
        return type;
    }

    public abstract Tile CreateTile(Tile dependent, Block parent);
    public abstract bool internalCheck(Tile other);


}
