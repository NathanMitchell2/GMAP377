using UnityEngine;

public class RotStorage : MonoBehaviour
{
    private Quaternion rot;
    void Update()
    {
        rot = transform.rotation;
    }

    public Quaternion GetRot()
    {
        return rot;
    }
}