using UnityEngine;
using UnityEngine.UI;

public class UIUpdate : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyReductionBar;

    public void StatInitialize()
    {
        PlayerStats carObject = playerObject.GetComponentInChildren<PlayerStats>();
        carObject.Initialize(healthBar, energyBar, energyReductionBar);
    }
}
