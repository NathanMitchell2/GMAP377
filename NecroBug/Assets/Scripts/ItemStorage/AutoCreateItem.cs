using System;
using UnityEngine;

public class AutoCreateItem : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private int count = 1;


    void Awake()
    {
        if (item != null && item.GetComponent<InventoryItem>())
        {
            //Git break avoid
            GameObject temp = Instantiate(item);

            InventoryItem nItem = temp.GetComponent<InventoryItem>();
            nItem.SetCount(count);

            ItemMemento mem = nItem.CreateMemento();

            Destroy(temp);

            Component comp = gameObject.AddComponent(mem.GetItemType());
            ((InventoryItem)comp).RestoreMemento(mem);
        }
        Destroy(this);
    }
}
