using UnityEngine;

public class TransparentTile : Tile
{
    public TransparentTile(Tile dependent, Block parent) : base(dependent, parent, TileType.Transparent)
    {

    }
    public override Tile CreateTile(Tile dependent, Block parent)
    {
        return new TransparentTile(dependent, parent);
    }

    public override bool internalCheck(Tile other)
    {
        return (other.GetTileType() == null);

    }
}
