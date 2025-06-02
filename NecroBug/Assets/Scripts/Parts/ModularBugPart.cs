using UnityEngine;

public abstract class ModularBugPart : MonoBehaviour
{
    public Vector3 offset = new Vector3(0,0.3f,0);
    public Vector3 rotOffset = new Vector3();
    public abstract void Activate();

    public void Awake()
    {
        PlayerStats stats = GetComponent<PlayerStats>();
        InventoryThing thing = GetComponent<InventoryThing>();
        if(stats != null && thing != null)
        {
            stats.health = (int)thing.GetHealth();
        }
    }

    public void OffsetPosition()
    {
        transform.SetLocalPositionAndRotation(offset,Quaternion.Euler(rotOffset));
    }

    public void CleanUp()
    {
        PlayerStats stats = GetComponent<PlayerStats>();
        InventoryThing thing = GetComponent<InventoryThing>();
        if (stats != null && thing != null)
        {
            thing.SetHealth(stats.health);
            thing.SetHealth(50);
        }
    }
}
