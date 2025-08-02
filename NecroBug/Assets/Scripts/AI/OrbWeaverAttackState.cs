using System.Collections;
using UnityEngine;

public class OrbWeaverAttackState : IState
{
    private Vector3 retreatTarget;
    private float originalSpeed;

    public void Enter(EnemyAI enemy)
    {
        // Cache original speed
        originalSpeed = enemy.agent.speed;
        // Allow rotation updates for facing player during retreat
        enemy.agent.updateRotation = false;
        // Start the attack sequence coroutine
        enemy.ChangeStateCoroutine(AttackSequence(enemy));
    }

    public void Update(EnemyAI enemy)
    {
        // While retreating, keep looking at the player
        if (retreatTarget != Vector3.zero)
        {
            Vector3 lookAt = enemy.player.position;
            lookAt.y = enemy.transform.position.y;
            enemy.transform.LookAt(lookAt);
        }
    }

    public void Exit(EnemyAI enemy)
    {
        // Restore original settings
        enemy.agent.speed = originalSpeed;
        enemy.agent.updateRotation = true;
        retreatTarget = Vector3.zero;
        enemy.agent.isStopped = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSight, bool playerInAttack, float dist)
    {
        // Handled in coroutine
    }

    private IEnumerator AttackSequence(EnemyAI enemy)
    {
        // Wind-up before pounce
        yield return new WaitForSeconds(0.3f);

        // Pounce jump
        enemy.agent.enabled = false;
        enemy.rb.isKinematic = false;
        Vector3 start = enemy.transform.position;
        Vector3 end = enemy.player.position + Vector3.up * 1.2f;
        enemy.rb.velocity = CalculateParabolicJump(start, end, 4f, 0.8f);
        yield return new WaitForSeconds(0.8f);

        // Bite damage
        if (Vector3.Distance(enemy.transform.position, enemy.player.position) <= 2f)
            enemy.playerStats.TakeDamage(enemy.biteDamage);

        // Reset physics and agent
        enemy.rb.velocity = Vector3.zero;
        enemy.rb.angularVelocity = Vector3.zero;
        enemy.rb.isKinematic = true;
        enemy.agent.enabled = true;

        // Reset web hit counter
        enemy.webStack = 0;

        // Retreat
        Vector3 dir = (enemy.transform.position - enemy.player.position).normalized;
        retreatTarget = enemy.transform.position + dir * enemy.retreatRange;
        enemy.agent.isStopped = false;
        enemy.agent.SetDestination(retreatTarget);

        // Wait until path is valid
        while (enemy.agent.pathPending)
            yield return null;
        // Wait until reached target
        while (enemy.agent.remainingDistance > enemy.agent.stoppingDistance)
            yield return null;

        // Debug and return to agro state
        yield return new WaitForSeconds(0.1f);
        enemy.ChangeState(new OrbWeaverAgroState());
    }

    // Helper to compute parabolic jump velocity
    private Vector3 CalculateParabolicJump(Vector3 start, Vector3 end, float height, float duration)
    {
        Vector3 disp = end - start;
        Vector3 horiz = new Vector3(disp.x, 0, disp.z);
        float vy = (disp.y + 0.5f * Mathf.Abs(Physics.gravity.y) * duration * duration) / duration;
        Vector3 vxz = horiz / duration;
        return vxz + Vector3.up * vy;
    }
}
