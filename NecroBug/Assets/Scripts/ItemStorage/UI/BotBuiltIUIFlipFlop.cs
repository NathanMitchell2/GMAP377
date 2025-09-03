using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotBuiltIUIFlipFlop : MonoBehaviour
{
    [SerializeField] string successText;
    [SerializeField] string processingText;
    [SerializeField] string failText;
    [SerializeField] Sprite successImg;
    [SerializeField] Sprite processingImg;
    [SerializeField] Sprite failImg;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //img = GetComponent<Image>();
        //text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetBuildSuccess(string success)
    {
        /*
        if(success == "processing")
        {
            text.text = processingText;
            img.sprite = processingImg;
        }
        else if(success == "success")
        {
            text.text = successText;
            img.sprite = successImg;
        }
        else
        {
            text.text = failText;
            img.sprite = failImg;
        }
        */
    }
}
