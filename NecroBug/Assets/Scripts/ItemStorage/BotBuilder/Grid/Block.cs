using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 size;
    [SerializeField] private Tile.TileType tile;
    [SerializeField] private List<Tile> tiles = new List<Tile>();
    public Block(Vector3 size, Tile.TileType tile)
    {
        this.size = size;
        this.tile = tile;
    }

    private void Awake()
    {
        tiles = new List<Tile>();
    }

    private Tile GenTile(Tile dependent)
    {
        switch (tile)
        {
            case Tile.TileType.Solid:
                return new SolidTile(dependent, this);
            case Tile.TileType.Empty:
                return new EmptyTile(dependent, this);
            case Tile.TileType.SolidCheck:
                return new ReqsSolidTile(dependent, this);
            case Tile.TileType.Transparent:
                return new TransparentTile(dependent, this);
            default:
                return new EmptyTile(dependent, this);
        }
    }

    public bool inBounds(Vector3 pos, BotGrid grid)
    {
        Vector3 size = GetSizeByAxis(Vector3.right);
        Vector3 rPos = this.pos;

        for (int i = 0; i < Math.Abs(size.x); i++)
        {
            for (int j = 0; j < Math.Abs(size.y); j++)
            {
                for (int k = 0; k < Math.Abs(size.z); k++)
                {
                    int x = (int)pos.x + (int)rPos.x + i * Math.Sign(size.x);
                    int y = (int)pos.y + (int)rPos.y + j * Math.Sign(size.y);
                    int z = (int)pos.z + (int)rPos.z + k * Math.Sign(size.z);

                    if (!grid.inBounds(new Vector3(x, y, z)))
                        return false;
                }
            }
        }

        return true;
    }

    public bool Place(Vector3 pos, BotGrid grid)
    {
        if (!inBounds(pos, grid))
            return false;

        Vector3 size = GetSizeByAxis(Vector3.right);
        Vector3 rPos = this.pos;

        for (int i = 0; i < (int)Math.Floor(Math.Abs(size.x)); i++)
        {
            for (int j = 0; j < (int)Math.Floor(Math.Abs(size.y)); j++)
            {
                for (int k = 0; k < (int)Math.Floor(Math.Abs(size.z)); k++)
                {
                    int x = (int)Math.Floor(pos.x + rPos.x) + i * Math.Sign(size.x);
                    int y = (int)Math.Floor(pos.y + rPos.y) + j * Math.Sign(size.y);
                    int z = (int)Math.Floor(pos.z + rPos.z) + k * Math.Sign(size.z);

                    Tile tile;
                    if (grid.GetCell(x, y, z).Count == 0)
                        tile = GenTile(null);
                    else
                        tile = GenTile(grid.GetCell(x, y, z)[grid.GetCell(x,y,z).Count-1]);


                    grid.GetCell(x, y, z).Add(tile);
                    if (tiles == null)
                        tiles = new List<Tile>();
                    tiles.Add(tile);
                }
            }
        }
        return true;
    }
    private int GetIndexInCell(List<Tile> cell)
    {
        for (int i = 0; i < cell.Count; i++)
        {
            if (tiles.Contains(cell[i]))
                return i;
        }
        return -1;
    }


    public bool Remove(Vector3 pos, BotGrid grid)
    {
        if (!CanRemove(pos, grid))
            return false;

        if (!inBounds(pos, grid))
            return false;

        Vector3 size = GetSizeByAxis(Vector3.right);
        Vector3 rPos = this.pos;

        for (int i = 0; i < (int)Math.Floor(Math.Abs(size.x)); i++)
        {
            for (int j = 0; j < (int)Math.Floor(Math.Abs(size.y)); j++)
            {
                for (int k = 0; k < (int)Math.Floor(Math.Abs(size.z)); k++)
                {
                    int x = (int)Math.Floor(pos.x + rPos.x) + i * Math.Sign(size.x);
                    int y = (int)Math.Floor(pos.y + rPos.y) + j * Math.Sign(size.y);
                    int z = (int)Math.Floor(pos.z + rPos.z) + k * Math.Sign(size.z);

                    List<Tile> cell = grid.GetCell(x, y, z);
                    int index = GetIndexInCell(cell);

                    tiles.Remove(cell[index]);
                    cell.RemoveAt(index);
                }
            }
        }
        return true;
    }


    public bool CanRemove(Vector3 pos,  BotGrid grid)
    {
        if (!inBounds(pos,  grid))
            return false;

        if (!inBounds(pos,  grid))
            return false;

        Vector3 size = GetSizeByAxis(Vector3.right);
        Vector3 rPos = this.pos;

        for (int i = 0; i < (int)Math.Floor(Math.Abs(size.x)); i++)
        {
            for (int j = 0; j < (int)Math.Floor(Math.Abs(size.y)); j++)
            {
                for (int k = 0; k < (int)Math.Floor(Math.Abs(size.z)); k++)
                {
                    int x = (int)Math.Floor(pos.x + rPos.x) + i * Math.Sign(size.x);
                    int y = (int)Math.Floor(pos.y + rPos.y) + j * Math.Sign(size.y);
                    int z = (int)Math.Floor(pos.z + rPos.z) + k * Math.Sign(size.z);


                    List<Tile> cell = grid.GetCell(x, y, z);
                    int index = GetIndexInCell(cell);

                    if(index == -1)
                        return false;
                    /*
                    if (grid.GetCell(x, y, z).Peek().GetTileType() == Tile.TileType.Empty)
                    return false;

                    if (!tiles.Contains(grid.GetCell(x, y, z).Peek()))
                    return false;
                    */
                }
            }
        }
        return true;
    }

    public bool Check(Vector3 pos, BotGrid grid)
    {
        Vector3 size = GetSizeByAxis(Vector3.right);
        Vector3 rPos = this.pos;

        for (int i = 0; i < Math.Abs(size.x); i++)
        {
            for (int j = 0; j < Math.Abs(size.y); j++)
            {
                for (int k = 0; k < Math.Abs(size.z); k++)
                {
                    int x = (int)pos.x + (int)rPos.x + i * Math.Sign(size.x);
                    int y = (int)pos.y + (int)rPos.y + j * Math.Sign(size.y);
                    int z = (int)pos.z + (int)rPos.z + k * Math.Sign(size.z);

                    if (!grid.inBounds(new Vector3(x, y, z)))
                        return false;

                    if(!Tile.CheckCell(grid.GetCell(x, y, z)))
                        return false;
                }
            }
        }

        return true;
    }

    public Vector3 GetSizeByAxis(Vector3 axis)
    {
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.FromToRotation(Vector3.right, axis));
        return rotation.MultiplyVector(size);
    }

}
