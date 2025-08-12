using UnityEngine;

public class EyePart : ModularBugPart
{
    public Laser laser;
    public override void Activate()
    {
        laser.PlayerShoot();
    }
}
