using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputStrategy pickUpStrat;
    [SerializeField] private InputStrategy moveStrat;
    [SerializeField] private InputStrategy jumpStrat;
    [SerializeField] private InputStrategy clickStrat;
    [SerializeField] private InputStrategy menuStrat;

    public void SetStrat(InputStrategy strat)
    {
        switch(strat.position)
        {
            case 1:
                if (pickUpStrat != null) removeStrat(pickUpStrat);
                pickUpStrat = strat;
                break;

            case 2:
                if (moveStrat != null) removeStrat(moveStrat);
                moveStrat = strat;
                break;

            case 3:
                if (jumpStrat != null) removeStrat(jumpStrat);
                jumpStrat = strat;
                break;

            case 4:
                if (clickStrat != null) removeStrat(clickStrat);
                clickStrat = strat;
                break;

            case 5:
                if (menuStrat != null) removeStrat(menuStrat);
                menuStrat = strat;
                break;

            default:
                break;
        }
        
    }

    private void removeStrat(InputStrategy strat)
    {
        if (hasStrat(strat))
        {
            Vector3 tempPos = strat.transform.position;
            strat.transform.parent = null;
            strat.transform.position = tempPos;

            switch (strat.position)
            {
                case 1:
                    pickUpStrat = null;
                    break;
                case 2:
                    moveStrat = null;
                    break;
                case 3:
                    jumpStrat = null;
                    break;
                case 4:
                    clickStrat = null;
                    break;
                case 5:
                    menuStrat = null;
                    break;
                default:
                    break;
            }
        }
    }

    public bool hasStrat(InputStrategy strat)
    {
        switch(strat.position)
        {
            case 1:
            return pickUpStrat != null;
            case 2:
            return moveStrat != null;
            case 3:
            return jumpStrat != null;
            case 4:
            return clickStrat != null;
            case 5:
            return menuStrat != null;
            default:
            return false;
        }
    }
    
    void OnPickUp(InputValue value)
    {
        if(pickUpStrat) pickUpStrat.RunStrategy(value);
    }
    
    void OnMove(InputValue value) 
    {
        if(moveStrat) moveStrat.RunStrategy(value);
    }
    
    void OnJump(InputValue value)
    {
        if(jumpStrat) jumpStrat.RunStrategy(value);
    }
        
    void OnClick(InputValue value)
    {
        if(clickStrat) clickStrat.RunStrategy(value);
    }

    void OnMenu(InputValue value)
    {
        if(menuStrat) menuStrat.RunStrategy(value);
    }
}
