using UnityEngine;

public class LegManager : MonoBehaviour
{
    public AnimLegControl[] legs;   // assign all 8 legs in inspector
    public int currentGroup = 0;    // 0 or 1
    public float groupStepDelay = 0.2f; // time between switching groups

    private float groupTimer = 0f;

    void Update()
    {
        groupTimer += Time.deltaTime;

        // Step all legs in the current group if ready
        foreach (var leg in legs)
        {
            if (leg.legGroup == currentGroup && !leg.IsMoving() && leg.IsReadyToStep())
            {
                leg.ForceStep();
            }
        }

        // Switch groups after delay
        if (groupTimer >= groupStepDelay)
        {
            currentGroup = 1 - currentGroup; // toggle between 0 and 1
            groupTimer = 0f;
        }
    }
}
