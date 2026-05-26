using UnityEngine;
using FMODUnity;

public class OrbWeaverAgroState : IState
{
    private const string OrbShotSound = "event:/Spider/web";

    private float shootInterval = 1.5f;
    private float timer;

    public void Enter(EnemyAI enemy)
    {
        timer = shootInterval; // fire on first eligible frame
    }

    public void Update(EnemyAI enemy)
    {
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        // Always face the player
        Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
        enemy.transform.rotation = Quaternion.LookRotation(dir);

        // Close in until within attack range, then hold position
        if (dist > enemy.data.attackRange)
        {
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(enemy.player.position);
        }
        else
        {
            enemy.agent.isStopped = true;
        }

        // Fire a web on interval while web stacks are not yet maxed
        timer += Time.deltaTime;
        if (dist <= enemy.data.attackRange && timer >= shootInterval && enemy.webStack < enemy.data.maxWebStacks)
        {
            timer -= shootInterval;
            ShootWeb(enemy);
            RuntimeManager.PlayOneShot(OrbShotSound, Camera.main.transform.position);
        }

        // Transition to pounce attack once the web threshold is reached
        if (enemy.webStack >= enemy.data.maxWebStacks)
        {
            enemy.agent.isStopped = false;
            enemy.ChangeState(enemy.GetAttackState());
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.agent.isStopped = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSight, bool playerInAttack, float dist)
    {
        // Transitions are handled in Update
    }

    private void ShootWeb(EnemyAI enemy)
    {
        if (enemy.stamina <= 0f) return;
        enemy.stamina = Mathf.Clamp(enemy.stamina - enemy.data.staminaDrainPerCharge, 0f, 100f);

        GameObject web = GameObject.Instantiate(enemy.data.webProjectilePrefab, enemy.spitPoint.position, Quaternion.identity);
        Rigidbody rb   = web.GetComponent<Rigidbody>();
        web.GetComponent<WebProjectile>().spider = enemy;

        Vector3 target = enemy.player.position + Vector3.up * 1.5f;
        rb.linearVelocity = (target - enemy.spitPoint.position).normalized * enemy.data.webProjectileSpeed;
    }
}
