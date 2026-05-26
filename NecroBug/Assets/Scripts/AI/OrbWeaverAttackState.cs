using System.Collections;
using UnityEngine;
using FMODUnity;

public class OrbWeaverAttackState : IState
{
    private const string PounceSound = "event:/Spider/Bite";

    private Vector3 retreatTarget;
    private float originalSpeed;

    public void Enter(EnemyAI enemy)
    {
        originalSpeed = enemy.agent.speed;
        enemy.agent.updateRotation = false;
        enemy.ChangeStateCoroutine(AttackSequence(enemy));
    }

    public void Update(EnemyAI enemy)
    {
        // Keep facing the player during the post-pounce retreat
        if (retreatTarget != Vector3.zero)
        {
            Vector3 lookAt = enemy.player.position;
            lookAt.y = enemy.transform.position.y;
            enemy.transform.LookAt(lookAt);
        }
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.agent.speed = originalSpeed;
        enemy.agent.updateRotation = true;
        retreatTarget = Vector3.zero;
        enemy.agent.isStopped = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSight, bool playerInAttack, float dist)
    {
        // Transitions are handled by the coroutine
    }

    private IEnumerator AttackSequence(EnemyAI enemy)
    {
        yield return new WaitForSeconds(0.3f); // wind-up

        // Physics-driven pounce jump
        enemy.agent.enabled  = false;
        enemy.rb.isKinematic = false;

        Vector3 start = enemy.transform.position;
        Vector3 end   = enemy.player.position + Vector3.up * 1.2f;
        enemy.rb.linearVelocity = CalculateParabolicJump(start, end, 0.8f);

        RuntimeManager.PlayOneShot(PounceSound, Camera.main.transform.position);
        yield return new WaitForSeconds(0.8f);

        enemy.isCharging = true;

        // Restore physics and agent
        enemy.rb.linearVelocity  = Vector3.zero;
        enemy.rb.angularVelocity = Vector3.zero;
        enemy.rb.isKinematic     = true;
        enemy.agent.enabled      = true;

        enemy.webStack = 0;

        // Retreat away from the player
        Vector3 retreatDir = (enemy.transform.position - enemy.player.position).normalized;
        retreatTarget = enemy.transform.position + retreatDir * enemy.data.retreatRange;
        enemy.agent.isStopped = false;
        enemy.agent.SetDestination(retreatTarget);

        while (enemy.agent.pathPending) yield return null;
        while (enemy.agent.remainingDistance > enemy.agent.stoppingDistance) yield return null;

        enemy.isCharging = false;
        yield return new WaitForSeconds(0.1f);
        enemy.ChangeState(new OrbWeaverAgroState());
    }

    /// <summary>
    /// Calculates the initial velocity for a parabolic jump from
    /// <paramref name="start"/> to <paramref name="end"/> over <paramref name="duration"/> seconds.
    /// </summary>
    private Vector3 CalculateParabolicJump(Vector3 start, Vector3 end, float duration)
    {
        Vector3 disp  = end - start;
        Vector3 horiz = new Vector3(disp.x, 0, disp.z);
        float   vy    = (disp.y + 0.5f * Mathf.Abs(Physics.gravity.y) * duration * duration) / duration;
        return horiz / duration + Vector3.up * vy;
    }
}
