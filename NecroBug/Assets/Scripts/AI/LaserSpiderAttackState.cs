using System.Collections;
using UnityEngine;

public class LaserSpiderAttackState : IState
{

    private float turnRateDegPerSec = 220f;
    private float aimOffsetY = 0f;
    private float yawVel;          
    private float lookLag = 0.5f; 
    private float maxYawSpeed = 200f;
    public void Enter(EnemyAI enemy)
    {
        enemy.agent.SetDestination(enemy.transform.position);
        enemy.isInCombat = true;
        enemy.ChangeStateCoroutine(ShootLaser(enemy));
    }

    public void Exit(EnemyAI enemy)
    {
        enemy.isInCombat = true;
        enemy.spiderLaser.activated = false;
    }

    public void Update(EnemyAI enemy)
    {

        Vector3 targetPos = enemy.player.position;
        targetPos.y += aimOffsetY;

        Vector3 to = targetPos - enemy.transform.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.0001f) return;

        float targetYaw = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;

        Vector3 e = enemy.transform.eulerAngles;
        float step = turnRateDegPerSec * Time.deltaTime;
        float newYaw = Mathf.SmoothDampAngle(e.y, targetYaw, ref yawVel, lookLag, maxYawSpeed, Time.deltaTime);
        enemy.transform.rotation = Quaternion.Euler(e.x, newYaw, e.z);
    }

    private IEnumerator ShootLaser(EnemyAI enemy)
    {
        enemy.transform.LookAt(enemy.player);
        float tiltDuration = 0.72f;
        float elapsed = 0f;
        Quaternion startRot = enemy.transform.rotation;
        Quaternion targetRot = Quaternion.Euler(
            -18f,
            startRot.eulerAngles.y,
            startRot.eulerAngles.z
        );

        while (elapsed < tiltDuration)
        {
            elapsed += Time.deltaTime;
            enemy.transform.rotation = Quaternion.Lerp(startRot, targetRot, elapsed / tiltDuration);
            yield return null;
        }

        float resetDuration = 0.28f;
        elapsed = 0f;
        startRot = enemy.transform.rotation;
        targetRot = Quaternion.Euler(
            0f,
            startRot.eulerAngles.y,
            startRot.eulerAngles.z
        );

        while (elapsed < resetDuration)
        {
            elapsed += Time.deltaTime;
            enemy.transform.rotation = Quaternion.Lerp(startRot, targetRot, elapsed / resetDuration);
            yield return null;
        }
        enemy.spiderLaser.activated = true;
        yield return new WaitForSeconds(2f);
        enemy.spiderLaser.activated = true;
        enemy.ChangeState(new TransitionState(0.5f, new PatrolState()));
    }

    public void CheckTransitions(EnemyAI enemy, bool playerInSightRange, bool playerInAttackRange, float distance)
    { }
}
