using UnityEngine;

public class MushroomOscillator : MonoBehaviour
{
    public Transform stalk;
    public Transform top;

    [Header("Growth Settings")]
    public float baseScaleY = 0.01f;
    public float baseTopY = 2.45f;
    public float correctionPerUnit = 0.12f;

    [Range(1f, 2f)]
    public float heightMultiplier = 1f;

    public float oscillationSpeed = 1f; // Speed of growth/shrink

    void Update()
    {
        // Oscillate heightMultiplier between 1 and 2
        heightMultiplier = Mathf.Lerp(1f, 2f, (Mathf.Sin(Time.time * oscillationSpeed) + 1f) / 2f);

        // Apply scaling to stalk
        if (stalk != null)
        {
            Vector3 s = stalk.localScale;
            s.y = baseScaleY * heightMultiplier;
            stalk.localScale = s;
        }

        // Adjust cap position
        if (top != null)
        {
            float deltaScale = heightMultiplier - 1f;
            float correctedTopY = baseTopY * heightMultiplier - (correctionPerUnit * deltaScale);
            Vector3 pos = top.localPosition;
            pos.y = correctedTopY;
            top.localPosition = pos;
        }
    }
}
