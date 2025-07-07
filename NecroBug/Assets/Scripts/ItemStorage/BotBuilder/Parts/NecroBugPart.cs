using UnityEngine;

public class NecroBugPart : ModularBugPart
{
    public override void Activate()
    {
    }

    private void Update()
    {
        BuiltBotStorage.sourcePos = transform.localPosition;

        Vector3 eRot = new Vector3(0, transform.localEulerAngles.y, 0);
        Quaternion rot = Quaternion.Euler(eRot);

        BuiltBotStorage.sourceRot = rot;
    }
}
