using System.Collections.Generic;
using UnityEngine;

public class EnemyActivationManager : MonoBehaviour
{
    public Transform player;
    public float activationDistance = 50f;
    public List<EnemyAI> allEnemies = new List<EnemyAI>();

    private void Start()
    {
        allEnemies.AddRange(FindObjectsOfType<EnemyAI>());

        foreach (var enemy in allEnemies)
        {
            enemy.enabled = false;
        }
    }

    private void Update()
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(player.position, enemy.transform.position);
            bool shouldBeActive = dist <= activationDistance;

            if (enemy.enabled != shouldBeActive)
            {
                enemy.enabled = shouldBeActive;
                if (shouldBeActive)
                    enemy.ResetAI(); // clean restart
            }
        }
    }
}
