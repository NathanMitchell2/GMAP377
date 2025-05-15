using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private List<InputAction> actions = new List<InputAction>();
    private List<ModularBugPart> bugParts = new List<ModularBugPart>();
    private List<InputActionRebindingExtensions.RebindingOperation> rebinds = new List<InputActionRebindingExtensions.RebindingOperation>();

    public void AddAction()
    {
        Debug.Log("AddIn");
        InputAction action = new InputAction();
        action.AddBinding("<Keyboard>/1");//tag AddCompositeBinding("ButtonWithTwoModifiers").With("Button", "<Keyboard>/1");

        action.Enable();
        actions.Add(action);
        Debug.Log("AddOut");
    }

    public void RemoveAction(int index)
    {
        Debug.Log("RemoveIn");
        actions.RemoveAt(index);
        Debug.Log("RemoveOut");
    }

    public void RebindAction(int index)
    {
        Debug.Log("RebindIn");
        actions[index].Disable();
        actions[index].PerformInteractiveRebinding(0)
            .WithControlsExcluding("Mouse")
            .Start()
            .OnComplete(context => 
            {
                Debug.Log("Complete");
                context.action.Enable();
                Debug.Log(context.action.bindings[0]); 
            });
        Debug.Log("RebindOut");
    }

    public void BindParts(List<GameObject> parts)
    {
        Debug.Log("BindIn");
        this.bugParts.Clear();
        for (int i = 0; i < parts.Count; i++)
        {
            this.bugParts.Add(parts[i].GetComponent<ModularBugPart>());
            actions[i].performed += (context) => 
            {
                Debug.Log("BoundIn");
                GetPartByAction(context.action).Activate();
                Debug.Log("BoundOut");
            };
        }
        Debug.Log("BindOuta");
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
