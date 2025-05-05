using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BotGrid
{
    private Tile empty;
    private Stack<Tile>[,,] grid;
    public BotGrid(Vector3 size, Tile empty)
    {
        grid = new Stack<Tile>[(int)size.x, (int)size.y, (int)size.z];
        this.empty = new EmptyTile(null, null);
        ClearGrid();
    }
    public BotGrid(int x, int y, int z, Tile empty)
    {
        grid = new Stack<Tile>[x, y, z];
        this.empty = new EmptyTile(null, null);
        ClearGrid();
    }

    public Stack<Tile> GetCell(int x, int y, int z)
    {
        return grid[x, y, z];
    }

    public void ClearGrid()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                for (int k = 0; k < grid.GetLength(2); k++)
                {
                    Stack<Tile> stack = new Stack<Tile>();
                    stack.Push(empty);
                    grid[i, j, k] = stack;
                }
            }
        }
    }

    public bool Check()
    {
        foreach(var tile in grid)
        {
            if(!tile.Peek().CheckTile()) return false;
        }
        return true;
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
