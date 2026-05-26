using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private float idleTimer = 0f;

    public void Enter(EnemyAI enemy)
    {
        idleTimer = 0f;
        enemy.agent.SetDestination(enemy.transform.position);
    }

    public void Update(EnemyAI enemy)
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= enemy.data.idleDuration)
            enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
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
        else if (playerInSightRange)
        {
            if (enemy.data.enemyType == EnemyType.OrbWeaver)
                enemy.ChangeState(new TransitionState(0.5f, new OrbWeaverAgroState()));
            else
                enemy.ChangeState(new TransitionState(0.5f, new ChaseState()));
        }
    }
}
