using UnityEngine;

public class InstantAddItem : MonoBehaviour
{
    public static bool doCreate = true;
    [SerializeField] private GameObject item;

    void Start()
    {
        if (doCreate && item != null && item.GetComponent<InventoryItem>() && GetComponent<ItemStorage>() != null)
        {
            ItemMemento mem = item.GetComponent<InventoryItem>().CreateMemento();

            GetComponent<ItemStorage>().AddItem(mem);
        }
        Destroy(this);
    }
}
