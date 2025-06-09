using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectableInventoryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image img;
    [SerializeField] private Image healthBar;
    private int index;
    private BuilderUI ui;

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetPart(InventoryItem inventoryThing)
    {
        text.text = inventoryThing.GetName();
        if (inventoryThing.GetStackable())
            countText.text = inventoryThing.GetCount().ToString();
        else
            countText.text = "";

        if (inventoryThing.GetIcon() != null)
        {
            img.sprite = inventoryThing.GetIcon();
        }
        else
        {
            img.color = Color.clear;
        }

        healthBar.fillAmount = (float)inventoryThing.GetHealth() / inventoryThing.GetMaxHealth();

    }
    public void SetUI(BuilderUI builderUI)
    {
        this.ui = builderUI;
    }

    public void Select()
    {
        ui.SelectItem(index);
    }

    public void Add()
    {
        ui.AddPart();
    }

    public void Close()
    {
        ui.CloseItem();
    }
}