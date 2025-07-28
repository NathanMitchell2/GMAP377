using UnityEngine;

public class OrbWeaverAgroState : IState
{
    private float shootCooldown = 2f;
    private float lastShotTime = -10f;

    public void Enter(EnemyAI enemy)
    {
        enemy.agent.isStopped = false;
        enemy.agent.updateRotation = false;
    }

    public void Update(EnemyAI enemy)
    {
        Vector3 toPlayer = (enemy.player.position - enemy.transform.position).normalized;
        Vector3 strafeDirection = Vector3.Cross(Vector3.up, toPlayer).normalized;
        Vector3 moveTarget = enemy.transform.position + strafeDirection*5;

        enemy.agent.SetDestination(moveTarget);
        enemy.transform.LookAt(new Vector3(enemy.player.position.x, enemy.transform.position.y, enemy.player.position.z));

        if (Time.time - lastShotTime >= shootCooldown)
        {
            ShootWeb(enemy);
            lastShotTime = Time.time;
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.agent.updateRotation = true;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (playerInAttackRange && enemy.webStack >= enemy.maxWebStacks)
        {
            enemy.ChangeState(new TransitionState(0.3f, new OrbWeaverAttackState()));
        }
        else if (!playerInSightRange)
        {
            enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
        }
    }

    private void ShootWeb(EnemyAI enemy)
    {
        if (enemy.stamina < enemy.staminaDrainPerCharge) return;

        // Drain stamina
        enemy.stamina -= enemy.staminaDrainPerCharge;
        enemy.stamina = Mathf.Clamp(enemy.stamina, 0f, 100f);

        // Instantiate web projectile
        GameObject web = GameObject.Instantiate(enemy.webProjectilePrefab, enemy.spitPoint.position, Quaternion.identity);
        Rigidbody rb = web.GetComponent<Rigidbody>();
        web.GetComponent<WebProjectile>().spider = enemy;

        // Calculate trajectory
        Vector3 targetPos = enemy.player.position;
        Vector3 parabola = CalculateArcVelocity(enemy.spitPoint.position, targetPos, 1f, 0.05f, Physics.gravity.y);

        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.linearVelocity = parabola;

    }

    private Vector3 CalculateArcVelocity(Vector3 start, Vector3 target, float baseTime, float timePerUnit, float gravity)
    {
        Vector3 displacement = target - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);
        float horizontalDistance = displacementXZ.magnitude;

        float timeToTarget = baseTime + horizontalDistance * timePerUnit;
        Vector3 velocityXZ = displacementXZ / timeToTarget;
        float verticalVelocity = (displacement.y + 0.5f * Mathf.Abs(gravity) * timeToTarget * timeToTarget) / timeToTarget;

        return velocityXZ + Vector3.up * verticalVelocity;
    }
}
