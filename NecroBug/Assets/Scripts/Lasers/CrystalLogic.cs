using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CrystalLogic : MonoBehaviour
{
    public bool isTriggered = false;
    public enum CrystalType { Flash, Beam, Switch, Door }
    public CrystalType crystalType;
    public HitFlash hitFlash;

    [Header("Beam")]
    public Transform beamOrigin;
    public Laser laser;
    public ParticleSystem chargeParticles;
    public ParticleSystem sparkleParticle;
    // public Laser laser;

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

    // Flip Flop
    public void CrystalTriggered()
    {
        // laser = strikingLaser;
        // SetLaser(strikingLaser);
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
            Beam();
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
        StartCoroutine(ActivateLaser());
        /* if (strikingLaser != null)
        {
            strikingLaser.CastBeam(beamOrigin.position, beamOrigin.transform.up);
        }
        else
        {
            Debug.LogError("Laser is null. Ensure that it is assigned correctly.");
        }*/
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

        if (hitFlash != null) { hitFlash.TriggerFlash(); }
        
        // Get Door
        // Open door
    }

    void SwitchOff()
    {
        foreach (CrystalLogic door in crystalDoorList)
        {
            door.Door();
        }

        if (hitFlash != null) { hitFlash.TriggerFlash(); }
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
            transform.rotation = transformA.rotation;
        }
        else if (!onDoorA)
        {
            onDoorA = true;
            transform.position = transformB.position;
            transform.rotation = transformB.rotation;
        }
    }

    private IEnumerator ActivateLaser()
    {
        if (laser.activated)
        {
            laser.activated = false;
            yield break;
        }

        if (chargeParticles != null)
        {
            chargeParticles.Play();
        }
        yield return new WaitForSeconds(chargeParticles.main.duration);

        sparkleParticle.Play();

        yield return new WaitForSeconds(0.1f);

        laser.activated = true;

        yield return new WaitForSeconds(1.5f);

        laser.activated = false;
        isTriggered = false;
        
    }

}
