using UnityEngine;

public class PlayerShoot : ModularBugPart
{
    public GameObject projectilePrefab;
    public float shootForce = 20f;
    public Transform firePoint;

    public EnemyAI enemy;
    public GameObject target;
    public LineRenderer lineRenderer;

    private float timer;
    public float shootCooldown;

    void Update()
    {
        timer += 1 * Time.deltaTime;
    }

    void Shoot()
    {
        Vector3 targetPos = target.transform.position;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Calculate trajectory
            Vector3 parabola = CalculateArcVelocity(firePoint.position, targetPos, 0.5f, 0.1f, Physics.gravity.y);

            // Render path with line
            RenderTrajectory(lineRenderer, firePoint.position, parabola, Physics.gravity.y);
            rb.linearDamping = 0f;
            rb.angularDamping = 0f;
            rb.linearVelocity = parabola;
        }

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

    public override void Activate()
    {
        if (timer > shootCooldown)
        {
            Shoot();
            timer = 0;
        }
    }
}
