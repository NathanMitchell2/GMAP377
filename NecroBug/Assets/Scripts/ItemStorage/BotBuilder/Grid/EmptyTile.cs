using UnityEngine;

public class EmptyTile : Tile
{
    public EmptyTile(Tile dependent, Block parent) : base(dependent, parent, TileType.Empty)
    {

    }
    public override Tile CreateTile(Tile dependent, Block parent)
    {
        return new EmptyTile(dependent, parent);
    }

    public override bool internalCheck(Tile other)
    {
        return (other.GetTileType() != TileType.SolidCheck);

    }
}
