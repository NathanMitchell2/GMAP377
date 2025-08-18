using UnityEngine;

public class AcidButton : MonoBehaviour
{
    [SerializeField] private GameObject roadblock;
    [SerializeField] private string playerTag = "Player";

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(playerTag) && roadblock != null)
        {
            roadblock.SetActive(false);
        }
    }
}
