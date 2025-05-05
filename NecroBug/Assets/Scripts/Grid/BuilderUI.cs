using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour
{
    public enum Directions
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Backward
    }

    [SerializeField] private TMP_Dropdown partDropdown;
    [SerializeField] private TMP_InputField posX;
    [SerializeField] private TMP_InputField posY;
    [SerializeField] private TMP_InputField posZ;
    [SerializeField] private TMP_InputField removeIndex;
    [SerializeField] private TMP_InputField selectIndex;
    [SerializeField] private TMP_InputField rotationEnum;
    [SerializeField] private List<GameObject> parts = new List<GameObject>();
    BotBulider builder;


    private void Awake()
    {
        builder = GetComponent<BotBulider>();
    }
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
        switch (option)
        {
            case "NecroBug":
                return Instantiate(parts[0],transform);
            case "Horn":
                return Instantiate(parts[1], transform);
            case "Jet":
                return Instantiate(parts[2], transform);
            default:
                return null;
        }
    }

    public void SelectPart()
    {
        builder.SetSelected(int.Parse(selectIndex.text));
        builder.UpdateDisplayCells();
    }

    public void RotateSelected()
    {
        int dir = int.Parse(rotationEnum.text);

        switch ((Directions)dir)
        {
            case Directions.Up:
                BotPart selected = builder.GetSelected();
                builder.RotateSelected(Vector3.up);
                break;
            case Directions.Down:
                selected = builder.GetSelected();
                builder.RotateSelected(Vector3.down);
                break;
            case Directions.Left:
                selected = builder.GetSelected();
                builder.RotateSelected(Vector3.left);
                break;
            case Directions.Right:
                selected = builder.GetSelected();
                builder.RotateSelected(Vector3.right);
                break;
            case Directions.Forward:
                selected = builder.GetSelected();
                builder.RotateSelected(Vector3.forward);
                break;
            case Directions.Backward:
                selected = builder.GetSelected();
                builder.RotateSelected(Vector3.back);
                break;

        }
        builder.UpdateDisplayCells();
    }
    public void MoveSelected(int dir)
    {
        //BotPart selected = builder.GetSelected();

        switch ((Directions)dir)
        {
            case Directions.Up:
                BotPart selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + Vector3.up);
                break;
            case Directions.Down:
                selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + Vector3.down);
                break;
            case Directions.Left:
                selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + Vector3.left);
                break;
            case Directions.Right:
                selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + Vector3.right);
                break;
            case Directions.Forward:
                selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + Vector3.forward);
                break;
            case Directions.Backward:
                selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + Vector3.back);
                break;

        }
        builder.UpdateDisplayCells();
    }

    public void AddPart()
    {
        GameObject part = GetPart(partDropdown.captionText.text);
        BotPart bPart = part.GetComponent<BotPart>();
        bPart.SetPos(new Vector3(int.Parse(posX.text), int.Parse(posY.text), int.Parse(posZ.text)));
        builder.AddPart(bPart);
        builder.UpdateDisplayCells();
    }

    public void RemovePart()
    {
        builder.RemovePart(int.Parse(removeIndex.text));
        builder.UpdateDisplayCells();
    }
}
