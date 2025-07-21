using System.Collections.Generic;
using UnityEngine;

public class HealAoe : MonoBehaviour
{
    private DamageObject m_DamageObject;
    private List<GameObject> m_List;
    private void Awake()
    {
        m_DamageObject = GetComponent<DamageObject>();
        m_List = new List<GameObject>();
    }
    private void OnTriggerStay(Collider other)
    {
        m_List.Add(other.gameObject);
    }

    private void Update()
    {
        if (m_List.Count > 0)
        {
            List<PlayerHealth> targets = DamageObject.GetPlayerHealths(m_List);
            m_DamageObject.Damage(targets);
            m_List.Clear();
        }
    }
}
