using Unity.VisualScripting;
using UnityEngine;

public class JetPart : ModularBugPart
{
    //93.75 mass
    [SerializeField] static private float dashForce = 20f;
    public override void Activate()
    {
        Transform parent = transform.parent;
        if (parent)
        {
            Rigidbody rigid = parent.GetComponent<Rigidbody>();
            if(rigid)
            {
                
                Vector3 dashDir = transform.right;
                rigid.linearVelocity = new Vector3(dashDir.x*dashForce, rigid.linearVelocity.y, dashDir.z*dashForce);
                //rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, jumpForce, rigid.linearVelocity.y);
            }
        }
    }

    public static float getDash()
    {
        return dashForce;
    }
    public static void setDash(float speed)
    {
        dashForce = speed;
    }
}
