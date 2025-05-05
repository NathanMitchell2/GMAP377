using UnityEngine;

public class SolidTile : Tile
{
    public SolidTile(Tile dependent) : base(dependent, TileType.Solid)
    {

    }
    public override Tile CreateTile(Tile dependent)
    {
        return new SolidTile(dependent);
    }

    public override bool internalCheck(Tile other)
    {
        return (other.GetTileType() == TileType.Empty) || (other.GetTileType() == TileType.SolidCheck);

    }
}
