using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class MediatorPart : InventoryItem
{
    [SerializeField] GameObject BotPartPrefab;
    [SerializeField] GameObject BugPartPrefab;
    private GameObject botPart;
    private GameObject bugPart;

    private InputAction action;

    public void CreateBotPart()
    {
        botPart = Instantiate(BotPartPrefab);
        AddAction(botPart.GetComponent<BotPart>().defaultBind);
        GetBotPart().Initialize(this);
    }

    public void CreateBugPart()
    {
        GameObject player = PlayerIdentifier.GetPlayer().gameObject;

        BotPart bPart = GetBotPart();
        Transform storage = bPart.GetPartStorage();
        //that transform is a paramter btw
        Vector3 pos = bPart.GetPos() + storage.localPosition;
        Quaternion rot = storage.localRotation * transform.rotation;

        bugPart = Instantiate(BugPartPrefab, pos, rot);

        GetBugPart().Initialize(this);
        ModularBugPart part = GetBugPart();
        part.Initialize(this);

        PlayerHealth partHealth = part.GetComponent<PlayerHealth>();
        if (partHealth != null)
        {
            partHealth.Initialize(this);
        }

        InputStrategy strat = part.GetComponent<InputStrategy>();
        if (strat != null)
        {
            player.GetComponent<InputManager>().SetStrat(strat);
        }

        //need to figure this
        //part.GetComponent<Rigidbody>().centerOfMass = car.GetComponent<Rigidbody>().centerOfMass;
    }

    public void DestroyBotPart()
    {
        GameObject player = PlayerIdentifier.GetPlayer().gameObject;
        BotBulider builder = player.GetComponent<BotBulider>();

        int index = builder.IndexOf(this);

        builder.RemovePart(index);

        Destroy(botPart);
    }
    public void DestroyBugPart()
    {
        GameObject player = PlayerIdentifier.GetPlayer().gameObject;
        BotBulider builder = player.GetComponent<BotBulider>();

        //GetBugPart().CleanUp();
        RemoveAction();

        builder.RemoveBuiltPart(builder.BuiltIndexOf(this));

        Destroy(bugPart);
    }

    public BotPart GetBotPart()
    {
        if(botPart == null)
        {
            botPart = Instantiate(BotPartPrefab);
            Debug.LogError("MediatorPart " + this + " had to make a bot part");
        }
        return botPart.GetComponent<BotPart>();
    }
    public ModularBugPart GetBugPart()
    {
        if (bugPart == null)
        {
            bugPart = Instantiate(BugPartPrefab);
            Debug.LogError("MediatorPart " + this + " had to make a bug part");
        }
        return bugPart.GetComponent<ModularBugPart>();
    }

    public void DestroyPart()
    {
        DestroyBotPart();
        DestroyBugPart();
        Destroy(gameObject);
    }

    public void AddAction(string defaultBind)
    {
        action = new InputAction();
        action.AddBinding(defaultBind);// "<Keyboard>/1");//tag AddCompositeBinding("ButtonWithTwoModifiers").With("Button", "<Keyboard>/1");

        action.Enable();
    }

    public void RemoveAction()
    {
        action.RemoveAction();
    }

    public void RebindAction()
    {
        action.Disable();
        action.PerformInteractiveRebinding(0)
            //.WithControlsExcluding("Mouse")
            .Start()
            .OnComplete(context =>
            {
                context.action.Enable();
            });
        //Debug.LogError(actions[index].bindings[0].ToString());
    }

    public void BindParts(List<GameObject> parts)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            action.performed += (context) =>
            {
                GetBugPart().Activate();
            };
        }
    }
    public string GetKeybindText()
    {
        if (action == null || !action.enabled)
            return "";

        return action.bindings[0].ToDisplayString();
    }

}
