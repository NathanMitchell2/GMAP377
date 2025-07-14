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
