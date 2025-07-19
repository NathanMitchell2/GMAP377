using System.Data.Common;
using UnityEngine;

public class BuiltBotStorage : ItemStorage
{
    public static Vector3 sourcePos;
    public static Quaternion sourceRot;
    public override InventoryItem AddItem(ItemMemento item)
    {
        if (item == null || item.GetItemType() != typeof(MediatorPart))
            return null;

        //Transform source = GetSource();

        MediatorPart tItem = (MediatorPart)gameObject.AddComponent(item.GetItemType());
        tItem.RestoreMemento(item);

        if (!tItem.HasBugPart())
        {
            tItem.CreateBugPart();
        }    

        AlignAll();

        return tItem;
    }
    public Transform GetSource()
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

        source.SetPositionAndRotation(sourcePos, sourceRot);
        source.SetParent(PlayerIdentifier.GetPlayer().transform);
        source.localScale = new Vector3(.8f,.8f,.8f);
    }

    public void AlignAll()
    {
        foreach (MediatorPart p in GetItemList())
        {
            p.AlignBugPart();
        }

        Transform source = GetSource();


        foreach (MediatorPart p in GetItemList())
        {
            //p.CreateBugPart();
            Transform obj = p.GetBugPart().transform;
            //Vector3 sourceBotPos = source.GetComponent<ModularBugPart>().GetMediator().GetBotPos();
            //obj.SetLocalPositionAndRotation(obj.localPosition + sourcePos + sourceBotPos, sourceRot * obj.localRotation);
            //obj.SetLocalPositionAndRotation(obj.localPosition + sourcePos + sourceBotPos, sourceRot * obj.localRotation);
            //Debug.LogError("Bug Pre Part at " + obj.transform.localPosition + " and " + obj.transform.localRotation);

            if (source == null || source == obj)
                continue;// obj.SetParent(PlayerIdentifier.GetPlayer().transform);
            else
                obj.SetParent(source);
            //Debug.LogError("Bug Post Part at " + obj.transform.localPosition + " and " + obj.transform.localRotation);
            //p.AlignBugPart();

            //source = GetSource();
        }
        AlignSource();
    }
}
