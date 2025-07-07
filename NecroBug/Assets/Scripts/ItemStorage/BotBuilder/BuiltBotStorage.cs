using System.Data.Common;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEditorInternal.ReorderableList;

public class BuiltBotStorage : ItemStorage
{
    public static Vector3 sourcePos;
    public static Quaternion sourceRot;
    public override InventoryItem AddItem(ItemMemento item)
    {
        if (item == null || item.GetItemType() != typeof(MediatorPart))
            return null;
        Debug.LogError("Buffer Post null checks");

        Transform source = GetSource();

        MediatorPart tItem = (MediatorPart)gameObject.AddComponent(item.GetItemType());
        tItem.RestoreMemento(item);

        if (!tItem.HasBugPart())
        {
            Debug.LogError("Create Bug Part");
            tItem.CreateBugPart();
        }    

        AlignAll();

        Debug.LogError("Buffer End");
        return tItem;
    }
    private Transform GetSource()
    {
        foreach(MediatorPart p in GetItemList())
        {
            if(p.HasBugPart())
            {
                ModularBugPart part = p.GetBugPart();

                if(part.GetType() == typeof(NecroBugPart))
                {
                    return part.transform;
                }
            }
        }
        return null;
    }
    private void AlignSource()
    {
        Transform source = GetSource();
        if (source == null)
            return;

        //source.SetLocalPositionAndRotation(sourcePos, sourceRot);
        source.localScale = Vector3.one;
    }

    public void AlignAll()
    {
        Transform source = GetSource();
        foreach (MediatorPart p in GetItemList())
        {
            p.DestroyBugPart();
            p.CreateBugPart();
            Transform obj = p.GetBugPart().transform;

            if (source == null || source == obj)
                obj.SetParent(PlayerIdentifier.GetPlayer().transform);
            else
                obj.SetParent(source);
            p.AlignBugPart();
        }
        AlignSource();
    }
}
