using UnityEngine;

public class HornPart : ModularBugPart
{
    public Vector3 offset;
    public GameObject hornHitbox;

    public GameObject effect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy(){
        //Instantiate(effect, transform.position, Quaternion.identity);
        //Destroy(effect);
    }

    public override void Activate()
    {
        GameObject hitbox = Instantiate(hornHitbox, transform.parent);
        hitbox.transform.SetLocalPositionAndRotation(offset, new Quaternion());
    }
}
