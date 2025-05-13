using UnityEngine;

public class RetreatState : IState
{
    private Vector3 retreatPosition;
    private float originalSpeed;
    public void Enter(EnemyAI enemy)
    {
        originalSpeed = enemy.agent.speed;
        retreatPosition = CalculateRetreatPosition(enemy);
        enemy.agent.speed = enemy.agent.speed * 1.5f;
        enemy.agent.SetDestination(retreatPosition);
    }
    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance) { }
    public void Update(EnemyAI enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (enemy.stamina < 100f)
            enemy.stamina += enemy.staminaRecoverRate * Time.deltaTime;

        enemy.stamina = Mathf.Clamp(enemy.stamina, 0f, 100f);

        if (distanceToPlayer >= enemy.retreatRange + 2f && enemy.stamina == 100)
        {
            if (enemy.playerInAttackRange && enemy.attackCooldown <= 0f)
                enemy.ChangeState(new TransitionState(0.5f, new ChargeAttackState()));
            else
                enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
        }
    }

    private Vector3 CalculateRetreatPosition(EnemyAI enemy)
    {
        Vector3 direction = (enemy.transform.position - enemy.player.position).normalized;
        float safeDistance = Mathf.Max(enemy.retreatRange + 3f, 10f);
        return enemy.transform.position + direction * safeDistance;
    }

    public void Exit(EnemyAI enemy) { enemy.agent.speed = originalSpeed; }

    
}