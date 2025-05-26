using UnityEngine;

public class RetreatState : IState
{
    private Vector3 retreatPosition;
    private float originalSpeed;
    public void Enter(EnemyAI enemy)
    {
        enemy.isInCombat = false;
        originalSpeed = enemy.agent.speed;
        retreatPosition = CalculateRetreatPosition(enemy);
        enemy.agent.speed = enemy.agent.speed * 1.5f;
        enemy.agent.SetDestination(retreatPosition);
    }
    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance) { }
    public void Update(EnemyAI enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);
        float distanceToRetreatPoint = Vector3.Distance(enemy.transform.position, retreatPosition);

        // Keep retreating if not far enough
        if (distanceToRetreatPoint < 1f && distanceToPlayer < enemy.retreatRange + 2f)
        {
            retreatPosition = CalculateRetreatPosition(enemy); // pick a farther one
            enemy.agent.SetDestination(retreatPosition);
            return;
        }

        // Transition once fully recovered and far enough
        if (distanceToPlayer >= enemy.retreatRange + 2f && enemy.stamina >= enemy.staminaDrainPerCharge)
        {
            if (enemy.playerInAttackRange && enemy.attackCooldown <= 0f)
                enemy.ChangeState(new TransitionState(0.5f, enemy.GetAttackState()));
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