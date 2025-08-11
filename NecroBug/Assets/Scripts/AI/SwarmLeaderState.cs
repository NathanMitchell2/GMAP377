using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwarmLeaderState : IState
{
    private List<EnemyAI> swarmMembers = new List<EnemyAI>();
    private float swarmRange = 50f;  // radius to recruit nearby bees

    public void Enter(EnemyAI enemy)
    {
        Debug.Log("Swarm leader entered");
        // Recruit all nearby bees (except self)
        Collider[] hits = Physics.OverlapSphere(enemy.transform.position, swarmRange);
        foreach (var hit in hits)
        {
            EnemyAI ai = hit.GetComponent<EnemyAI>();
            if (ai != null && ai != enemy && ai.enemyType == EnemyAI.EnemyType.Bee)
            {
                // disable their NavMeshAgent & switch to follow behavior
                var agent = ai.GetComponent<NavMeshAgent>();
                if (agent != null) agent.enabled = false;
                ai.ChangeState(new SwarmMemberState(enemy));
                swarmMembers.Add(ai);
            }
        }
    }

    public void Update(EnemyAI enemy)
    {
        // Leader continues normal chase toward player
        enemy.agent.isStopped = false;
        enemy.agent.SetDestination(enemy.player.position);
    }

    public void Exit(EnemyAI enemy)
    {
        foreach (var member in swarmMembers)
        {
            var agent = member.GetComponent<NavMeshAgent>();
            if (agent != null) agent.enabled = true;
            member.ChangeState(new PatrolState());
        }
        swarmMembers.Clear();
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
    }
}
