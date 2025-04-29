using System.Collections;
using UnityEngine;

public class ChargeAttackState : IState
{
    private bool isChargingStarted = false;

    public void Enter(EnemyAI enemy)
    {
        if (!isChargingStarted)
        {
            enemy.agent.SetDestination(enemy.transform.position); // stop moving
            enemy.isCharging = true;
            enemy.ChangeStateCoroutine(ChargeAndSmash(enemy));
        }
    }

    public void Update(EnemyAI enemy) { }

    public void Exit(EnemyAI enemy)
    {
        enemy.isCharging = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        // Transitions handled inside coroutine after smash
    }

    private IEnumerator ChargeAndSmash(EnemyAI enemy)
    {
        isChargingStarted = true;
        enemy.transform.LookAt(enemy.player);
        Vector3 targetPosition = enemy.player.position;

        yield return new WaitForSeconds(enemy.chargeUpTime);

        if (!Physics.CheckSphere(enemy.transform.position, enemy.attackRange, enemy.whatIsPlayer))
        {
            enemy.ChangeState(new PatrolState());
            yield break;
        }

        Vector3 jumpDirection = (targetPosition - enemy.transform.position).normalized;

        enemy.agent.enabled = false;
        enemy.rb.isKinematic = false;
        enemy.rb.AddForce(jumpDirection * enemy.jumpForce * enemy.rb.mass + Vector3.up * enemy.rb.mass*2, ForceMode.Impulse);

        yield return new WaitForSeconds(1.5f);

        enemy.rb.linearVelocity = Vector3.zero;
        enemy.rb.isKinematic = true;
        enemy.agent.enabled = true;

        enemy.alreadyAttacked = true;
        enemy.Invoke(nameof(enemy.ResetAttack), enemy.timeBetweenAttacks);

        enemy.ChangeState(new PatrolState());
    }
}

