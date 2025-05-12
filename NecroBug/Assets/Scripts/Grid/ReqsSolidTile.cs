using System.Transactions;
using UnityEngine;
using static Tile;

public class ReqsSolidTile : Tile
{
    public ReqsSolidTile(Tile dependent, Block parent) : base(dependent, parent, TileType.SolidCheck)
    {

    }
    public override Tile CreateTile(Tile dependent, Block parent)
    {
        return new ReqsSolidTile(dependent, parent);
    }

    public override bool internalCheck(Tile other)
    {
        return (other.GetTileType() == TileType.SolidCheck)&&(dependent.GetTileType()==TileType.Solid);

    }
}
