using System;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InstantAddItem : MonoBehaviour
{
    public static bool doCreate = true;
    [SerializeField] private GameObject item;
    [SerializeField] private int count = 1;
    [SerializeField] private Vector3 botPos;


    void Start()
    {
        if (doCreate && item != null && item.GetComponent<InventoryItem>() && GetComponent<ItemStorage>() != null)
        {
            
            GameObject temp = Instantiate(item);
            InventoryItem nItem = temp.GetComponent<InventoryItem>();
            nItem.SetCount(count);

            try
            {
                MediatorPart part = (MediatorPart)nItem;
                part.CreateBotPart();
                part.GetBotPart().SetPos(botPos);
                /*
                BotBulider builder = GetComponent<BotBulider>();
                if (part.HasBotPart() && builder != null)
                {
                    Debug.LogError("Early Set Bot Part");
                    builder.MovePart(builder.IndexOf(part.GetBotPart()), botPos);
                }
                */
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            ItemMemento mem = nItem.CreateMemento();
            GetComponent<ItemStorage>().AddItem(mem);
            Destroy(temp);
        }
        Destroy(this);
    }
}
