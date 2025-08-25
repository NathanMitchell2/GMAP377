using UnityEngine;

public class SwarmAttackState : IState
{
    private SwarmUnit _unit;
    private float _cooldown;

    private bool _leaderCharging;
    private bool _leaderDiving;
    private float _leaderChargeTimer;
    private Vector3 _leaderDiveTarget;

    private bool _hadAgent;
    private bool _agentWasEnabled;

    public void Enter(EnemyAI enemy)
    {
        enemy.isInCombat = true;

        _unit = enemy.GetComponent<SwarmUnit>();
        if (_unit == null)
        {
            _unit = enemy.gameObject.AddComponent<SwarmUnit>();
            _unit.Bind(enemy);
        }

        _hadAgent = enemy.agent != null;
        if (_hadAgent)
        {
            _agentWasEnabled = enemy.agent.enabled;
            if (_agentWasEnabled)
            {
                enemy.agent.ResetPath();
                enemy.agent.isStopped = true;
                enemy.agent.enabled = false;
            }
        }

        _cooldown = Mathf.Max(0.05f, enemy.swarmAttackCooldown);
        _leaderCharging = false;
        _leaderDiving = false;
        _leaderChargeTimer = 0f;
    }

    public void Update(EnemyAI enemy)
    {
        if (_unit) _unit.TickFormation();

        if (_leaderCharging)
        {
            _leaderChargeTimer -= Time.deltaTime;
            if (_leaderChargeTimer <= 0f)
            {
                _leaderCharging = false;
                _leaderDiving = true;
            }
            return;
        }

        if (_leaderDiving)
        {
            DoLeaderDiveStep(enemy);
            return;
        }

        _cooldown -= Time.deltaTime;
        if (_cooldown <= 0f && enemy.playerInAttackRange)
        {
            if (_unit != null && _unit.TryOrderOneDive())
            {
                _cooldown = enemy.swarmAttackCooldown;
            }
            else
            {
                _leaderCharging = true;
                _leaderChargeTimer = enemy.diveWindup;
                _leaderDiveTarget = enemy.player.position;
                _cooldown = enemy.swarmAttackCooldown;
            }
        }
    }

    private void DoLeaderDiveStep(EnemyAI enemy)
    {
        Vector3 start = enemy.transform.position;

        float groundY = _leaderDiveTarget.y;
        if (Physics.Raycast(_leaderDiveTarget + Vector3.up * 20f, Vector3.down, out var gHit, 60f, enemy.groundMask))
            groundY = gHit.point.y;

        Vector3 toTarget = (_leaderDiveTarget - start);
        Vector3 dir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector3.forward;

        Vector3 step = dir * enemy.diveSpeed * Time.deltaTime;
        if (Vector3.Distance(start, _leaderDiveTarget) > 3f)
            step += Vector3.up * (enemy.diveArcHeight * 0.5f) * Time.deltaTime;
        else
            step += Vector3.down * 0.5f * Time.deltaTime;

        Vector3 next = start + step;
        Vector3 seg = next - start;
        float segLen = Mathf.Max(seg.magnitude, 0.0001f);
        Vector3 segDir = seg / segLen;

        if (Physics.SphereCast(start, enemy.diveHitRadius, segDir, out var hit, segLen, enemy.robotMask))
        {
            var list = DamageObject.GetPlayerHealths(hit.collider.gameObject);
            if (list.Count == 0)
            {
                var parentHealth = hit.collider.GetComponentInParent<PlayerHealth>();
                if (parentHealth != null) list.Add(parentHealth);
            }
            if (list.Count > 0) DamageObject.Damage(enemy.diveDamage, list[0]);
            Object.Destroy(enemy.gameObject);
            return;
        }

        if (Physics.SphereCast(start, enemy.diveHitRadius, segDir, out var gAlong, segLen, enemy.groundMask))
        {
            Object.Destroy(enemy.gameObject);
            return;
        }

        enemy.transform.position = next;
        Vector3 f = seg; f.y = 0f;
        if (f.sqrMagnitude > 0.0001f)
            enemy.transform.rotation = Quaternion.LookRotation(f.normalized, Vector3.up);

        if (next.y <= groundY + 0.05f)
        {
            Object.Destroy(enemy.gameObject);
            return;
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.isInCombat = false;

        if (_hadAgent && _agentWasEnabled && enemy && enemy.agent)
        {
            enemy.agent.enabled = true;
            enemy.agent.Warp(enemy.transform.position);
            enemy.agent.isStopped = false;
        }
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (!playerInSightRange && !playerInAttackRange && !_leaderCharging && !_leaderDiving)
            enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }
}
