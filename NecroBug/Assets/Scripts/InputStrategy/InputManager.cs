using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputStrategy pickUpStrat;
    [SerializeField] private InputStrategy moveStrat;
    [SerializeField] private InputStrategy jumpStrat;
    [SerializeField] private InputStrategy clickStrat;

    public void SetStrat(InputStrategy strat)
    {
        if(!hasStrat(strat))
        {
            switch(strat.position)
            {
                case 1:
                pickUpStrat = strat;
                break;

                case 2:
                moveStrat = strat;
                break;

                case 3:
                jumpStrat = strat;
                break;

                case 4:
                clickStrat = strat;
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
}
