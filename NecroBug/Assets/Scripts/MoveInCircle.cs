using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public Transform centerPoint; // The point to orbit around
    public float radius = 2f;
    public float speed = 1f;

    private float angle = 0f;

    void Update()
    {
        angle += speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Set position around center point
        transform.position = centerPoint.position + new Vector3(x, 0f, z);
    }
}
