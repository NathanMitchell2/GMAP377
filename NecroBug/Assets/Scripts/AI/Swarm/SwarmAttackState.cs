using UnityEngine;

public class SwarmAttackState : IState
{
    private SwarmUnit _unit;
    private float _cooldown;

    public void Enter(EnemyAI enemy)
    {
        enemy.isInCombat = true;

        _unit = enemy.GetComponent<SwarmUnit>();
        if (_unit == null)
        {
            _unit = enemy.gameObject.AddComponent<SwarmUnit>();
            _unit.Bind(enemy);
        }

        enemy.agent.isStopped = false;

        _cooldown = Mathf.Max(0.05f, enemy.swarmAttackCooldown);
    }

    public void Update(EnemyAI enemy)
    {
        if (_unit) _unit.TickFormation();

        _cooldown -= Time.deltaTime;
        if (_cooldown <= 0f && enemy.playerInAttackRange)
        {
            if (_unit.TryOrderOneDive())           
                _cooldown = enemy.swarmAttackCooldown;
            else
                _cooldown = 0.15f;                  
        }

        if (_unit != null && _unit.AllSoldiersDead)
        {
            enemy.ChangeState(new TransitionState(0.2f, new ChargeAttackState()));
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.isInCombat = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (!playerInSightRange && !playerInAttackRange)
            enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }
}
