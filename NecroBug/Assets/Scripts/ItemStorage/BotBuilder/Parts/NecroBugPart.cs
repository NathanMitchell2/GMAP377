using UnityEngine;

public class NecroBugPart : ModularBugPart
{
    public override void Activate()
    {
    }

    private void Update()
    {
        BuiltBotStorage.sourcePos = transform.position;

        Vector3 eRot = new Vector3(0, transform.eulerAngles.y, 0);
        Quaternion rot = Quaternion.Euler(eRot);

        BuiltBotStorage.sourceRot = rot;

        ConnectAnchor();
    }
}
