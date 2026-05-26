using System.Collections;
using UnityEngine;
using FMODUnity;

public class AcidSpitState : IState
{
    private const string SpitSound = "event:/Bug/bug spit";

    public void Enter(EnemyAI enemy)
    {
        enemy.agent.SetDestination(enemy.transform.position);
        enemy.isInCombat = true;
        enemy.ChangeStateCoroutine(SpitAtPlayer(enemy));
        RuntimeManager.PlayOneShot(SpitSound, Camera.main.transform.position);
    }

    public void Update(EnemyAI enemy) { }

    public void Exit(EnemyAI enemy)
    {
        enemy.isInCombat = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance) { }

    private IEnumerator SpitAtPlayer(EnemyAI enemy)
    {
        yield return new WaitForSeconds(0.4f); // wind-up

        if (enemy.stamina < enemy.data.staminaDrainPerCharge)
        {
            enemy.ChangeState(new TransitionState(0.5f, new RetreatState()));
            yield break;
        }

        enemy.stamina = Mathf.Clamp(enemy.stamina - enemy.data.staminaDrainPerCharge, 0f, 100f);

        // Face the player at the moment of firing
        Vector3 targetPos = enemy.player.position;
        enemy.transform.LookAt(new Vector3(targetPos.x, enemy.transform.position.y, targetPos.z));

        // Spawn and launch the acid projectile on a parabolic arc
        GameObject acid = GameObject.Instantiate(enemy.data.acidProjectilePrefab, enemy.spitPoint.position, Quaternion.identity);
        Rigidbody rb    = acid.GetComponent<Rigidbody>();
        Vector3 parabola = CalculateArcVelocity(enemy.spitPoint.transform.position, targetPos, 1f, 0.05f, Physics.gravity.y);

        RenderTrajectory(enemy.lineRenderer, enemy.spitPoint.position, parabola, Physics.gravity.y);
        rb.linearDamping  = 0f;
        rb.angularDamping = 0f;
        rb.linearVelocity = parabola;

        enemy.alreadyAttacked = true;
        enemy.Invoke(nameof(enemy.ResetAttack), enemy.data.timeBetweenAttacks);
        enemy.attackCooldown = 1f;

        yield return new WaitForSeconds(1f);

        enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }

    /// <summary>
    /// Calculates the initial velocity needed to reach <paramref name="target"/> from
    /// <paramref name="start"/> under gravity using a parabolic arc.
    /// </summary>
    public static Vector3 CalculateArcVelocity(Vector3 start, Vector3 target, float baseTime, float timePerUnit, float gravity)
    {
        Vector3 displacement   = target - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);
        float   horizontalDist = displacementXZ.magnitude;
        float   timeToTarget   = baseTime + horizontalDist * timePerUnit;

        Vector3 velocityXZ      = displacementXZ / timeToTarget;
        float   verticalVelocity = (displacement.y + 0.5f * Mathf.Abs(gravity) * timeToTarget * timeToTarget) / timeToTarget;

        return velocityXZ + Vector3.up * verticalVelocity;
    }

    private void RenderTrajectory(LineRenderer lineRenderer, Vector3 start, Vector3 velocity, float gravity, int steps = 30, float timeStep = 0.1f)
    {
        lineRenderer.positionCount = steps + 1;
        for (int i = 0; i <= steps; i++)
        {
            float t = i * timeStep;
            lineRenderer.SetPosition(i, start + velocity * t + 0.5f * Vector3.up * gravity * t * t);
        }
    }
}
