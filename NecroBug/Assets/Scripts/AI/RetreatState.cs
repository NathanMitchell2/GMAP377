using UnityEngine;

public class RetreatState : IState
{
    private Vector3 retreatPosition;

    public void Enter(EnemyAI enemy)
    {
        retreatPosition = CalculateRetreatPosition(enemy);
        enemy.agent.SetDestination(retreatPosition);
    }
    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance) { }
    public void Update(EnemyAI enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, retreatPosition);

        if (distance <= 1f)
        {
            if (enemy.playerInAttackRange)
                enemy.ChangeState(new ChargeAttackState());
            else
                enemy.ChangeState(new PatrolState());
        }
    }

    private Vector3 CalculateRetreatPosition(EnemyAI enemy)
    {
        Vector3 direction = (enemy.transform.position - enemy.player.position).normalized;
        return enemy.transform.position + direction * 5f;
    }

    public void Exit(EnemyAI enemy) { }

    
}