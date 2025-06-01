using UnityEngine;
using UnityEngine.UI;

public class UIUpdate : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyReductionBar;
    private PlayerStats playerStats;

    public void StatInitialize()
    {
        //PlayerStats carObject = playerObject.GetComponentInChildren<PlayerStats>();
        //carObject.Initialize(healthBar, energyBar, energyReductionBar);
    }

    private void Update()
    {
        healthBar.fillAmount = (float)GetStats().health / 100;
    }
    private PlayerStats GetStats()
    {
        if(playerStats == null)
        {
            return playerObject.GetComponentInChildren<PlayerStats>();
        }
        return playerStats;
    }
}
