using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private SpreadType type;

    public enum SpreadType
    {
        Single,
        Spread_Flat,
        Spread_Stack,
        Spread_Priority,
        Heal
    }

    public static void DamageDirect(float damage, PlayerHealth target)
    {
        if (target != null)
            target.ApplyDirectDamage(damage);
    }

    public static List<PlayerHealth> GetPlayerHealths(List<GameObject> recievers)
    {
        List<PlayerHealth> healths = new List<PlayerHealth>();

        foreach (GameObject player in recievers)
        {
            if (player != null)
            {
                PlayerHealth temp = player.GetComponent<PlayerHealth>();
                if (temp != null)
                    healths.Add(temp);
            }
        }

        return healths;
    }

    public void Damage(List<PlayerHealth> recievers)
    {
        if (recievers.Count == 0)
            return;

        switch (type)
        {
            case SpreadType.Single:
                int max = recievers[0].GetPriority();
                PlayerHealth target = recievers[0];

                foreach(PlayerHealth p in recievers)
                {
                    int temp = p.GetPriority();
                    if(temp > max)
                    {
                        max = temp;
                        target = p;
                    }    
                }
                Debug.Log("Damaged");
                target.TakeDamage(damage);
                break;

            case SpreadType.Spread_Flat:
                
                foreach(PlayerHealth p in recievers)
                {
                    p.TakeDamage(damage);
                }

                break;
            case SpreadType.Spread_Stack:

                foreach (PlayerHealth p in recievers)
                {
                    p.TakeDamage(damage/recievers.Count);
                }

                break;
            case SpreadType.Spread_Priority:
                Dictionary<int, List<PlayerHealth>> priorityBracket = new Dictionary<int, List<PlayerHealth>>();

                foreach(PlayerHealth p in recievers)
                {
                    priorityBracket[p.GetPriority()].Add(p);
                }

                float overdamage = 0;
                foreach (int i in priorityBracket.Keys)
                {
                    foreach(PlayerHealth p in priorityBracket[i])
                    {
                        overdamage += p.TakeDamage(damage / priorityBracket[i].Count);
                    }
                    if (!(overdamage > 0))
                        break;
                }
                break;
            case SpreadType.Heal:
                List<PlayerHealth> toRemove = new List<PlayerHealth>();

                for (int i = 0; i < recievers.Count; i++)
                {
                    PlayerHealth p = recievers[i].GetParent();
                    if (p != null && !recievers.Contains(p))
                        recievers.Add(p);
                }

                foreach (PlayerHealth p in recievers)
                {
                    if (p.GetParent() != null && recievers.Contains(p.GetParent()))
                        toRemove.Add(p);
                }

                foreach (PlayerHealth p in toRemove)
                {
                    recievers.Remove(p);
                }

                foreach (PlayerHealth p in recievers)
                {
                    p.Heal(damage);
                }
                break;

            default:
                foreach (PlayerHealth p in recievers)
                {
                    p.ApplyDirectDamage(damage);
                }
                break;
        }
    }

    public static void Damage(float damageAmount, PlayerHealth health)
    {
        if (health != null)
        {
            health.ApplyDirectDamage(damageAmount);
        }
    }
}
