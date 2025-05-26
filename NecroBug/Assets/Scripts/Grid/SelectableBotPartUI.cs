using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectableBotPartUI : MonoBehaviour
{
    [SerializeField] private GameObject key;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private Image img;
    private int index;
    private BuilderUI ui;

    private void Update()
    {
        SetKey(ui.GetKeybindText(index));
    }

    public void SetKey(string text)
    {
        keyText.text = text;
    }
    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetPart(InventoryThing inventoryThing)
    {
        text.text = inventoryThing.GetName();
        if (inventoryThing.GetIcon() != null)
        {
            img.sprite = inventoryThing.GetIcon();
        }
        else
        {
            img.color = Color.clear;
        }

        }
    public void SetUI(BuilderUI builderUI)
    {
        this.ui = builderUI;
    }

    public void Select()
    {
        ui.SelectPart(index);
    }
    public void Bind()
    {
        ui.BindPart(index);
    }

    public void UsesCustomBinds(bool useCustomBinds)
    {
        key.SetActive(useCustomBinds);
    }

    public void DebugClick()
    {
    }    


}