using UnityEngine;
using UnityEngine.UI;

public class UIUpdate : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyReductionBar;
    private PlayerHealth playerStats;

    private void Update()
    {
        healthBar.fillAmount = (float)GetStats().GetHealth() / 100;
    }
    private PlayerHealth GetStats()
    {
        if(playerStats == null)
        {
            return playerObject.GetComponentInChildren<PlayerHealth>();
        }
        return playerStats;
    }
}
