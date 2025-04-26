using UnityEngine;

public class ChaseState : IState
{
    public void Enter(EnemyAI enemy) { }

    public void Update(EnemyAI enemy)
    {
        enemy.agent.SetDestination(enemy.player.position);
    }

    public void Exit(EnemyAI enemy) { }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (distance < enemy.retreatRange)
            enemy.ChangeState(new RetreatState());
        else if (playerInAttackRange && enemy.playerSpeed < enemy.maxPlayerSpeedCharge)
            enemy.ChangeState(new ChargeAttackState());
        else if (!playerInSightRange && !playerInAttackRange && distance > enemy.retreatRange)
            enemy.ChangeState(new PatrolState());
    }
}
