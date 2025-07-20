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

    //[SerializeField] private TMP_Dropdown partDropdown;
    //[SerializeField] private TMP_InputField posX;
    //[SerializeField] private TMP_InputField posY;
    //[SerializeField] private TMP_InputField posZ;
    //[SerializeField] private TMP_InputField removeIndex;
    //[SerializeField] private TMP_InputField selectIndex;
    //[SerializeField] private TMP_InputField rotationEnum;
    //[SerializeField] private List<GameObject> parts = new List<GameObject>();
    //[SerializeField] private GameObject tipController;
    //[SerializeField] private Transform partTransform;
    [SerializeField] private Transform selectUILoc;
    [SerializeField] private GameObject selectUIPrefab;
    [SerializeField] private BotBuiltIUIFlipFlop builtBotUI;
    [SerializeField] private GameObject gridRotatePivot;
    [SerializeField] private GameObject inventoryUIObj;
    [SerializeField] private Transform inventoryUILoc;
    [SerializeField] private GameObject inventoryUIPrefab;
    //[SerializeField] private GameObject inventoryNoneUIPrefab;
    //[SerializeField] private InventoryManager inventory;
    private InventoryItem selectedItem;
    //private int axis = 0;
    BotBulider builder;
    private StorageManager storageManager;

    private int selectedIndex;

    private IEnumerator<bool> checkAndBuild;


    private GridDisplayCell[,,] gridDisplayCells;
    [SerializeField] GameObject cell;
    [SerializeField] Transform gridTransform;

    private bool lateUpdate = false;
    private void Awake()
    {
        GameObject player = PlayerIdentifier.GetPlayer().gameObject;
        builder = player.GetComponentInChildren<BotBulider>();
        storageManager = player.GetComponent<StorageManager>();

        Vector3 size = builder.GetSize();
        gridDisplayCells = new GridDisplayCell[(int)size.x, (int)size.y, (int)size.z];
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    GameObject newCell = Instantiate(cell, gridTransform);
                    newCell.transform.SetLocalPositionAndRotation(new Vector3(i, j, k), new Quaternion());
                    //Debug.Log(newCell==null);
                    gridDisplayCells[i, j, k] = newCell.GetComponent<GridDisplayCell>();
                }
            }
        }
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateAll();
    }
    private void OnEnable()
    {
        UpdateAll();
    }
    private void LateUpdate()
    {
        if (lateUpdate)
        {
            UpdateAll();
            lateUpdate = false;
        }    
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

        Vector3 ogPos = builder.GetPart(selectedIndex).GetPos();
        switch ((Directions)dir)
        {
            case Directions.Up:
                builder.MovePart(selectedIndex, ogPos + Vector3.up);
                break;
            case Directions.Down:
                builder.MovePart(selectedIndex, ogPos + Vector3.down);
                break;
            case Directions.Left:
                builder.MovePart(selectedIndex, ogPos + left);
                break;
            case Directions.Right:
                builder.MovePart(selectedIndex, ogPos + right);
                break;
            case Directions.Forward:
                builder.MovePart(selectedIndex, ogPos + forward);
                break;
            case Directions.Backward:
                builder.MovePart(selectedIndex, ogPos + back);
                break;

        }
        UpdateAll();
    }

    private List<InventoryItem> GetInventoryItems()
    {
        return storageManager.GetItemList(StorageManager.StorageKey.Inventory);
    }

    public void AddPart()
    {
        if (selectedItem.GetName() == "Necrobug")
            return;
        MediatorPart part = (MediatorPart)selectedItem;//(MediatorPart)GetInventoryItems().(selectedItem);


        MediatorPart nPart = (MediatorPart)storageManager.Transfer(part, StorageManager.StorageKey.Inventory, StorageManager.StorageKey.BotBuilder);
        selectedIndex = builder.IndexOf(nPart.GetBotPart());

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
        //inventory.AddItem(builder.GetGridParts()[selectedIndex]);
        builder.RemovePart(selectedIndex);
        UpdateAll();
        lateUpdate = true;

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
        for (int i = 0; i < builder.GetPartCount(); i++)
        {
            Vector3 pos = new Vector3(0, -height*i, 0);
            GameObject selectUITemp = Instantiate(selectUIPrefab, selectUILoc);
            selectUITemp.GetComponent<RectTransform>().SetLocalPositionAndRotation(pos, Quaternion.identity);
            SelectableBotPartUI selectableUI = selectUITemp.GetComponent<SelectableBotPartUI>();
            selectableUI.SetIndex(i);

            //selectableUI.SetPart(builder.GetIndex(i).GetInventoryPart());
            selectableUI.SetPart(builder.GetPart(i).GetMediator());

            selectableUI.SetUI(this);

            //selectableUI.UsesCustomBinds(builder.GetIndex(i).customBinds);
            MediatorPart part = builder.GetPart(i).GetMediator();
            selectableUI.UsesCustomBinds(part.GetCustomBinds());

            selectableUI.SetKey(part.GetKeybindText());

            if (selectedIndex == i)
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

        //GameObject noneUI = Instantiate(inventoryNoneUIPrefab, inventoryUILoc);
        //noneUI.GetComponent<SelectableInventoryUI>().SetUI(this);

        List<InventoryItem> inventory = ((InventoryManager)storageManager.GetStorage(StorageManager.StorageKey.Inventory)).FilterInUse();

        for (int i = 0; i < inventory.Count; i++)
        {
            if (i == 0) ;
                //continue;

            Vector3 pos = new Vector3(0, -height * (i), 0);
            GameObject selectUITemp = Instantiate(inventoryUIPrefab, inventoryUILoc);
            selectUITemp.GetComponent<RectTransform>().SetLocalPositionAndRotation(pos, Quaternion.identity);
            SelectableInventoryUI selectableUI = selectUITemp.GetComponent<SelectableInventoryUI>();
            selectableUI.SetIndex(i);
            selectableUI.SetPart(inventory[i]);
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
        selectedItem = GetInventoryItems()[index];
    }
    public void SelectPart(int index)
    {
        selectedIndex = index;
        UpdateAll();
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
        UpdateDisplayCells();

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
    
    public void UpdateDisplayCells()
    {
        Vector3 size = builder.GetSize();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                for (int k = 0; k < size.z; k++)
                {
                    gridDisplayCells[i, j, k].ProcessStack(builder.GetCell(i,j,k));
                }
            }
        }
    }
    


    public void Rotate()
    {
        builder.ProgressOrientation(selectedIndex);
        UpdateAll();
    }
    public void RotateGrid(float rotation)
    {
        gridRotatePivot.transform.Rotate(new Vector3(0, rotation, 0));
    }
    public void RotateGrid(Vector3 rotation)
    {
        gridRotatePivot.transform.Rotate(rotation);
    }

    /*
    public Transform GetGridTransform()
    {
        return partTransform;
    }
    */
}
