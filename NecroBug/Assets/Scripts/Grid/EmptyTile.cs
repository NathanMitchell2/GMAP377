using UnityEngine;

public class EmptyTile : Tile
{
    public EmptyTile(Tile dependent) : base(dependent, TileType.Empty)
    {

    }
    public override Tile CreateTile(Tile dependent)
    {
        return new EmptyTile(dependent);
    }

    public override bool internalCheck(Tile other)
    {
        return true;

    }
}
