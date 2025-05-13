using System.Collections.Generic;
using UnityEngine;

public class LegAttacher : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponentInChildren<AnimLegControl>() != null && player.GetComponentInChildren<AnimLegControl>().tipController == null)
        {
            List<LegIdentifier> legs = new List<LegIdentifier>(player.GetComponentsInChildren<LegIdentifier>());
            foreach (var l in legs)
            {
                int identity = l.identifier;

                l.GetComponentInChildren<AnimLegControl>().tipController = transform.GetChild(identity).gameObject;
            }
        }
    }
}
