using System.Collections.Generic;
using UnityEngine;

public class SingleCollliderHeal : MonoBehaviour
{
    private DamageObject m_DamageObject;
    private void Awake()
    {
        m_DamageObject = GetComponent<DamageObject>();
    }
    private void OnTriggerStay(Collider other)
    {
        List<GameObject> temp = new List<GameObject>();
        temp.Add(other.gameObject);

        List<PlayerHealth> targets = DamageObject.GetPlayerHealths(temp);
        if(targets.Count > 0 )
        {
            m_DamageObject.Damage(targets);
            Destroy(gameObject);
        }
    }
}
