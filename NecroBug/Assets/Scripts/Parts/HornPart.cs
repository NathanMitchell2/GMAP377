using UnityEngine;

public class HornPart : ModularBugPart
{
    public GameObject hornHitbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate()
    {
        GameObject hitbox = Instantiate(hornHitbox, transform.parent);
        hitbox.transform.SetLocalPositionAndRotation(offset, new Quaternion());
    }
}
