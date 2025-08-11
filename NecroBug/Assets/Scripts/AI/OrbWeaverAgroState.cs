using System.Collections;
using UnityEngine;

public class OrbWeaverAgroState : IState
{
    private float shootInterval = 1.5f;
    private float timer;

    public void Enter(EnemyAI enemy)
    {
        // First shot immediately
        timer = shootInterval;
    }

    public void Update(EnemyAI enemy)
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        // Face the player
        Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
        enemy.transform.rotation = Quaternion.LookRotation(dir);

        // Move into attack range, else stop
        if (dist > enemy.attackRange)
        {
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(enemy.player.position);
        }
        else
        {
            enemy.agent.isStopped = true;
        }

        // Check if we should shoot and dart
        timer += Time.deltaTime;
        if (dist <= enemy.attackRange && timer >= shootInterval && enemy.webStack < enemy.maxWebStacks)
        {
            timer -= shootInterval;
            // Fire and strafe, using coroutine to manage timing
            ShootWeb(enemy);
        }

        // Transition to pounce when threshold reached
        if (enemy.webStack >= enemy.maxWebStacks)
        {
            enemy.agent.isStopped = false;
            enemy.ChangeState(enemy.GetAttackState());
        }
    }

    public void Exit(EnemyAI enemy)
    {
        // Resume movement
        enemy.agent.isStopped = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSight, bool playerInAttack, float dist)
    {
        // Handled in Update
    }

    private void ShootWeb(EnemyAI enemy)
    {
        if (enemy.stamina <= 0f) return;
        enemy.stamina = Mathf.Clamp(enemy.stamina - enemy.staminaDrainPerCharge, 0f, 100f);
        GameObject web = GameObject.Instantiate(enemy.webProjectilePrefab,
                                               enemy.spitPoint.position,
                                               Quaternion.identity);
        Rigidbody rb = web.GetComponent<Rigidbody>();
        web.GetComponent<WebProjectile>().spider = enemy;
        Vector3 target = enemy.player.position + Vector3.up * 3f;
        rb.linearVelocity = (target - enemy.spitPoint.position).normalized * enemy.webProjectileSpeed;
        // Actual hit counting should occur in WebProjectile.OnCollision
    }

}

