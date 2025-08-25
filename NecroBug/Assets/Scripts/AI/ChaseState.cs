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
        if (distance < enemy.retreatRange && enemy.stamina < enemy.staminaDrainPerCharge)
            enemy.ChangeState(new TransitionState(0.5f,new RetreatState()));
        else if (playerInAttackRange &&
                enemy.playerSpeed < enemy.maxPlayerSpeedCharge &&
                enemy.stamina >= enemy.staminaDrainPerCharge &&
                enemy.attackCooldown <= 0f)
        {
            Debug.Log("entered");
            enemy.ChangeState(new TransitionState(0.5f, enemy.GetAttackState()));
        }
        else if (!playerInSightRange && !playerInAttackRange && distance > enemy.retreatRange)
            enemy.ChangeState(new TransitionState(0.5f,new PatrolState()));
    }
}
