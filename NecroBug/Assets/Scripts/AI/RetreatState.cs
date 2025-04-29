using UnityEngine;

public class RetreatState : IState
{
    public void Enter(EnemyAI enemy)
    {
        RetreatFromPlayer(enemy);
    }

    public void Update(EnemyAI enemy)
    {
        // Continuously move away if needed
        RetreatFromPlayer(enemy);
    }

    public void Exit(EnemyAI enemy) { }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (distance > enemy.retreatRange && !playerInSightRange && !playerInAttackRange)
            enemy.ChangeState(new PatrolState());
    }

    private void RetreatFromPlayer(EnemyAI enemy)
    {
        Vector3 retreatDirection = (enemy.transform.position - enemy.player.position).normalized;
        Vector3 retreatPosition = enemy.transform.position + retreatDirection * 5f;
        enemy.agent.SetDestination(retreatPosition);
    }
}