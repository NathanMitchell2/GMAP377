using UnityEngine;

public abstract class ModularBugPart : MonoBehaviour
{
    public Vector3 offset = new Vector3(0,0.3f,0);
    public Vector3 rotOffset = new Vector3();
    public abstract void Activate();

    private MediatorPart mediator;
    public void Initialize(MediatorPart mediator)
    {
        this.mediator = mediator;
    }

    public MediatorPart GetMediator()
    { return mediator; }
    public void OffsetPosition()
    {
        transform.SetLocalPositionAndRotation(offset,Quaternion.Euler(rotOffset));
    }

}
