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

    public MediatorPart mediator;

    public void Initialize(MediatorPart mediator)
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

    private void Update()
    {
        ConnectAnchor();
    }
    protected void ConnectAnchor()
    {
        MediatorPart med = GetMediator();
        Joint joint = GetComponent<FixedJoint>();
        if (med != null && joint != null)
        {
            Vector3 pos = med.GetBotPos();// + med.GetStoragePos();
                                          //Quaternion rot = med.GetStorageRot();// * parent.rotation;

            Debug.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" + med.GetName());
            joint.connectedAnchor = pos;
            joint.connectedAnchor = overridePos;
            joint.anchor = pos;
            joint.anchor = overridePos;
        }
    }

    public Vector3 overridePos;

}
