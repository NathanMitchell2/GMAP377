using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown partDropdown;
    [SerializeField] private TMP_InputField posX;
    [SerializeField] private TMP_InputField posY;
    [SerializeField] private TMP_InputField posZ;
    [SerializeField] private TMP_InputField removeIndex;
    [SerializeField] private List<GameObject> parts = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private GameObject GetPart(string option)
    {
        Debug.Log(option);
        switch (option)
        {
            case "NecroBug":
                return Instantiate(parts[0]);
            case "Horn":
                return Instantiate(parts[1]);
            case "Jet":
                return Instantiate(parts[2]);
            default:
                return null;
        }
    }

    public void AddPart()
    {
        BotBulider builder = GetComponent<BotBulider>();
        GameObject part = GetPart(partDropdown.captionText.text);
        BotPart bPart = part.GetComponent<BotPart>();
        bPart.SetPos(new Vector3(int.Parse(posX.text), int.Parse(posY.text), int.Parse(posZ.text)));
        builder.AddPart(bPart);
    }

    public void RemovePart()
    {
        BotBulider builder = GetComponent<BotBulider>();
        builder.RemovePart(int.Parse(removeIndex.text));
    }
}
