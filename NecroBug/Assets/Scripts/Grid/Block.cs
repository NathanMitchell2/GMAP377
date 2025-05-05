using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Block : MonoBehaviour
{
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
            default:
                return new EmptyTile(dependent, this);
        }
    }

    public void Place(Vector3 pos, Vector3 axis, BotGrid grid)
    {
        Vector3 size = GetSizeByAxis(axis);

        for (int i = 0; i < Math.Abs(size.x); i++)
        {
            for (int j = 0; j < Math.Abs(size.y); j++)
            {
                for (int k = 0; k < Math.Abs(size.z); k++)
                {
                    int x = (int)pos.x + i * Math.Sign(size.x);
                    int y = (int)pos.y + j * Math.Sign(size.y);
                    int z = (int)pos.z + k * Math.Sign(size.z);

                    Tile tile;
                    if (grid.GetCell(x, y, z).Count == 0)
                        tile = GenTile(null);
                    else
                        tile = GenTile(grid.GetCell(x, y, z).Peek());


                    grid.GetCell(x, y, z).Push(tile);
                    tiles.Add(tile);
                }
            }
        }
    }
    public bool Remove(Vector3 pos, Vector3 axis, BotGrid grid)
    {
        if (!CanRemove(pos, axis, grid))
            return false;

        Vector3 size = GetSizeByAxis(axis);

        for (int i = 0; i < Math.Abs(size.x); i++)
        {
            for (int j = 0; j < Math.Abs(size.y); j++)
            {
                for (int k = 0; k < Math.Abs(size.z); k++)
                {
                    int x = (int)pos.x + i * Math.Sign(size.x);
                    int y = (int)pos.y + j * Math.Sign(size.y);
                    int z = (int)pos.z + k * Math.Sign(size.z);

                    Tile tile = grid.GetCell(x, y, z).Pop();
                    tiles.Remove(tile);
                }
            }
        }
        return true;
    }

    public bool CanRemove(Vector3 pos, Vector3 axis, BotGrid grid)
    {
        Vector3 size = GetSizeByAxis(axis);

        for (int i = 0; i < Math.Abs(size.x); i++)
        {
            for (int j = 0; j < Math.Abs(size.y); j++)
            {
                for (int k = 0; k < Math.Abs(size.z); k++)
                {
                    int x = (int)pos.x + i * Math.Sign(size.x);
                    int y = (int)pos.y + j * Math.Sign(size.y);
                    int z = (int)pos.z + k * Math.Sign(size.z);

                    if (grid.GetCell(x, y, z).Peek().GetTileType() == Tile.TileType.Empty)
                        return false;

                    if (!tiles.Contains(grid.GetCell(x, y, z).Peek()))
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
