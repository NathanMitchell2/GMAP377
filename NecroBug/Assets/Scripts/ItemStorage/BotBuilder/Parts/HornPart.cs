using UnityEngine;

public class HornPart : ModularBugPart
{
    public Vector3 offset;
    public GameObject hornHitbox;

    public GameObject effect;

    void OnDestroy(){
        //Instantiate(effect, transform.position, Quaternion.identity);
        //Destroy(effect);
    }

    public override void Activate()
    {
        GameObject hitbox = Instantiate(hornHitbox, transform.parent);
        hitbox.transform.SetLocalPositionAndRotation(offset+transform.localPosition, new Quaternion());
    }
    //93.75
    //mass? 375
}
