using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;
public class MediatorMemento : ItemMemento
{
    private string itemName;
    private Sprite icon;
    private float health;
    private float maxHealth;
    private bool stackable;
    private int maxStack;
    private int count;
    private GameObject botPrefab;
    private GameObject bugPrefab;
    private BotPartMemento botPart;
    private BugPartMemento bugPart;
    private bool customBinds;
    private string defaultBinding;
    private string binding;
    public MediatorMemento(string itemName, Sprite icon, float health, float maxHealth, bool stackable, int maxStack, int count, GameObject botPrefab, GameObject bugPrefab, BotPartMemento botPart, BugPartMemento bugPart, bool customBinds, string defaultBinding, string binding) : base(itemName, icon, health, maxHealth, stackable, maxStack, count)
    {
        this.itemName = itemName;
        this.icon = icon;
        this.health = health == -1 ? maxHealth : health;
        this.maxHealth = maxHealth;
        this.stackable = stackable;
        this.maxStack = maxStack;
        this.count = count;
        this.botPrefab = botPrefab;
        this.bugPrefab = bugPrefab;
        this.botPart = botPart;
        this.bugPart = bugPart;
        this.customBinds = customBinds;
        this.defaultBinding = defaultBinding;
        this.binding = binding;
    }
    public override System.Type GetItemType()
    {
        return typeof(MediatorPart);
    }
    /*
    public string GetName()
    {
        return itemName;
    }

    public Sprite GetIcon()
    {
        return icon;
    }
    public int GetCount()
    {
        return count;
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public bool GetStackable() { return this.stackable; }

    public int GetMaxStack() { return maxStack; }*/

    public GameObject GetBotPrefab() { return botPrefab; }
    public GameObject GetBugPrefab() { return bugPrefab; }
    public BotPartMemento GetBotPart() { return botPart; }
    public BugPartMemento GetBugPart() { return bugPart; }
    public bool GetCustomBinds() { return customBinds; }
    public String GetDefaultBinding() { return defaultBinding; }
    public String GetBinding() { return binding; }
}

public class MediatorPart : InventoryItem
{
    public static Transform BotPartTransform;

    [SerializeField] private GameObject BotPartPrefab;
    [SerializeField] private GameObject BugPartPrefab;
    [SerializeField] private bool customBinds = true;
    [SerializeField] private string defaultBind = "";
    private GameObject botPart;
    private GameObject bugPart;

    private InputAction action;
    private string binding;

    private Vector3 botPos;
    private Vector3 storagePos;
    private Quaternion storageRot;

    private bool destroying = false;

    //private bool isPrefabFlag = true;


    private void Awake()
    {
        //isPrefabFlag = false;
        binding = defaultBind;
        AddAction();
    }
    public override ItemMemento CreateMemento()
    {
        //if (isPrefabFlag)
        //    binding = defaultBind;
        if(destroying) return null;
        BotPartMemento botPart = HasBotPart() ? GetBotPart().CreateMemento() : null;
        BugPartMemento bugPart = HasBugPart() ? GetBugPart().CreateMemento() : null;
        return new MediatorMemento(itemName, icon, health, maxHealth, stackable, maxStack, count, BotPartPrefab, BugPartPrefab, botPart, bugPart, customBinds, defaultBind, binding);
    }
    public override void RestoreMemento(ItemMemento item)
    {
        RestoreMemento(item, true);
    }
    public void RestoreMemento(ItemMemento item, bool doDelay)
    {
        SuperItemRestore(item);
        try
        {
            MediatorMemento med = (MediatorMemento)item;
            BotPartPrefab = med.GetBotPrefab();
            BugPartPrefab = med.GetBugPrefab();
            if (med.GetBotPart() != null)
            {
                if (!HasBotPart())
                    CreateBotPart();
                GetBotPart().RestoreMemento(med.GetBotPart(), doDelay);
                //GameObject player = PlayerIdentifier.GetPlayer().gameObject;
                //BotBulider builder = player.GetComponentInChildren<BotBulider>();
                //builder.AddPart(GetBotPart());

            }
            if (med.GetBugPart() != null)
            {
                if (!HasBugPart())
                    CreateBugPart();
                GetBugPart().RestoreMemento(med.GetBugPart());
            }
            customBinds = med.GetCustomBinds();
            action.ApplyBindingOverride(0, med.GetBinding());
            defaultBind = med.GetDefaultBinding();
            binding = med.GetBinding();
            AlignBugPart();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }
    }
    public Vector3 GetBotPos()
    {
        if (!HasBotPart())
            if (botPos != null)
                return botPos;
            else return new Vector3(0,0,0);

        botPos = GetBotPart().GetPos();
        return botPos;
    }
    private Vector3 GetStoragePos()
    {
        if (!HasBotPart())
            if (storagePos != null)
                return storagePos;
            else return new Vector3(0,0,0);

        storagePos = GetBotPart().GetPartStorage().localPosition;
        return storagePos;
    }
    private Quaternion GetStorageRot()
    {
        if (!HasBotPart())
            if (storageRot != null)
                return storageRot;
            else return Quaternion.identity;

        storageRot = GetBotPart().GetPartStorage().localRotation;
        return storageRot;
    }
    public bool HasBotPart() { return botPart != null; }
    public bool HasBugPart() { return bugPart != null; }
    public void CreateBotPart()
    {
        if (botPart != null)
            DestroyBotPart();

        botPart = Instantiate(BotPartPrefab, BotPartTransform);
        //AddAction(botPart.GetComponent<BotPart>().defaultBind);
        GetBotPart().Initialize(this);
    }

    public void CreateBugPart()
    {
        if(bugPart != null)
            DestroyBugPart();

        bugPart = Instantiate(BugPartPrefab);
        AlignBugPart();
    }

    public void DestroyBotPart()
    {
        GameObject player = PlayerIdentifier.GetPlayer().gameObject;
        BotBulider builder = player.GetComponentInChildren<BotBulider>();

        if(HasBotPart())
        {
            int index = builder.IndexOf(GetBotPart());

            //if (index != -1)
            //builder.RemovePart(index);

            //GameObject temp = botPart;
            //botPart = null;
            Destroy(botPart);
        }    
    }
    public void DestroyBugPart()
    {
        if (!HasBugPart())
            return;
        GameObject player = PlayerIdentifier.GetPlayer().gameObject;
        BotBulider builder = player.GetComponentInChildren<BotBulider>();

        //GetBugPart().CleanUp();

        //builder.RemoveItem(this);
        //builder.RemoveBuiltPart(builder.BuiltIndexOf(this));
        //GameObject temp = bugPart;
        //bugPart = null;
        Destroy(bugPart);
    }

    public BotPart GetBotPart()
    {
        if(!HasBotPart())
        {
            botPart = Instantiate(BotPartPrefab);
            botPart.GetComponent<BotPart>().Initialize(this);
            Debug.LogError("MediatorPart " + this + " had to make a bot part");
        }
        return botPart.GetComponent<BotPart>();
    }
    public ModularBugPart GetBugPart()
    {
        if (!HasBugPart())
        {
            bugPart = Instantiate(BugPartPrefab);
            bugPart.GetComponent<ModularBugPart>().Initialize(this);
            Debug.LogError("MediatorPart " + this + " had to make a bug part");
        }
        return bugPart.GetComponent<ModularBugPart>();
    }

    public void LinkBotPart(BotPart part)
    {
        DestroyBotPart();
        botPart = part.gameObject;
        part.Initialize(this);
    }
    public void LinkBugPart(ModularBugPart part)
    {
        DestroyBugPart();
        bugPart = part.gameObject;
        part.Initialize(this);
    }
    public void AlignBugPart()
    {
        if (HasBugPart())
        {
            //that transform is a paramter btw
            //Transform parent = PlayerIdentifier.GetPlayer().transform;
            Vector3 pos = GetBotPos() + GetStoragePos();
            Quaternion rot = GetStorageRot();// * parent.rotation;

            //Debug.LogError("Align Bug Part to " + pos + " and " + rot);
            bugPart.transform.SetParent(null);
            bugPart.transform.SetLocalPositionAndRotation(pos, rot);
            bugPart.transform.localScale = BugPartPrefab.transform.localScale;


            ModularBugPart part = GetBugPart();
            //Debug.LogError("action binding");
            //Debug.LogError(action.bindings[0].effectivePath);
            //Debug.LogError(action.bindings[0].path);
            //Debug.LogError(action.bindings[0].overridePath);
            //Debug.LogError(binding);
            //Debug.LogError("End action binding");
            part.Initialize(this);
            BindPart();

            
            PlayerHealth partHealth = part.GetComponent<PlayerHealth>();
            if (partHealth != null)
            {
                partHealth.Initialize(this);
            }
            
            InputStrategy strat = part.GetComponent<InputStrategy>();
            if (strat != null)
            {
                PlayerIdentifier.GetPlayer().GetComponent<InputManager>().SetStrat(strat);
            }
            
        }
    }
    private void Update()
    {
        GetBotPos();
        GetStoragePos();
        GetStorageRot();
    }
    public void OnDestroy()
    {
        Debug.Log("MediatorPart OnDestroy, auto cleanup may have unexpected behavior");
        DestroyItem();
    }
    public override void DestroyItem()
    {
        DestroyBotPart();
        DestroyBugPart();
        RemoveAction();
        Destroy(this);
        destroying = true;
    }

    public override void CleanToBaseClass()
    {
        DestroyBotPart();
        DestroyBugPart();
        binding = defaultBind;
    }
    public void AddAction()
    {
        //Debug.Log("Added");
        action = new InputAction();
        action.AddBinding(defaultBind);// "<Keyboard>/1");//tag AddCompositeBinding("ButtonWithTwoModifiers").With("Button", "<Keyboard>/1");
        //action.Enable();
    }

    public void RemoveAction()
    {
        //Debug.Log("Removed");
        action.Dispose();
        if(HasBugPart())
        {
            //GetBugPart().Dispose();
        }
    }

    public void RebindAction()
    {
        //Debug.Log("Rebind");
        action.Disable();
        action.PerformInteractiveRebinding(0)
            //.WithControlsExcluding("Mouse")
            .Start()
            .OnComplete(context =>
            {
                //context.action.ApplyBindingOverride(context.action.bindings[0].path);
                //Debug.LogError(context.action.bindings[0].effectivePath);
                //Debug.LogError(context.action.bindings[0].path);
                //Debug.LogError(context.action.bindings[0].overridePath);
                context.action.ApplyBindingOverride(context.action.bindings[0].effectivePath);
                action.ApplyBindingOverride(context.action.bindings[0].effectivePath);
                //Debug.LogError(context.action.ApplyBindingOverride)
                action = context.action;
                SetBinding(context.action.bindings[0].effectivePath);
                context.action.Enable();
            });
        //Debug.LogError(actions[index].bindings[0].ToString());
    }
    public void SetBinding(string binding)
    {
        this.binding = binding;
    }
    public void BindPart()
    {
        //Debug.Log("Bound");
        action.ApplyBindingOverride(0, binding);
        action.performed += (context) =>
        {
            //Debug.LogError("Performed");
            //Debug.LogError(context.action.bindings[0].effectivePath);
            GetBugPart().Activate();
        };
        action.Enable();
    }
    public string GetKeybindText()
    {
        InputAction temp = new InputAction();
        temp.AddBinding(binding);

        string output = temp.bindings[0].ToDisplayString();

        temp.Dispose();

        return output;
    }
    public string GetPath()
    {
        return action.bindings[0].effectivePath;
    }
    public bool GetCustomBinds()
    {
        return customBinds;
    }

}
