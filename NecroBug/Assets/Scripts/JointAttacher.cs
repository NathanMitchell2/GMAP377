using UnityEngine;

public class JointAttacher : MonoBehaviour
{
    [SerializeField] private FixedJoint joint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joint.connectedBody = transform.parent.GetComponent<Rigidbody>();
        //Debug.Log(joint.connectedBody);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
