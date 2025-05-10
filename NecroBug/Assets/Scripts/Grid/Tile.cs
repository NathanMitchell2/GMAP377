using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public abstract class Tile
{
    private TileType type;
    private Tile dependent;
    private Block parent; // Isn't used
    public enum TileType
    {
        Empty,
        Outside,
        Solid,
        SolidCheck
    }
    public Tile(Tile dependent, Block parent, TileType type)
    {
        this.dependent = dependent;
        this.parent = parent;
        this.type = type;
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
        return internalCheck(other) && CheckTile();
    }

    public TileType GetTileType()
    {
        return type;
    }

    public abstract Tile CreateTile(Tile dependent, Block parent);
    public abstract bool internalCheck(Tile other);


}
