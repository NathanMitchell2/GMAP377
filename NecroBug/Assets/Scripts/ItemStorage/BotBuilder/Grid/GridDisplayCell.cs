using System.Collections.Generic;
using UnityEngine;

public class GridDisplayCell : MonoBehaviour
{
    [SerializeField] Color falseColor = new Color(1, 0, 0, .4f);
    [SerializeField] Color trueColor = new Color(0, 1, 0, .4f);
    [SerializeField] Color emptyColor = new Color(.5f, .5f, .5f, 0.0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessStack(List<Tile> stack)
    {
        if(stack.Count == 0) return;

        Tile tile = stack[stack.Count-1];
        Material material = GetComponent<Renderer>().material;
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
