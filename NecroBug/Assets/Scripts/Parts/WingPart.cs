using UnityEngine;

public class WingPart : ModularBugPart
{
    [SerializeField] private float jumpForce = 10f;
    public override void Activate()
    {
        Transform parent = transform.parent;
        if (parent)
        {
            Rigidbody rigid = parent.GetComponent<Rigidbody>();
            if(rigid)
            {
                rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, jumpForce, rigid.linearVelocity.y);
            }
        }
    }
}
