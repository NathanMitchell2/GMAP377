using UnityEngine;

public class PatrolState : IState
{
    public void Enter(EnemyAI enemy)
    {
        enemy.walkPointSet = false;
        enemy.isInCombat = false;
    }

    public void Update(EnemyAI enemy)
    {
        if (!enemy.walkPointSet)
            SearchWalkPoint(enemy);

        if (enemy.walkPointSet)
        {
            enemy.agent.SetDestination(enemy.walkPoint);
            if (Vector3.Distance(enemy.transform.position, enemy.walkPoint) < 1f)
            {
                enemy.ChangeState(new TransitionState(0.5f,new IdleState()));
            }
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.walkPointSet = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (playerInAttackRange && enemy.playerSpeed < enemy.maxPlayerSpeedCharge && enemy.stamina >= enemy.staminaDrainPerCharge && enemy.attackCooldown <= 0f)
            enemy.ChangeState(new TransitionState(0.5f, enemy.GetAttackState()));
        else if (playerInSightRange)
        {
            if (enemy.enemyType == EnemyAI.EnemyType.OrbWeaver)
                enemy.ChangeState(new TransitionState(0.5f, new OrbWeaverAgroState()));
            else if (enemy.enemyType == EnemyAI.EnemyType.Bee)
                enemy.ChangeState(new TransitionState(0.5f, new SwarmLeaderState()));
            else
                enemy.ChangeState(new TransitionState(0.5f, new ChaseState()));
        }
        else if (distance < enemy.retreatRange && enemy.stamina < enemy.staminaDrainPerCharge)
            enemy.ChangeState(new TransitionState(0.5f, new RetreatState()));
    }

    private void SearchWalkPoint(EnemyAI enemy)
    {
        float randomZ = Random.Range(-enemy.walkPointRange, enemy.walkPointRange);
        float randomX = Random.Range(-enemy.walkPointRange, enemy.walkPointRange);

        Vector3 candidate = new Vector3(
            enemy.patrolCenter.x + randomX,
            enemy.transform.position.y,
            enemy.patrolCenter.z + randomZ
        );

        if (Physics.Raycast(candidate, -Vector3.up, 2f, enemy.whatIsGround))
        {
            enemy.walkPoint = candidate;
            enemy.walkPointSet = true;
        }
    }
}
