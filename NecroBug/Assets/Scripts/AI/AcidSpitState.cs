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
        Vector3 dir = (targetPos - enemy.spitPoint.position).normalized;
        rb.AddForce(dir * enemy.acidSpitForce, ForceMode.Impulse);

        // Cooldown
        enemy.alreadyAttacked = true;
        enemy.Invoke(nameof(enemy.ResetAttack), enemy.timeBetweenAttacks);
        enemy.attackCooldown = 1f;

        yield return new WaitForSeconds(1f); // let the projectile travel

        enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }
}
