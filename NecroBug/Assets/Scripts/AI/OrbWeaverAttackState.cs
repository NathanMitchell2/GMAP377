using System.Collections;
using UnityEngine;

public class OrbWeaverAttackState : IState
{
    private bool attacked = false;
    private Vector3 retreatTarget;

    public void Enter(EnemyAI enemy)
    {
        enemy.agent.SetDestination(enemy.transform.position);
        enemy.ChangeStateCoroutine(Attack(enemy));
    }

    public void Update(EnemyAI enemy)
    {
        // Make spider face the player while retreating
        if (retreatTarget != Vector3.zero)
        {
            Vector3 lookAtPos = new Vector3(enemy.player.position.x, enemy.transform.position.y, enemy.player.position.z);
            enemy.transform.LookAt(lookAtPos);
        }
    }

    public void Exit(EnemyAI enemy)
    {
        attacked = false;
        enemy.agent.enabled = true;
        enemy.rb.isKinematic = true;
        retreatTarget = Vector3.zero;
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

        Vector3 startPos = enemy.transform.position;
        Vector3 targetPos = enemy.player.position + Vector3.up * 1.2f; // leap over head
        Vector3 velocity = CalculateParabolicJump(startPos, targetPos, 4f, 0.8f);

        enemy.rb.velocity = velocity;
        yield return new WaitForSeconds(0.8f);

        if (Vector3.Distance(enemy.transform.position, enemy.player.position) <= 2f)
        {
            enemy.playerStats.TakeDamage(enemy.biteDamage);
        }

        enemy.rb.velocity = Vector3.zero;
        enemy.rb.angularVelocity = Vector3.zero;
        enemy.rb.isKinematic = true;
        enemy.agent.enabled = true;

        // Reset web stack after attack
        enemy.webStack = 0;

        // Retreat directly opposite from player (fixed distance)
        Vector3 retreatDir = (enemy.transform.position - enemy.player.position).normalized;
        retreatTarget = enemy.transform.position + retreatDir * 10f;
        enemy.agent.SetDestination(retreatTarget);

        while (Vector3.Distance(enemy.transform.position, retreatTarget) > 1f)
        {
            yield return null;
        }

        // Quick dart left or right and shoot
        yield return PerformDart(enemy);
        ShootWeb(enemy);

        yield return new WaitForSeconds(0.2f);

        // Dart again
        yield return PerformDart(enemy);

        yield return new WaitForSeconds(0.3f);
        enemy.ChangeState(new TransitionState(0.3f, new PatrolState()));
    }

    private IEnumerator PerformDart(EnemyAI enemy)
    {
        Vector3 toPlayer = (enemy.player.position - enemy.transform.position).normalized;
        Vector3 lateral = Random.value > 0.5f ? Vector3.Cross(Vector3.up, toPlayer) : Vector3.Cross(toPlayer, Vector3.up);
        Vector3 dartTarget = enemy.transform.position + lateral.normalized * 4f;

        enemy.agent.SetDestination(dartTarget);
        yield return new WaitForSeconds(0.4f); // dart duration
    }

    private void ShootWeb(EnemyAI enemy)
    {
        if (enemy.stamina < enemy.staminaDrainPerCharge) return;

        enemy.stamina -= enemy.staminaDrainPerCharge;
        enemy.stamina = Mathf.Clamp(enemy.stamina, 0f, 100f);

        GameObject web = GameObject.Instantiate(enemy.webProjectilePrefab, enemy.spitPoint.position, Quaternion.identity);
        Rigidbody rb = web.GetComponent<Rigidbody>();
        web.GetComponent<WebProjectile>().spider = enemy;

        Vector3 targetPos = enemy.player.position;
        Vector3 parabola = CalculateArcVelocity(enemy.spitPoint.position, targetPos, 1f, 0.05f, Physics.gravity.y);

        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.linearVelocity = parabola;
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

    private Vector3 CalculateArcVelocity(Vector3 start, Vector3 target, float baseTime, float timePerUnit, float gravity)
    {
        Vector3 displacement = target - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);
        float horizontalDistance = displacementXZ.magnitude;

        float timeToTarget = baseTime + horizontalDistance * timePerUnit;
        Vector3 velocityXZ = displacementXZ / timeToTarget;
        float verticalVelocity = (displacement.y + 0.5f * Mathf.Abs(gravity) * timeToTarget * timeToTarget) / timeToTarget;

        return velocityXZ + Vector3.up * verticalVelocity;
    }
}
