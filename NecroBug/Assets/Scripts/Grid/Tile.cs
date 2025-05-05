using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public abstract class Tile
{
    private TileType type;
    private Tile dependent;
    public enum TileType
    {
        Empty,
        Outside,
        Solid,
        SolidCheck
    }
    public Tile(Tile dependent, TileType type)
    {
        this.dependent = dependent;
        this.type = type;
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

    public abstract Tile CreateTile(Tile dependent);
    public abstract bool internalCheck(Tile other);


}
