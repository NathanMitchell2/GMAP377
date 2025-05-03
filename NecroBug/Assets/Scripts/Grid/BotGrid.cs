using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BotGrid
{
    private string[,,] grid;
    public BotGrid(Vector3 size)
    {
        grid = new string[(int)size.x, (int)size.y, (int)size.z];
        ClearGrid();
    }
    public BotGrid(int x, int y, int z)
    {
        grid = new string[x, y, z];
        ClearGrid();
    }

    public void ClearGrid()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int k = 0; k < grid.GetLength(2); k++)
                {
                    SetGrid(i, j, k, "empty");
                }
            }
        }
    }

    public bool CheckBlock(Block block)
    {
        Vector3 pos = block.GetPos();
        Vector3 size = block.GetSize();
        for (int i = 0; i < (int)size.x; i++)
        {
            for (int j = 0; j < (int)size.y; j++)
            {
                for (int k = 0; k < (int)size.z; k++)
                {
                    string locName = GetGrid((int)pos.x + i, (int)pos.y + j, (int)pos.z + k);
                    bool check = block.CanPlace(locName);

                    if (!check)
                        return false;
                }
            }
        }
        return true;
    }
    public void SetBlock(Block block)
    {
        Vector3 pos = block.GetPos();
        Vector3 size = block.GetSize();
        for (int i = 0; i < (int)size.x; i++)
        {
            for (int j = 0; j < (int)size.y; j++)
            {
                for (int k = 0; k < (int)size.z; k++)
                {
                    SetGrid((int)pos.x + i, (int)pos.y + j, (int)pos.z + k, block.GetName());
                }
            }
        }
    }
    private void SetGrid(Vector3 loc, string name)
    {
        if(inBounds(loc))
            grid[(int)loc.x, (int)loc.y, (int)loc.z] = name;
    }
    private void SetGrid(int x, int y, int z, string name)
    {
        if (inBounds(x,y,z))
            grid[x, y, z] = name;
    }
    private string GetGrid(Vector3 loc)
    {
        if (inBounds(loc))
            return "null";
        return grid[(int)loc.x, (int)loc.y, (int)loc.z];
    }
    private string GetGrid(int x, int y, int z)
    {
        if (inBounds(x,y,z))
            return "null";
        return grid[x, y, z];
    }

    private bool inBounds(Vector3 loc)
    {
        bool xBounds = (int)loc.x >= 0 && (int)loc.x < grid.GetLength(0);
        bool yBounds = (int)loc.y >= 0 && (int)loc.y < grid.GetLength(1);
        bool zBounds = (int)loc.z >= 0 && (int)loc.z < grid.GetLength(2);

        return xBounds && yBounds && zBounds;
    }
    private bool inBounds(int x, int y, int z)
    {
        bool xBounds = x >= 0 && x < grid.GetLength(0);
        bool yBounds = y >= 0 && y < grid.GetLength(1);
        bool zBounds = z >= 0 && z < grid.GetLength(2);

        return xBounds && yBounds && zBounds;
    }
}
