using TMPro;
using UnityEngine;

public class DynamicUISetter : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void SetFloat(float value)
    {
        text.text = value.ToString();
    }
}
