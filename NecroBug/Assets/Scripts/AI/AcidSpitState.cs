using System.Collections;
using UnityEngine;
using FMODUnity;

public class AcidSpitState : IState
{

    [SerializeField]
    private string spitSound = "event:/Bug/bug spit";

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance){}

    public void Enter(EnemyAI enemy)
    {
        enemy.agent.SetDestination(enemy.transform.position); // stop movement
        enemy.isInCombat = true;
        enemy.ChangeStateCoroutine(SpitAtPlayer(enemy));
        RuntimeManager.PlayOneShot(spitSound, Camera.main.transform.position);
        Debug.Log("spit sound played");
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.isInCombat = true;
    }

    public void Update(EnemyAI enemy)
    { }

    private IEnumerator SpitAtPlayer(EnemyAI enemy)
    {
        yield return new WaitForSeconds(0.4f); // wind-up time

        if (enemy.stamina < enemy.staminaDrainPerCharge)
        {
            enemy.ChangeState(new TransitionState(0.5f, new RetreatState()));
            yield break;
        }

        // Drain stamina
        enemy.stamina -= enemy.staminaDrainPerCharge;
        enemy.stamina = Mathf.Clamp(enemy.stamina, 0f, 100f);

        // Look at the player
        Vector3 targetPos = enemy.player.position;
        enemy.transform.LookAt(new Vector3(targetPos.x, enemy.transform.position.y, targetPos.z));

        // Instantiate and shoot acid
        GameObject acid = GameObject.Instantiate(enemy.acidProjectilePrefab, enemy.spitPoint.position, Quaternion.identity);
        Rigidbody rb = acid.GetComponent<Rigidbody>();

        // Calculate trajectory
        Vector3 parabola = CalculateArcVelocity(enemy.spitPoint.transform.position, targetPos, 1f, 0.05f, Physics.gravity.y);

        // Render path with line
        RenderTrajectory(enemy.lineRenderer, enemy.spitPoint.position, parabola, Physics.gravity.y);
        Vector3 dir = (targetPos - enemy.spitPoint.position).normalized;
        // rb.AddForce(dir * enemy.acidSpitForce, ForceMode.Impulse);
        // Debug.Log(parabola);
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.linearVelocity = parabola;

        // Cooldown
        enemy.alreadyAttacked = true;
        enemy.Invoke(nameof(enemy.ResetAttack), enemy.timeBetweenAttacks);
        enemy.attackCooldown = 1f;

        yield return new WaitForSeconds(1f); // let the projectile travel

        enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }

    public static Vector3 CalculateArcVelocity(Vector3 start, Vector3 target, float baseTime, float timePerUnit, float gravity)
    {
        Vector3 displacement = target - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);
        float horizontalDistance = displacementXZ.magnitude;

        float timeToTarget = baseTime + horizontalDistance * timePerUnit;

        Vector3 velocityXZ = displacementXZ / timeToTarget;

        float verticalVelocity = (displacement.y + 0.5f * Mathf.Abs(gravity) * timeToTarget * timeToTarget) / timeToTarget;

        return velocityXZ + Vector3.up * verticalVelocity;
    }

    void RenderTrajectory(LineRenderer lineRenderer, Vector3 start, Vector3 velocity, float gravity, int steps = 30, float timeStep = 0.1f)
    {
        lineRenderer.positionCount = steps + 1;

        for (int i = 0; i <= steps; i++)
        {
            float t = i * timeStep;
            Vector3 point = start + velocity * t + 0.5f * Vector3.up * gravity * t * t;
            lineRenderer.SetPosition(i, point);
        }
    }
}
