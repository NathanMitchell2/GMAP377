using UnityEngine;

public class AddGridTransformInstant : MonoBehaviour
{
    [SerializeField] private Transform gridTransform;

    void Awake()
    {
        MediatorPart.BotPartTransform = gridTransform;
    }
}
