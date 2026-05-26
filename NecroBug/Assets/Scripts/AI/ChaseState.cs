using UnityEngine;

public class ChaseState : IState
{
    public void Enter(EnemyAI enemy) { enemy.isInCombat = true; }

    public void Update(EnemyAI enemy)
    {
        enemy.agent.SetDestination(enemy.player.position);
    }

    public void Exit(EnemyAI enemy) { }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (distance < enemy.data.retreatRange && enemy.stamina < enemy.data.staminaDrainPerCharge)
        {
            enemy.ChangeState(new TransitionState(0.5f, new RetreatState()));
        }
        else if (playerInAttackRange &&
                 enemy.playerSpeed < enemy.data.maxPlayerSpeedCharge &&
                 enemy.stamina >= enemy.data.staminaDrainPerCharge &&
                 enemy.attackCooldown <= 0f)
        {
            enemy.ChangeState(new TransitionState(0.5f, enemy.GetAttackState()));
        }
        else if (!playerInSightRange && !playerInAttackRange && distance > enemy.data.retreatRange)
        {
            enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
        }
    }
}
