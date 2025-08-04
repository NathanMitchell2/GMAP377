using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LaserSensor : MonoBehaviour
{
    bool _isTriggered = false;

    List<Laser> strikingLasers;

    private void Awake()
    {
        {
            strikingLasers = new List<Laser>();
        }
    }

    private void Start()
    {
        _isTriggered = false;
    }

    public static void HandleLaser(Laser laser, LaserSensor prev, LaserSensor current)
    {
        if (prev == current)
            return;

        if (prev != null)
            prev.RemoveLaser(laser);

        if (current != null)
            current.AddLaser(laser);
    }

    void AddLaser(Laser strikingLaser)
    {
        strikingLasers.Add(strikingLaser);
        // onLaserAdded?.Invoke(strikingLaser);
        if (strikingLasers.Count == 1)
            _isTriggered = true;
    }

    void RemoveLaser(Laser unstrikingLaser)
    {
        strikingLasers.Remove(unstrikingLaser);
        // onLaserRemoved?.Invoke(unstrikingLaser);
        if (strikingLasers.Count == 0)
            _isTriggered = false;
    }
}
