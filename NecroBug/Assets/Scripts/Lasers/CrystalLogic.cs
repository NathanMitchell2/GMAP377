using UnityEngine;
using UnityEngine.Events;

public class CrystalLogic : MonoBehaviour
{
    public bool isTriggered = false;
    public enum CrystalType { Flash, Beam, Switch, Door }
    public CrystalType crystalType;

    public Laser laser;

    private void Start()
    {
        switch (crystalType)
        {
            case CrystalType.Flash:
                break;
            case CrystalType.Beam:
                break;
            case CrystalType.Switch:
                break;
            case CrystalType.Door:
                break;
        }
    }
    // Flip Flop
    public void CrystalTriggered()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            CrystalOn();
        }
        else
        {
            isTriggered = false;
            CrystalOff();
        }
    }
    public void CrystalOn()
    {
        if (crystalType == CrystalType.Flash)
        {
            Flash();
        }
        else if (crystalType == CrystalType.Beam)
        {
            Beam();
        }
        else if (crystalType == CrystalType.Switch)
        {
            SwitchOn();
        }
        else if (crystalType  != CrystalType.Door)
        {
            Door();
        }
    }
    public void CrystalOff()
    {
        if (crystalType == CrystalType.Flash)
        {
            // Enters cooldown
        }
        else if (crystalType == CrystalType.Beam)
        {
            // Enters Cooldown
        }
        else if (crystalType == CrystalType.Switch)
        {
            SwitchOff();
        }
        else if (crystalType != CrystalType.Door)
        {
            Door();
        }
    }

    void Flash()
    {
        // Instantiate flash + VFX

        // Crystal off
    }

    void Beam()
    {
        // Play VFX
        // Coroutine
        // Instantiate new laser

        // Crystal off
    }

    void SwitchOn()
    {
        // Get Door
        // Open door
    }

    void SwitchOff()
    {
        // Get Door
        // Close Door
    }

    void Door()
    {
        // Opens door
    }

}
