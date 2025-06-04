using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject tipController;
    [SerializeField] private Transform partTransform;
    [SerializeField] private Transform selectUILoc;
    [SerializeField] private GameObject selectUIPrefab;
    [SerializeField] private BotBuiltIUIFlipFlop builtBotUI;
    [SerializeField] private GameObject gridRotatePivot;
    [SerializeField] private GameObject inventoryUIObj;
    [SerializeField] private Transform inventoryUILoc;
    [SerializeField] private GameObject inventoryUIPrefab;
    [SerializeField] private GameObject inventoryNoneUIPrefab;
    [SerializeField] private InventoryManager inventory;
    private InventoryItem selectedItem;
    private int axis = 0;
    BotBulider builder;

    private IEnumerator<bool> checkAndBuild;

    private void Awake()
    {
        builder = GetComponent<BotBulider>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject part = GetPart("Necrobug");
        BotPart bPart = part.GetComponent<BotPart>();
        bPart.SetPos(new Vector3(4, 4, 4));

        bPart.GetComponent<InventoryThing>().SetItem(inventory.GetItem(0));

        builder.AddPart(bPart);
        builder.SetSelected(0);
        UpdateAll();
    }
    private void OnEnable()
    {
        UpdateAll();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private GameObject GetPart(string option)
    {
        switch (option)
        {
            case "Necrobug":
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
        //Vector3.Dot(gridRotatePivot.transform.eulerAngles, Vector3.forward);




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


        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        float max = Vector3.Dot(gridRotatePivot.transform.forward, Vector3.forward);

        float tempMax = Vector3.Dot(gridRotatePivot.transform.forward, Vector3.right);
        if (tempMax > max)
        {
            max = tempMax;
            forward = Vector3.left;
            right = Vector3.forward;
            back = Vector3.right;
            left = Vector3.back;
        }

        tempMax = Vector3.Dot(gridRotatePivot.transform.forward, Vector3.back);
        if (tempMax > max)
        {
            max = tempMax;
            forward = Vector3.back;
            right = Vector3.left;
            back = Vector3.forward;
            left = Vector3.right;
        }

        tempMax = Vector3.Dot(gridRotatePivot.transform.forward, Vector3.left);
        if (tempMax > max)
        {
            max = tempMax;
            forward = Vector3.right;
            right = Vector3.back;
            back = Vector3.left;
            left = Vector3.forward;
        }


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
                builder.MoveSelected(selected.GetPos() + left);
                break;
            case Directions.Right:
                selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + right);
                break;
            case Directions.Forward:
                selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + forward);
                break;
            case Directions.Backward:
                selected = builder.GetSelected();
                builder.MoveSelected(selected.GetPos() + back);
                break;

        }
        UpdateAll();
    }

    public void AddPart()
    {
        if (selectedItem.GetName() == "Necrobug")
            return;
        inventory.RemoveItem(selectedItem);
        GameObject part = GetPart(selectedItem.GetName());//partDropdown.captionText.text);
        
        BotPart bPart = part.GetComponent<BotPart>();
        //bPart.SetPos(new Vector3(int.Parse(posX.text), int.Parse(posY.text), int.Parse(posZ.text)));
        bPart.SetPos(new Vector3(0,0,0));
        bPart.GetComponent<InventoryThing>().SetItem(selectedItem);

        builder.AddPart(bPart);
        SelectPart(builder.IndexOf(bPart));

        UpdateAll();
    }
    public void CloseItem()
    {
        inventoryUIObj.SetActive(false);
    }
    public void OpenItem()
    {
        inventoryUIObj.SetActive(true);
        BuildInventoryList();
    }

    public void RemovePart()
    {
        inventory.AddItem(builder.GetSelected().GetComponent<InventoryThing>().GetItem());
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
        int selected = builder.IndexOf(builder.GetSelected());
        //Vector3 rootPos = selectUILoc.GetComponent<RectTransform>().position;
        for (int i = 0; i < builder.GetCount(); i++)
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

            if (selected == i)
            {
                selectableUI.GetComponentInChildren<PartSelectUI>().Select();
            }
            else
            {
                selectableUI.GetComponentInChildren<PartSelectUI>().DeSelect();
            }
        }

    }

    private void BuildInventoryList()
    {
        for (int i = 0; i < inventoryUILoc.childCount; i++)
        {
            Destroy(inventoryUILoc.GetChild(i).gameObject);
        }
        float height = inventoryUIPrefab.GetComponent<RectTransform>().rect.height;
        //int selected = builder.IndexOf(builder.GetSelected());
        //Vector3 rootPos = selectUILoc.GetComponent<RectTransform>().position;

        GameObject noneUI = Instantiate(inventoryNoneUIPrefab, inventoryUILoc);
        noneUI.GetComponent<SelectableInventoryUI>().SetUI(this);

        for (int i = 0; i < inventory.Count(); i++)
        {
            if (i == 0)
                continue;

            Vector3 pos = new Vector3(0, -height * i, 0);
            GameObject selectUITemp = Instantiate(inventoryUIPrefab, inventoryUILoc);
            selectUITemp.GetComponent<RectTransform>().SetLocalPositionAndRotation(pos, Quaternion.identity);
            SelectableInventoryUI selectableUI = selectUITemp.GetComponent<SelectableInventoryUI>();
            selectableUI.SetIndex(i);
            selectableUI.SetPart(inventory.GetItem(i));
            selectableUI.SetUI(this);

            /*
            if (selected == i)
            {
                selectableUI.GetComponentInChildren<PartSelectUI>().Select();
            }
            else
            {
                selectableUI.GetComponentInChildren<PartSelectUI>().DeSelect();
            }
            */
        }
    }
    public void SelectItem(int index)
    {
        selectedItem = inventory.GetItem(index);
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

    private IEnumerator<bool> BuildBot()
    {
        builtBotUI.SetBuildSuccess("processing");
        bool check;
        yield return false;
        yield return check = builder.Check();
        if (!check)
        {
            builtBotUI.SetBuildSuccess("failed");
            yield return true;
        }
        else
        {
            CreateBot();
            builtBotUI.SetBuildSuccess("success");
            yield return true;
        }
    }
    public void UpdateAll()
    {
        BuildList();
        builder.UpdateDisplayCells();
        //builtBotUI.GetComponent<BotBuiltIUIFlipFlop>().SetBuildSuccess(builder.Check());
        //CreateBot();

        if(checkAndBuild != null && !checkAndBuild.Current)
        {
            //Debug.LogError("Reset");
            checkAndBuild.Dispose();
        }
        checkAndBuild = BuildBot();
        StartCoroutine(checkAndBuild);
    }


    public void Rotate()
    {
        //axis = BoundAxis(axis + 1);
        //builder.RotateSelected(AxisToVector(axis));
        builder.ProgressOrientationSelected();
        UpdateAll();
    }
    public void RotateGrid(float rotation)
    {
        gridRotatePivot.transform.Rotate(new Vector3(0, rotation, 0));
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
