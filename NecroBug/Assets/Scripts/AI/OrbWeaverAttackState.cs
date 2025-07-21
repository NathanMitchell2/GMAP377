using System.Collections;
using UnityEngine;

public class OrbWeaverAttackState : IState
{
    private bool attacked = false;

    public void Enter(EnemyAI enemy)
    {
        enemy.agent.SetDestination(enemy.transform.position);
        enemy.ChangeStateCoroutine(Attack(enemy));
    }

    public void Update(EnemyAI enemy) { }

    public void Exit(EnemyAI enemy)
    {
        attacked = false;
        enemy.agent.enabled = true;
        enemy.rb.isKinematic = true;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        // Controlled via coroutine
    }

    private IEnumerator Attack(EnemyAI enemy)
    {
        yield return new WaitForSeconds(0.3f); // charge delay

        if (enemy.wasRecentlyHit)
        {
            enemy.wasRecentlyHit = false;
            enemy.ChangeState(new TransitionState(0.3f, new RetreatState()));
            yield break;
        }

        // Disable NavMesh and activate physics
        enemy.agent.enabled = false;
        enemy.rb.isKinematic = false;

        // Prepare jump target (above player center)
        Vector3 startPos = enemy.transform.position;
        Vector3 targetPos = enemy.player.position + Vector3.up * 1.2f; // leap over head

        // Compute arc velocity
        float jumpHeight = 4f;
        float timeToTarget = 0.8f;

        Vector3 velocity = CalculateParabolicJump(startPos, targetPos, jumpHeight, timeToTarget);
        enemy.rb.velocity = velocity;

        // Wait for landing
        yield return new WaitForSeconds(timeToTarget);

        // Bite if spider landed near player
        float biteRange = 2f;
        if (Vector3.Distance(enemy.transform.position, enemy.player.position) <= biteRange)
        {
            enemy.playerStats.TakeDamage(enemy.biteDamage);
        }

        // Reset
        enemy.webStack = 0;
        enemy.rb.velocity = Vector3.zero;
        enemy.rb.angularVelocity = Vector3.zero;
        enemy.rb.isKinematic = true;
        enemy.agent.enabled = true;

        yield return new WaitForSeconds(0.4f);
        enemy.ChangeState(new TransitionState(0.3f, new PatrolState()));
    }

    private Vector3 CalculateParabolicJump(Vector3 start, Vector3 end, float height, float duration)
    {
        Vector3 displacement = end - start;
        Vector3 horizontalDisplacement = new Vector3(displacement.x, 0, displacement.z);
        float verticalDisplacement = displacement.y;

        Vector3 horizontalVelocity = horizontalDisplacement / duration;
        float verticalVelocity = (verticalDisplacement + 0.5f * Mathf.Abs(Physics.gravity.y) * duration * duration) / duration;

        return horizontalVelocity + Vector3.up * verticalVelocity;
    }
}
