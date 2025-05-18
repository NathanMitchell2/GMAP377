using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    [SerializeField] private GameObject tipController;
    [SerializeField] private Transform partTransform;
    [SerializeField] private Transform selectUILoc;
    [SerializeField] private GameObject selectUIPrefab;
    private int axis = 0;
    BotBulider builder;


    private void Awake()
    {
        builder = GetComponent<BotBulider>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject part = GetPart("NecroBug");
        BotPart bPart = part.GetComponent<BotPart>();
        bPart.SetPos(new Vector3(4, 4, 4));
        builder.AddPart(bPart);
        builder.SetSelected(0);
        UpdateAll();

        ScrollRect scroll;
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
                return Instantiate(parts[0], partTransform);
            case "Horn":
                return Instantiate(parts[1], partTransform);
            case "Jet":
                return Instantiate(parts[2], partTransform);
            case "Wing":
                return Instantiate(parts[3], partTransform);
            case "Generic":
                return Instantiate(parts[4], partTransform);
            case "Legs":
                return Instantiate(parts[5], partTransform);
            case "Leg":
                return Instantiate(parts[6], partTransform);
            default:
                return null;
        }
    }

    public void SelectPart()
    {
        builder.SetSelected(int.Parse(selectIndex.text));
        UpdateAll();
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
        UpdateAll();
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
        UpdateAll();
    }

    public void AddPart()
    {
        GameObject part = GetPart(partDropdown.captionText.text);
        
        BotPart bPart = part.GetComponent<BotPart>();
        //bPart.SetPos(new Vector3(int.Parse(posX.text), int.Parse(posY.text), int.Parse(posZ.text)));
        bPart.SetPos(new Vector3(0,0,0));
        builder.AddPart(bPart);
        UpdateAll();
    }

    public void RemovePart()
    {
        builder.RemoveSelected();
        UpdateAll();
    }

    public void CreateBot()
    {
        builder.CreateBot();
    }

    private void BuildList()
    {
        for (int i = 0; i < selectUILoc.childCount; i++)
        {
            Destroy(selectUILoc.GetChild(i).gameObject);
        }
        float height = selectUIPrefab.GetComponent<RectTransform>().rect.height;
        //Vector3 rootPos = selectUILoc.GetComponent<RectTransform>().position;
        for(int i = 0; i < builder.GetCount(); i++)
        {
            Vector3 pos = new Vector3(0, -height*i, 0);
            GameObject selectUITemp = Instantiate(selectUIPrefab, selectUILoc);
            selectUITemp.GetComponent<RectTransform>().SetLocalPositionAndRotation(pos, Quaternion.identity);
            SelectableBotPartUI selectableUI = selectUITemp.GetComponent<SelectableBotPartUI>();
            selectableUI.SetIndex(i);
            selectableUI.SetPart(builder.GetIndex(i).GetInventoryPart());
            selectableUI.SetUI(this);
            selectableUI.UsesCustomBinds(builder.GetIndex(i).customBinds);
            selectableUI.SetKey(GetKeybindText(i));
        }
    }
    public void SelectPart(int index)
    {
        builder.SetSelected(index);
        UpdateAll();
    }
    public void BindPart(int index)
    {
        builder.BindAction(index);
        UpdateAll();
    }

    public string GetKeybindText(int index)
    {
        return builder.GetKeybindText(index);
    }

    public void UpdateAll()
    {
        BuildList();
        builder.UpdateDisplayCells();
    }

    public void Rotate()
    {
        //axis = BoundAxis(axis + 1);
        //builder.RotateSelected(AxisToVector(axis));
        builder.ProgressOrientationSelected();
        UpdateAll();
    
    }

    private int BoundAxis(int axis)
    {
        if (axis < 0)
            return 5;
        else if (axis > 5)
            return 0;
        return axis;
    }

    private Vector3 AxisToVector(int axis)
    {

        switch ((Directions)axis)
        {
            case Directions.Up:
                return Vector3.up;
            case Directions.Down:
                return Vector3.down;
            case Directions.Left:
                return Vector3.left;
            case Directions.Right:
                return Vector3.right;
            case Directions.Forward:
                return Vector3.forward;
            case Directions.Backward:
                return Vector3.back;
            default:
                return Vector3.right;

        }
    }
}
