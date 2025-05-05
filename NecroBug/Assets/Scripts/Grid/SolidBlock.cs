using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SolidBlock : Block
{
    public SolidBlock(Vector3 pos, Vector3 size, Vector3 axis) : base(pos, size, axis, "solid")
    {

    }

    public override bool CanPlace(string other)
    {
        return other == "empty";
    }
}
