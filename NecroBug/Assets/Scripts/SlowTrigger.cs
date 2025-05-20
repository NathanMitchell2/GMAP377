using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SlowTrigger : MonoBehaviour
{
    public float speedReduce = .5f;
    public float steerReduce = .5f;
    private void OnTriggerStay(Collider other)
    {
        carControler car = other.gameObject.GetComponentInParent<carControler>();
        if (car != null)
        {
            car.TerrainSpeedDown(speedReduce, steerReduce);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        carControler car = other.gameObject.GetComponentInParent<carControler>();
        if (car != null)
        {
            car.TerrainSpeedReset();
        }
    }
}
