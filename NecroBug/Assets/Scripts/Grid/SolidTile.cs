using UnityEngine;

public class SolidTile : Tile
{
    public SolidTile(Tile dependent, Block parent) : base(dependent, parent, TileType.Solid)
    {

    }
    public override Tile CreateTile(Tile dependent, Block parent)
    {
        return new SolidTile(dependent, parent);
    }

    public override bool internalCheck(Tile other)
    {
        return (other.GetTileType() == TileType.Empty) || (other.GetTileType() == TileType.SolidCheck);

    }
}
