using System.Collections;
using UnityEngine;
using FMODUnity;

public class ChargeAttackState : IState
{
    private const string ChargeSound = "event:/Bug/bug hurt with scream";

    public void Enter(EnemyAI enemy)
    {
        enemy.agent.SetDestination(enemy.transform.position);
        enemy.isCharging = true;
        enemy.isInCombat = true;
        enemy.ChangeStateCoroutine(ChargeAndSmash(enemy));
    }

    public void Update(EnemyAI enemy) { }

    public void Exit(EnemyAI enemy)
    {
        enemy.isCharging = false;
        enemy.isInCombat = false;
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    {
        // Transitions are handled by the coroutine
    }

    private IEnumerator ChargeAndSmash(EnemyAI enemy)
    {
        RuntimeManager.PlayOneShot(ChargeSound, Camera.main.transform.position);

        if (enemy.stamina < enemy.data.staminaDrainPerCharge)
        {
            enemy.ChangeState(new TransitionState(0.5f, new RetreatState()));
            yield break;
        }

        Vector3 targetPosition = enemy.player.position;
        enemy.transform.LookAt(targetPosition);

        // Disable NavMesh agent and lift the enemy slightly (hover effect)
        enemy.agent.enabled = false;
        enemy.rb.isKinematic = true;

        float liftDuration = 0.4f;
        float startY  = enemy.transform.position.y;
        float targetY = startY + 1f;
        float timer   = 0f;

        while (timer < liftDuration)
        {
            float newY = Mathf.Lerp(startY, targetY, timer / liftDuration);
            Vector3 pos = enemy.transform.position;
            enemy.transform.position = new Vector3(pos.x, newY, pos.z);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // suspense pause

        enemy.stamina = Mathf.Clamp(enemy.stamina - enemy.data.staminaDrainPerCharge, 0f, 100f);

        // Physics-driven charge toward the player's last position
        enemy.rb.isKinematic = false;
        Vector3 toPlayer = (targetPosition - enemy.transform.position).normalized;
        toPlayer.y = -0.1f; // slight downward angle for impact
        toPlayer.Normalize();

        enemy.rb.AddForce(toPlayer * (enemy.data.jumpForce * enemy.rb.mass), ForceMode.Impulse);

        yield return new WaitForSeconds(1.5f);

        // Restore state
        enemy.rb.linearVelocity  = Vector3.zero;
        enemy.rb.angularVelocity = Vector3.zero;
        enemy.rb.isKinematic     = true;
        enemy.agent.enabled      = true;

        enemy.alreadyAttacked = true;
        enemy.Invoke(nameof(enemy.ResetAttack), enemy.data.timeBetweenAttacks);
        enemy.attackCooldown = 1.5f;

        enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }
}
