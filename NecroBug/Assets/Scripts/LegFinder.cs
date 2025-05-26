using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LegFinder : MonoBehaviour
{
    [SerializeField] private GameObject tipController;
    [SerializeField] private GameObject controllerGroup;
    void Awake()
    {
        GameObject tipTemp = Instantiate(tipController);
        GetComponentInChildren<ChainIKConstraint>().data.target = tipTemp.transform;
        GetComponentInChildren<AnimLegControl>().tipController = tipTemp;
        RigBuilder rig = GetComponentInChildren<RigBuilder>();
        rig.Build();
    }
}
