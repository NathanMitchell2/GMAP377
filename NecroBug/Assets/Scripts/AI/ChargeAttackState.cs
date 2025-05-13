using System.Collections;
using UnityEngine;

public class ChargeAttackState : IState
{
    private bool isChargingStarted = false;

    public void Enter(EnemyAI enemy)
    {
            enemy.agent.SetDestination(enemy.transform.position); // stop movement
            enemy.isCharging = true;
            enemy.isInCombat = true;
            enemy.ChangeStateCoroutine(ChargeAndSmash(enemy));
    }

    public void Update(EnemyAI enemy) { }

    public void Exit(EnemyAI enemy)
    {
        enemy.isCharging = false;
        enemy.isInCombat = true;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        // Handled by coroutine
    }

    private IEnumerator ChargeAndSmash(EnemyAI enemy)
    {
        isChargingStarted = true;

        if (enemy.stamina < enemy.staminaDrainPerCharge)
        {
            enemy.ChangeState(new TransitionState(0.5f, new RetreatState()));
            yield break;
        }

        Vector3 targetPosition = enemy.player.position;
        enemy.transform.LookAt(targetPosition);

        // Disable agent and enable physics for lift and charge
        enemy.agent.enabled = false;
        enemy.rb.isKinematic = true;

        // Smooth lift (hover effect)
        float liftDuration = 0.4f;
        float startY = enemy.transform.position.y;
        float targetY = startY + 1f;
        float timer = 0f;

        while (timer < liftDuration)
        {
            float newY = Mathf.Lerp(startY, targetY, timer / liftDuration);
            Vector3 currentPos = enemy.transform.position;
            enemy.transform.position = new Vector3(currentPos.x, newY, currentPos.z);

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // suspense pause

        // Deduct stamina
        enemy.stamina -= enemy.staminaDrainPerCharge;
        enemy.stamina = Mathf.Clamp(enemy.stamina, 0f, 100f);

        // Enable physics and charge
        enemy.rb.isKinematic = false;
        Vector3 toPlayer = (targetPosition - enemy.transform.position).normalized;
        toPlayer.y = -0.1f; // downwards influence
        toPlayer.Normalize();

        float chargeForce = enemy.jumpForce * enemy.rb.mass;
        enemy.rb.AddForce(toPlayer * chargeForce, ForceMode.Impulse);

        yield return new WaitForSeconds(1.5f); // wait for landing/impact

        // Reset enemy
        enemy.rb.velocity = Vector3.zero;
        enemy.rb.angularVelocity = Vector3.zero;
        enemy.rb.isKinematic = true;
        enemy.agent.enabled = true;

        enemy.alreadyAttacked = true;
        enemy.Invoke(nameof(enemy.ResetAttack), enemy.timeBetweenAttacks);
        enemy.attackCooldown = 1.5f;

        enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }

 
}

