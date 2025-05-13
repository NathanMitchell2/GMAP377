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
        if (idleTimer >= enemy.idleDuration)
        {
            enemy.ChangeState(new TransitionState(0.5f,new PatrolState()));
        }
    }

    public void Exit(EnemyAI enemy)
    {
        // nothing specific for Idle Exit
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (distance < enemy.retreatRange && enemy.stamina < enemy.staminaDrainPerCharge)
        {
            enemy.ChangeState(new TransitionState(0.5f,new RetreatState()));
        }
        else if (playerInAttackRange &&
                enemy.playerSpeed < enemy.maxPlayerSpeedCharge &&
                enemy.stamina >= enemy.staminaDrainPerCharge &&
                enemy.attackCooldown <= 0f)
        {
            enemy.ChangeState(new TransitionState(0.5f, new ChargeAttackState()));
        }
        else if (playerInSightRange)
        {
            enemy.ChangeState(new TransitionState(0.5f,new ChaseState()));
        }
    }
}
