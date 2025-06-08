using UnityEngine;

[ExecuteAlways] // So it works in edit mode too
public class Mushroom : MonoBehaviour
{
    [Range(1f, 2f)]
    public float heightMultiplier = 1f;

    public Transform stalk;
    public Transform top;

    public float baseScaleY = 0.01f;
    public float baseTopY = 2.45f;
    public float correctionPerUnit = 0.12f;

    void Start()
    {
        if (stalk != null)
        {
            // Scale the stalk
            Vector3 s = stalk.localScale;
            s.y = baseScaleY * heightMultiplier;
            stalk.localScale = s;

            // Adjust the cap
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
}
