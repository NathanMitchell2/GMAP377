using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrystalLogic : MonoBehaviour
{
    public bool isTriggered = false;
    public enum CrystalType { Flash, Beam, Switch, Door }
    public CrystalType crystalType;

    [Header("Beam")]
    public Transform beamOrigin;
    public Laser laser;

    [Header("Switch")]
    public List<CrystalLogic> crystalDoorList;

    [Header("Door")]
    public Transform transformA;
    public Transform transformB;
    public bool onDoorA = true;

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

    public void SetLaser(Laser incomingLaser)
    {
        laser = incomingLaser;
    }

    // Flip Flop
    public void CrystalTriggered(Laser strikingLaser)
    {
        SetLaser(strikingLaser);
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
            //Door(transformA);
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
            //Door(transformB);
        }
    }

    void Flash()
    {
        // Instantiate flash + VFX

        // Crystal off
    }

    public void Beam()
    {
        laser.CastBeam(beamOrigin.position, beamOrigin.transform.up);
        // Play VFX
        // Coroutine
        // Instantiate new laser

        // Crystal off
    }

    void SwitchOn()
    {
        foreach(CrystalLogic door in crystalDoorList)
        {
            door.Door();
        }
        // Get Door
        // Open door
    }

    void SwitchOff()
    {
        foreach (CrystalLogic door in crystalDoorList)
        {
            door.Door();
        }
        // Get Door
        // Close Door
    }

    void Door()
    {
        // Opens door
        if (onDoorA)
        {
            onDoorA = false;
            transform.position = transformA.position;
        }
        else if (!onDoorA)
        {
            onDoorA = true;
            transform.position = transformB.position;
        }
    }

}
