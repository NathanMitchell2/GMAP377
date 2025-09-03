using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    private float _repathCooldown = 0.4f;
    private float _repathTimer;
    private float _stuckTimer;
    private Vector3 _lastPos;

    public void Enter(EnemyAI enemy)
    {
        enemy.isInCombat = false;
        enemy.walkPointSet = false;

        if (enemy.agent != null)
        {
            if (!enemy.agent.enabled) enemy.agent.enabled = true;
            enemy.agent.updatePosition = true;
            enemy.agent.updateRotation = true;
            enemy.agent.isStopped = false;
            enemy.agent.autoBraking = false;         
            if (enemy.agent.stoppingDistance < 0.5f)  
                enemy.agent.stoppingDistance = 0.6f;
        }

        _repathTimer = 0f;
        _stuckTimer = 0f;
        _lastPos = enemy.transform.position;
    }

    public void Update(EnemyAI enemy)
    {
        _repathTimer -= Time.deltaTime;

        if (!enemy.walkPointSet)
            SearchWalkPoint(enemy);

        if (enemy.walkPointSet && enemy.agent != null && enemy.agent.enabled)
        {
            if (!enemy.agent.hasPath && _repathTimer <= 0f)
            {
                enemy.agent.SetDestination(enemy.walkPoint);
                _repathTimer = _repathCooldown;
            }

            float moved = (enemy.transform.position - _lastPos).sqrMagnitude;
            _lastPos = enemy.transform.position;

            bool pathReady = !enemy.agent.pathPending;
            bool farFromGoal = enemy.agent.remainingDistance > enemy.agent.stoppingDistance + 0.2f;
            bool barelyMoving = enemy.agent.velocity.sqrMagnitude < 0.02f && moved < 0.0004f;

            if (pathReady && farFromGoal && barelyMoving)
            {
                _stuckTimer += Time.deltaTime;
                if (_stuckTimer > 1.0f)   
                {
                    enemy.agent.ResetPath();
                    enemy.walkPointSet = false;
                    _stuckTimer = 0f;
                    return;
                }
            }
            else
            {
                _stuckTimer = 0f;
            }

            if (pathReady && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + 0.2f)
            {
                enemy.ChangeState(new TransitionState(0.5f, new IdleState()));
            }
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.walkPointSet = false;
        if (enemy.agent != null && enemy.agent.enabled)
        {
            enemy.agent.autoBraking = true; 
        }
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (playerInAttackRange && enemy.playerSpeed < enemy.maxPlayerSpeedCharge &&
            enemy.stamina >= enemy.staminaDrainPerCharge && enemy.attackCooldown <= 0f)
        {
            enemy.ChangeState(new TransitionState(0.5f, enemy.GetAttackState()));
        }
        else if (playerInSightRange)
        {
            if (enemy.enemyType == EnemyAI.EnemyType.OrbWeaver)
                enemy.ChangeState(new TransitionState(0.5f, new OrbWeaverAgroState()));
            else
                enemy.ChangeState(new TransitionState(0.5f, new ChaseState()));
        }
        else if (distance < enemy.retreatRange && enemy.stamina < enemy.staminaDrainPerCharge)
        {
            enemy.ChangeState(new TransitionState(0.5f, new RetreatState()));
        }
    }

    private void SearchWalkPoint(EnemyAI enemy)
    {
        for (int i = 0; i < 6; i++)
        {
            float randomZ = Random.Range(-enemy.walkPointRange, enemy.walkPointRange);
            float randomX = Random.Range(-enemy.walkPointRange, enemy.walkPointRange);

            Vector3 candidate = new Vector3(
                enemy.patrolCenter.x + randomX,
                enemy.patrolCenter.y,
                enemy.patrolCenter.z + randomZ
            );

            if (NavMesh.SamplePosition(candidate, out var hit, 3.0f, NavMesh.AllAreas))
            {
                var path = new NavMeshPath();
                if (enemy.agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    enemy.walkPoint = hit.position;
                    enemy.walkPointSet = true;

                    enemy.agent.SetDestination(enemy.walkPoint);
                    _repathTimer = _repathCooldown;
                    return;
                }
            }
        }

        enemy.walkPointSet = false;
    }
}
