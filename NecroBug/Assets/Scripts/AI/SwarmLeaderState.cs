using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SwarmLeaderState : IState
{
    // -------- Coordinator so anything can talk to a specific swarm leader --------
    public static class SwarmCoordinator
    {
        private static readonly Dictionary<EnemyAI, SwarmLeaderState> _leaders = new();

        public static void Register(EnemyAI leader, SwarmLeaderState state) => _leaders[leader] = state;
        public static void Unregister(EnemyAI leader) { if (_leaders.ContainsKey(leader)) _leaders.Remove(leader); }
        public static bool TryGet(EnemyAI leader, out SwarmLeaderState state) => _leaders.TryGetValue(leader, out state);

        // Public API: call this to order an attack wave now.
        public static void IssueAttack(EnemyAI leader)
        {
            if (TryGet(leader, out var state)) state.IssueAttackInternal();
        }
    }

    private readonly List<EnemyAI> _members = new();
    private float _cooldownTimer;
    private int _orderId;          // increases each time we issue an order
    private int _frameIssuedOrder; // for “just issued this frame” checks

    // Called by Coordinator
    private void IssueAttackInternal()
    {
        if (_cooldownTimer > 0f) return;
        _orderId++;
        _frameIssuedOrder = Time.frameCount;
        _cooldownTimer = _leaderRef.swarmAttackCooldown; // set after first Update
        _justIssued = true;
    }

    private EnemyAI _leaderRef;
    private bool _justIssued;

    public void Enter(EnemyAI enemy)
    {
        enemy.SetSwarmLeader(true);
        _leaderRef = enemy;
        enemy.isInCombat = true;
        SwarmCoordinator.Register(enemy, this);

        // Keep leader on NavMesh
        enemy.SetAgentEnabled(true);
        if (enemy.rb != null) { enemy.rb.isKinematic = true; enemy.rb.useGravity = false; }

        // -------- Recruit members --------
        var hits = Physics.OverlapSphere(enemy.transform.position, enemy.swarmRecruitRange);
        var candidates = new List<EnemyAI>();

        foreach (var h in hits)
        {
            if (!h || !h.TryGetComponent(out EnemyAI ai)) continue;
            if (ai == enemy) continue;
            if (ai.enemyType != EnemyAI.EnemyType.Bee) continue; // only bees
            candidates.Add(ai);
        }

        candidates = candidates
            .OrderBy(ai => (ai.transform.position - enemy.transform.position).sqrMagnitude)
            .Take(Mathf.Max(0, enemy.swarmMaxMembers))
            .ToList();

        for (int i = 0; i < candidates.Count; i++)
        {
            var m = candidates[i];
            if (m == null) continue;

            // Turn off their agent while in formation (they fly)
            var agent = m.GetComponent<NavMeshAgent>();
            if (agent != null) agent.enabled = false;

            m.ChangeState(new SwarmMemberState(enemy, i));
            _members.Add(m);
        }

        _cooldownTimer = 0f;
        _orderId = 0;
        _frameIssuedOrder = -1;
        _justIssued = false;
    }

    public void Update(EnemyAI enemy)
    {
        // Leader chases/steers via agent
        enemy.agent.isStopped = false;
        enemy.agent.SetDestination(enemy.player.position);

        // Advance cooldown
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer < 0f) _cooldownTimer = 0f;
        }

        // Optional auto-orders: when close enough and off cooldown
        if (enemy.swarmAutoIssueOrders
            && _cooldownTimer <= 0f
            && Vector3.Distance(enemy.transform.position, enemy.player.position) <= enemy.attackRange) // reuse attackRange
        {
            IssueAttackInternal();
        }

        // Reset frame flag after members had a chance to read it
        if (_justIssued && Time.frameCount > _frameIssuedOrder)
        {
            _justIssued = false; // end the single-frame pulse
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.SetSwarmLeader(false);
        // Restore members
        foreach (var m in _members)
        {
            if (m == null) continue;
            var agent = m.GetComponent<NavMeshAgent>();
            if (agent != null && !agent.enabled) agent.enabled = true;
            m.ChangeState(new PatrolState()); // or Idle if you prefer
        }
        _members.Clear();

        SwarmCoordinator.Unregister(enemy);

        enemy.isInCombat = false;
        enemy.agent.isStopped = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        if (!playerInSightRange && !playerInAttackRange)
            enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }

    // -------- Accessors for members --------
    public bool HasNewOrderThisFrame => _justIssued && Time.frameCount == _frameIssuedOrder;
    public int CurrentOrderId => _orderId;
}