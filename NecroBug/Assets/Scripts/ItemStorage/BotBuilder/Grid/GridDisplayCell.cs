using System.Collections.Generic;
using UnityEngine;

public class GridDisplayCell : MonoBehaviour
{
    public enum CellState
    {
        Empty,
        Valid,
        Invalid,
        Selected
    }
    [SerializeField] Color falseColor = new Color(1, 0, 0, .4f);
    [SerializeField] Color trueColor = new Color(0, 1, 0, .4f);
    [SerializeField] Color emptyColor = new Color(.5f, .5f, .5f, 0.0f);
    [SerializeField] Color selectedColor = new Color(1f, 1f, 0f, .4f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetState(CellState state)
    {
        Material material = GetComponent<Renderer>().material;
        switch (state)
        {
            case CellState.Empty:
                material.color = emptyColor;
                break;
            case CellState.Valid:
                material.color = trueColor;
                break;
            case CellState.Invalid:
                material.color = falseColor;
                break;
            case CellState.Selected:
                material.color = selectedColor;
                break;
        }
    }

    public void ProcessStack(List<Tile> stack)
    {
        Material material = GetComponent<Renderer>().material;
        if (stack.Count == 0)
        {
            material.color = emptyColor;
            return;

        }

        Tile tile = stack[stack.Count-1];
        if (tile != null) {
            if (tile.GetTileType() == Tile.TileType.Empty)
            {
                material.color = emptyColor;
            }
            else if (Tile.CheckCell(stack))
            {
                material.color = trueColor;
            }
            else
            {
                material.color = falseColor;
            }
        }

    }
}
