using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image img;

    public void SetItem(ItemMemento item)
    {
        text.text = item.GetName();
        if (item.GetStackable())
            countText.text = item.GetCount().ToString();
        else
            countText.text = "";

        if (item.GetIcon() != null)
        {
            img.sprite = item.GetIcon();
        }
        else
        {
            img.color = Color.clear;
        }
    }
}
