using UnityEngine;

public class SwarmMemberState : IState
{
    private EnemyAI leader;
    private Vector3 offset;
    private float followSpeed;

    public SwarmMemberState(EnemyAI leader)
    {
        this.leader = leader;
        // random offset so bees don't stack exactly
        offset = Random.insideUnitSphere * 1.5f;
        offset.y = 0f;
    }

    public void Enter(EnemyAI enemy)
    {
        Debug.Log("swarm memeber");
        // disable nav agent, cache speed
        enemy.agent.enabled = false;
        followSpeed = enemy.getSpeed();
    }

    public void Update(EnemyAI enemy)
    {
        // Move directly toward leader + offset
        Vector3 target = leader.transform.position + offset;
        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            target,
            followSpeed * Time.deltaTime
        );
    }

    public void Exit(EnemyAI enemy)
    {
        // restore nav agent
        enemy.agent.enabled = true;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
  
    }
}
