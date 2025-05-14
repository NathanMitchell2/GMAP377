using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectableBotPartUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image img;
    private int index;
    private BuilderUI ui;

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetPart(InventoryThing inventoryThing)
    {
        text.text = inventoryThing.GetName();
        img.sprite = inventoryThing.GetIcon();
    }
    public void SetUI(BuilderUI builderUI)
    {
        this.ui = builderUI;
    }

    public void Select()
    {
        ui.SelectPart(index);
    }

    public void DebugClick()
    {
    }    


}