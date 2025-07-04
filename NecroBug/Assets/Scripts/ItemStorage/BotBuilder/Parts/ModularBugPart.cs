using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class BugPartMemento
{
    Vector3 pos;
    Quaternion rot;
    public BugPartMemento(Vector3 pos, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
    }
    public Vector3 GetPos()
    { return pos; }
    public Quaternion GetRot()
    { return rot; }
}
public abstract class ModularBugPart : MonoBehaviour
{
    public abstract void Activate();

    private MediatorPart mediator;
    public void Initialize(MediatorPart mediator, string binding)
    {
        this.mediator = mediator;
    }
    public BugPartMemento CreateMemento()
    {
        return new BugPartMemento(transform.localPosition, transform.localRotation);
    }
    public void RestoreMemento(BugPartMemento memento)
    {
        transform.SetLocalPositionAndRotation(memento.GetPos(), memento.GetRot());
    }
    public MediatorPart GetMediator()
    { return mediator; }

}
