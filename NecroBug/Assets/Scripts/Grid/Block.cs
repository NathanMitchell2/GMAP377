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

    private void Start()
    {
        tiles = new List<Tile>();
    }

    private Tile GenTile(Tile dependent)
    {
        switch (tile)
        {
            case Tile.TileType.Solid:
                return new SolidTile(dependent);
            case Tile.TileType.Empty:
                return new EmptyTile(dependent);
            default:
                return new EmptyTile(dependent);
        }
    }

    public void Place(Vector3 pos, Vector3 axis, BotGrid grid)
    {
        Vector3 size = GetSizeByAxis(axis);

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    int x = (int)pos.x + i;
                    int y = (int)pos.y + j;
                    int z = (int)pos.z + k;

                    Tile tile;
                    if (grid.GetCell(x, y, z).Count == 0)
                        tile = GenTile(null);
                    else
                        tile = GenTile(grid.GetCell(x, y, z).Peek());


                    grid.GetCell(x, y, z).Push(tile);
                    //Debug.Log(tile);
                    //Debug.Log(tiles);
                    //tiles.Add(tile);
                }
            }
        }
    }
    public bool Remove(Vector3 pos, Vector3 axis, BotGrid grid)
    {
        Debug.Log("c");
        if (!CanRemove(pos, axis, grid))
            return false;

        Vector3 size = GetSizeByAxis(axis);

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    int x = (int)pos.x + i;
                    int y = (int)pos.y + j;
                    int z = (int)pos.z + k;

                    Tile tile = grid.GetCell(x, y, z).Pop();
                    //tiles.Remove(tile);
                }
            }
        }
        return true;
    }

    public bool CanRemove(Vector3 pos, Vector3 axis, BotGrid grid)
    {
        return true;
        Vector3 size = GetSizeByAxis(axis);

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    int x = (int)pos.x + i;
                    int y = (int)pos.y + j;
                    int z = (int)pos.z + k;

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
