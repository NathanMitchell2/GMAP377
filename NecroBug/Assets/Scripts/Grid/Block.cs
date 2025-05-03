using Unity.VisualScripting;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 size;
    [SerializeField] private Vector3 axis;
    [SerializeField] protected string name;

    public abstract bool CanPlace(string other);
    public Block(Vector3 pos, Vector3 size, Vector3 axis, string name)
    {
        this.pos = pos;
        this.size = size;
        this.axis = axis;
        this.name = name;
    }

    public Vector3 GetPos() {
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.FromToRotation(Vector3.right, axis));
        return rotation.MultiplyPoint(pos);
    }
    public Vector3 GetSize()
    {
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.FromToRotation(Vector3.right, axis));
        return rotation.MultiplyVector(size);
    }
    public string GetName() {  return name; }
    public Vector3 GetAxis() { return axis; }
    public void SetAxis(Vector3 axis)
    {
        this.axis = axis;
    }

}
