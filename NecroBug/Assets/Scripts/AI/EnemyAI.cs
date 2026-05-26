using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Core enemy brain. Drives a finite state machine and owns all runtime state.
/// Tunable configuration lives in the assigned <see cref="EnemyData"/> ScriptableObject,
/// keeping this component focused on behaviour rather than data.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    // ========== CONFIGURATION ==========
    public EnemyData data;

    // ========== COMPONENT REFERENCES ==========
    [Header("Component References")]
    [SerializeField] private string currentStateName;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Transform player;
    public PlayerStats playerStats;
    public LineRenderer lineRenderer;
    public Transform spitPoint;
    public carControler carController;

    // ========== LASER SPIDER COMPONENTS ==========
    [Header("Laser Spider")]
    public Laser spiderLaser;
    public ParticleSystem laserCharge;
    public ParticleSystem laserSparkle;

    // ========== PATROL STATE ==========
    [Header("Patrol State")]
    public Vector3 patrolCenter;
    public Vector3 walkPoint;
    public bool walkPointSet;

    // ========== COMBAT STATE ==========
    [Header("Combat State")]
    public bool alreadyAttacked;
    public bool isCharging;
    public bool isInCombat;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    public float playerSpeed;
    public float attackCooldown;
    public bool wasRecentlyHit;
    public bool angered;

    // ========== STAMINA (runtime) ==========
    [Header("Runtime Stamina")]
    public float stamina = 100f;
    public bool isExhausted => stamina <= 0f;

    // ========== ORB WEAVER RUNTIME STATE ==========
    [Header("Orb Weaver")]
    public int webStack;

    // ========== AI CONTROL ==========
    [Header("AI Control")]
    private IState currentState;
    public Vector3 lastPlayerPosition;

    // ========== SWARM STATE ==========
    [Header("Swarm State")]
    public bool isSwarmLeader { get; private set; }

    // ========= AWAKE & START =========
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lastPlayerPosition = player.position;
        patrolCenter = transform.position;
    }

    private void Start()
    {
        player = PlayerIdentifier.GetPlayer().transform.Find("Center");
        ChangeState(new PatrolState());
    }

    // ========= UPDATE LOOP =========
    private void Update()
    {
        currentStateName = currentState?.GetType().Name ?? "null";
        RegenerateStamina();

        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;

        UpdatePlayerDetection();
        currentState.Update(this);
        lastPlayerPosition = player.position;
    }

    // ========= DETECTION =========
    private void UpdatePlayerDetection()
    {
        playerSpeed = (player.position - lastPlayerPosition).magnitude / Time.deltaTime;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        bool inFOV = angleToPlayer < data.fieldOfView / 2f && distanceToPlayer <= data.viewDistance;
        bool inView = false;

        if (inFOV)
        {
            Vector3 start = transform.position + Vector3.up * 1.5f;
            Vector3 end = player.position + Vector3.up * 1.5f;
            Vector3 rayDir = (end - start).normalized;

            if (!Physics.SphereCast(start, 0.5f, rayDir, out RaycastHit hit, distanceToPlayer, data.visionObstacles))
                inView = true;
        }

        playerInSightRange = inView;
        playerInAttackRange = distanceToPlayer <= data.attackRange;

        currentState.CheckTransitions(this, playerInSightRange, playerInAttackRange, distanceToPlayer);
    }

    // ========= STATE MANAGEMENT =========
    public void ChangeState(IState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public Coroutine ChangeStateCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    // ========= GET ATTACK STATE =========
    public IState GetAttackState()
    {
        return data.enemyType switch
        {
            EnemyType.OrbWeaver   => new OrbWeaverAttackState(),
            EnemyType.AcidBeetle  => new AcidSpitState(),
            EnemyType.JetBeetle   => new ChargeAttackState(),
            EnemyType.Bee         => new SwarmAttackState(),
            EnemyType.LaserSpider => new LaserSpiderAttackState(),
            _                     => new ChargeAttackState(),
        };
    }

    // ========= STAMINA =========
    private void RegenerateStamina()
    {
        if (isInCombat) return;
        stamina = Mathf.Min(100f, stamina + data.staminaRecoverRate * Time.deltaTime);
    }

    // ========= DAMAGE HANDLING =========
    private float lastHitTime = 0.5f;

    private void OnCollisionEnter(Collision other)
    {
        if (Time.time - lastHitTime < data.damageCooldown) return;
        if (other.gameObject.CompareTag("Player") && isCharging)
        {
            var list = new List<GameObject> { other.collider.gameObject };
            GetComponent<DamageObject>().Damage(DamageObject.GetPlayerHealths(list));
        }
    }

    public void DealAcidDamage(int damageAmount, PlayerStats otherPlayer)
    {
        if (Time.time - lastHitTime < data.damageCooldown) return;
        playerStats = otherPlayer;
        if (playerStats != null)
            DealDamage(damageAmount);
        else
            Debug.LogWarning("PlayerStats not found on object!");
    }

    public void DealDamage(int damageAmount)
    {
        lastHitTime = Time.time;
        if (data.enemyType == EnemyType.OrbWeaver && currentState is OrbWeaverAgroState)
            wasRecentlyHit = true;
        if (playerStats != null)
            playerStats.TakeDamage(damageAmount);
        else
            Debug.LogWarning("PlayerStats not assigned!");
    }

    // ========= RESET ATTACK =========
    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // ========= RESET AI =========
    public void ResetAI()
    {
        alreadyAttacked = false;
        isCharging = false;
        isInCombat = false;
        walkPointSet = false;
        stamina = 100f;
        attackCooldown = 0f;
        agent.enabled = true;
        rb.isKinematic = true;
        ChangeState(new PatrolState());
    }

    public void SetAgentEnabled(bool on)
    {
        if (agent == null) return;
        if (agent.enabled == on) return;
        if (!on) agent.ResetPath();
        agent.enabled = on;
    }

    public void SetSwarmLeader(bool on) => isSwarmLeader = on;

    public float GetSpeed() => agent.speed;

    private void OnDrawGizmosSelected()
    {
        if (data == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.viewDistance);
        Vector3 leftLimit  = Quaternion.Euler(0, -data.fieldOfView / 2f, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0,  data.fieldOfView / 2f, 0) * transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, leftLimit  * data.viewDistance);
        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, rightLimit * data.viewDistance);
    }
}
