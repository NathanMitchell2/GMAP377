using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private List<InputAction> actions = new List<InputAction>();
    private List<ModularBugPart> bugParts = new List<ModularBugPart>();
    private List<InputActionRebindingExtensions.RebindingOperation> rebinds = new List<InputActionRebindingExtensions.RebindingOperation>();

    public void AddAction(string defaultBind)
    {
        InputAction action = new InputAction();
        action.AddBinding(defaultBind);// "<Keyboard>/1");//tag AddCompositeBinding("ButtonWithTwoModifiers").With("Button", "<Keyboard>/1");

        action.Enable();
        actions.Add(action);
    }

    public void RemoveAction(int index)
    {
        actions.RemoveAt(index);
    }

    public void RebindAction(int index)
    {
        actions[index].Disable();
        actions[index].PerformInteractiveRebinding(0)
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
        this.bugParts.Clear();
        for (int i = 0; i < parts.Count; i++)
        {
            this.bugParts.Add(parts[i].GetComponent<ModularBugPart>());
            actions[i].performed += (context) => 
            {
                GetPartByAction(context.action).Activate();
            };
        }
    }

    public ModularBugPart GetPartByAction(InputAction action)
    {
        int index = actions.IndexOf(action);
        return bugParts[index];
    }

    public string GetKeybindText(int index)
    {
        InputAction action = actions[index];

        if (action == null || !action.enabled)
            return "";

        return action.bindings[0].ToDisplayString();

    }
}
