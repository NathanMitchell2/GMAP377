using Unity.VisualScripting;
using UnityEngine;

public class JetPart : ModularBugPart
{
    [SerializeField] private float dashForce = 20f;
    public override void Activate()
    {
        Transform parent = transform.parent;
        if (parent)
        {
            Rigidbody rigid = parent.GetComponent<Rigidbody>();
            if(rigid)
            {
                Vector3 camFor = Camera.main.transform.forward;
                rigid.linearVelocity = new Vector3(camFor.x*dashForce, rigid.linearVelocity.y, camFor.z*dashForce);
                //rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, jumpForce, rigid.linearVelocity.y);
            }
        }
    }
}
