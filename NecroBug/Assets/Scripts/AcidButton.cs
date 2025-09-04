using UnityEngine;

public class AcidButton : MonoBehaviour
{
    [SerializeField] private GameObject roadblock;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && roadblock != null)
        {
            roadblock.SetActive(false);
        }
    }
}
