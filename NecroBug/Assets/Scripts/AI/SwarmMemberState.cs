using UnityEngine;

public class SwarmMemberState : IState
{
    private readonly EnemyAI _leader;
    private readonly int _slotIndex;

    // attack order tracking
    private int _lastSeenOrderId = -1;

    // dive state
    private bool _windingUp;
    private float _windupT;
    private bool _diving;
    private Vector3 _diveTarget;

    // hover
    private float _bobPhase;

    public SwarmMemberState(EnemyAI leader, int slotIndex)
    {
        _leader = leader;
        _slotIndex = slotIndex;
    }

    public void Enter(EnemyAI enemy)
    {
        enemy.isInCombat = true;

        // FLY MODE
        enemy.SetAgentEnabled(false);
        if (enemy.rb != null)
        {
            enemy.rb.isKinematic = true;
            enemy.rb.useGravity = false;
            enemy.rb.linearVelocity = Vector3.zero;
            enemy.rb.angularVelocity = Vector3.zero;
        }

        _bobPhase = Random.value * Mathf.PI * 2f;
        _windingUp = false;
        _diving = false;
        _windupT = 0f;
    }

    public void Update(EnemyAI enemy)
    {
        if (_leader == null)
        {
            Exit(enemy);
            enemy.ChangeState(new PatrolState());
            return;
        }

        // Check for a NEW order (compare ids so we don't miss if frames misalign)
        if (SwarmLeaderState.SwarmCoordinator.TryGet(_leader, out var leaderState))
        {
            int id = leaderState.CurrentOrderId;
            if (id != _lastSeenOrderId)
            {
                _lastSeenOrderId = id;
                // lock target to robot's current position
                _diveTarget = _leader.player.position;
                _windingUp = true;
                _windupT = 0f;
                _diving = false;
            }
        }

        if (_windingUp)
        {
            _windupT += Time.deltaTime;
            if (_windupT >= enemy.diveWindup)
            {
                _windingUp = false;
                _diving = true;
            }
            else
            {
                HoldFormationHover(enemy);
            }
            return;
        }

        if (_diving)
        {
            DoDive(enemy);
            return;
        }

        // Normal: hold formation
        HoldFormationHover(enemy);
    }

    private void HoldFormationHover(EnemyAI enemy)
    {
        int count = Mathf.Max(1, _leader.swarmMaxMembers);
        float angle = (Mathf.PI * 2f) * (_slotIndex / (float)count);
        Vector3 ringOffset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _leader.swarmRadius;

        Vector3 target = _leader.transform.position + ringOffset;

        // Put formation at hover height above ground
        if (Physics.Raycast(target + Vector3.up * 20f, Vector3.down, out var hit, 60f, _leader.groundMask))
            target.y = hit.point.y + _leader.hoverHeight;
        else
            target.y = _leader.transform.position.y + _leader.hoverHeight;

        _bobPhase += Time.deltaTime * _leader.hoverBobSpeed;
        target.y += Mathf.Sin(_bobPhase) * _leader.hoverBobAmplitude;

        enemy.transform.position = Vector3.Lerp(
            enemy.transform.position,
            target,
            Time.deltaTime * _leader.swarmReformLerp
        );

        // Face outward (looks cooler), or swap to face leader by reversing vector
        Vector3 outward = (enemy.transform.position - _leader.transform.position);
        outward.y = 0f;
        if (outward.sqrMagnitude > 0.0001f)
        {
            var look = Quaternion.LookRotation(outward.normalized, Vector3.up);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, look, Time.deltaTime * 8f);
        }
    }

    private void DoDive(EnemyAI enemy)
    {
        Vector3 startPos = enemy.transform.position;

        // Bias to ground at dive target
        float groundY = _diveTarget.y;
        if (Physics.Raycast(_diveTarget + Vector3.up * 20f, Vector3.down, out var gHit, 60f, enemy.groundMask))
            groundY = gHit.point.y;

        Vector3 toTarget = (_diveTarget - startPos);
        Vector3 dir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector3.forward;
        Vector3 step = dir * enemy.diveSpeed * Time.deltaTime;

        // small arc at start, then down as we approach
        if (Vector3.Distance(startPos, _diveTarget) > 3f)
            step += Vector3.up * (enemy.diveArcHeight * 0.5f) * Time.deltaTime;
        else
            step += Vector3.down * 0.5f * Time.deltaTime;

        Vector3 nextPos = startPos + step;

        // intrusive hit detection along segment
        Vector3 seg = nextPos - startPos;
        float segLen = Mathf.Max(seg.magnitude, 0.0001f);
        if (Physics.SphereCast(startPos, enemy.diveHitRadius, seg / segLen, out var hit, segLen, enemy.robotMask))
        {
            DealDamageViaTeamSystem(hit.collider, enemy.diveDamage);
            KillBee(enemy);
            return;
        }

        enemy.transform.position = nextPos;

        // Ground impact (whether we hit or not, we die)
        if (nextPos.y <= groundY + 0.05f)
        {
            KillBee(enemy);
            return;
        }

        // Face dive direction
        Vector3 face = (nextPos - startPos); face.y = 0f;
        if (face.sqrMagnitude > 0.0001f)
            enemy.transform.rotation = Quaternion.LookRotation(face.normalized, Vector3.up);
    }

    private void DealDamageViaTeamSystem(Collider c, int amount)
    {
        if (!c) return;

        // Try to get PlayerHealth directly from this collider's GameObject
        var list = DamageObject.GetPlayerHealths(c.gameObject);

        // Fallback: search up the hierarchy if the collider isn't on the root
        if (list.Count == 0)
        {
            var parentHealth = c.GetComponentInParent<PlayerHealth>();
            if (parentHealth != null) list.Add(parentHealth);
        }

        // If we found a target, use the team's static Damage() helper
        if (list.Count > 0)
        {
            // DamageObject.Damage expects int damage
            DamageObject.Damage(amount, list[0]);
        }
        // If you ever need to hit multiple colliders/players, you can expand:
        // var all = new List<PlayerHealth>();
        // all.AddRange(list);
        // new DamageObject { type = DamageObject.SpreadType.Spread_Flat, damage = amount }.Damage(all);
    }

    private void KillBee(EnemyAI enemy)
    {
        // TODO?: VFX/SFX or pooling
        Object.Destroy(enemy.gameObject);
    }

    public void Exit(EnemyAI enemy)
    {
        // Back to ground mode (agent will take over when they return to non-swarm states)
        if (enemy.rb != null)
        {
            enemy.rb.isKinematic = true;   // keep kinematic while agent runs
            enemy.rb.useGravity = false;  // agent handles height
        }
        enemy.SetAgentEnabled(true);
        enemy.isInCombat = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        // Members obey the leader only
    }
}