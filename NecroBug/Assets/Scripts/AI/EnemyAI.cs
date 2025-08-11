using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{   
    // ========== COMPONENT REFERENCES ==========
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Transform player;
    public PlayerStats playerStats;
    public LineRenderer lineRenderer;
    public Transform spitPoint;
    public carControler carController;

    // ========== LAYER DETECTION ==========
    public LayerMask whatIsGround, whatIsPlayer;
    public LayerMask visionObstacles;

    // ========== GENERAL DETECTION & BEHAVIOR ==========
    public float fieldOfView;
    public float viewDistance;
    public float sightRange, attackRange, retreatRange;
    public float noiseDetectionSpeed;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float idleDuration;
    public float chargeUpTime;
    public float jumpForce;
    public float maxPlayerSpeedCharge;
    public int rayCount;

    // ========== PATROLLING ==========
    public Vector3 patrolCenter;
    public Vector3 walkPoint;
    public bool walkPointSet;

    // ========== COMBAT STATE ==========
    public bool alreadyAttacked;
    public bool isCharging;
    public bool isInCombat = false;
    public bool playerInSightRange, playerInAttackRange;
    public float playerSpeed;
    public float attackCooldown = 0f;
    public float damageCooldown = 0.5f;
    private float lastHitTime = 0.5f;

    // ========== STAMINA SYSTEM ==========
    public float stamina = 100f;
    public float staminaDrainPerCharge = 30f;
    public float staminaRecoverRate = 10f;
    public bool isExhausted => stamina <= 0f;

    // ========== AI CONTROL ==========
    private IState currentState;
    public Vector3 lastPlayerPosition;

    // ========== ENEMY TYPE ==========
    public enum EnemyType { JetBeetle, AcidBeetle, OrbWeaver, Bee }
    public EnemyType enemyType;

    // ========== JET BEETLE STATS ==========
    [Header("Jet Beetle")]
    public int chargeDamage = 20;

    // ========== ACID BEETLE STATS ==========
    [Header("Acid Beetle")]
    public GameObject acidProjectilePrefab;
    public float acidSpitForce = 20f;

    // ========== ORB WEAVER STATS ==========
    [Header("Orb Weaver")]
    public int webStack = 0;
    public int maxWebStacks = 3;
    public float webProjectileSpeed = 20f;
    public bool wasRecentlyHit = false;
    public int biteDamage = 15;
    public GameObject webProjectilePrefab;

    // ========== SWARM/BEES STATS ==========
    [Header("Bee / Swarm")]
    public float swarmRecruitRange = 25f;      // how far leader can recruit members
    public int swarmMaxMembers = 6;            // cap, tweakable in Inspector
    public float swarmRadius = 3.5f;           // circle radius around leader
    public float swarmReformLerp = 8f;         // how snappy members hold formation
    public float swarmMemberSpeed = 6f;        // (if you ever use velocity move)
    public float swarmAttackCooldown = 1.2f;   // regroup time between attack orders
    public bool angered = false;

    [Header("Bee Flight / Formation")]
    public float hoverHeight = 2.0f;           // member flight height above ground
    public float hoverBobAmplitude = 0.15f;    // subtle bobbing
    public float hoverBobSpeed = 3.0f;

    [Header("Bee Dive Attack")]
    public float diveWindup = 0.15f;           // tiny delay before members dive
    public float diveSpeed = 18f;              // dive travel speed
    public float diveArcHeight = 1.0f;         // small lift at dive start
    public float diveHitRadius = 0.6f;         // sphere hit radius during dive
    public int diveDamage = 12;                // damage to robot on hit
    public LayerMask robotMask;                // set to your robot/player layer
    public LayerMask groundMask;               // set to ground layer

    [Header("Leader Orders (optional auto)")]
    public bool swarmAutoIssueOrders = true;   // auto-issue attack waves

    private EnemyHealth _health;
    private bool _promotedToSwarmLeader;

    // ========= AWAKE & START =========
    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lastPlayerPosition = player.position;
        patrolCenter = transform.position;
    }


    private void Start()
    {
        player = PlayerIdentifier.GetPlayer().transform.Find("Center");
        // Initialize state based on type
        switch (enemyType)
        {
            case EnemyType.OrbWeaver:
                ChangeState(new PatrolState());
                break;
            case EnemyType.AcidBeetle:
                ChangeState(new PatrolState());
                break;
            case EnemyType.JetBeetle:
                ChangeState(new PatrolState());
                break;
            case EnemyType.Bee:
                ChangeState(new PatrolState());
                break;
            default:
                ChangeState(new IdleState());
                break;
        }
    }

    public void SetAgentEnabled(bool on)
    {
        if (agent == null) return;
        if (agent.enabled == on) return;
        if (!on) agent.ResetPath();
        agent.enabled = on;
    }

    // ========= UPDATE LOOP =========
    private void Update()
    {
        //Debug.Log(currentState);
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

        bool inFOV = angleToPlayer < fieldOfView / 2f && distanceToPlayer <= viewDistance;
        bool inView = false;

        if (inFOV)
        {
            Vector3 start = transform.position + Vector3.up * 1.5f;
            Vector3 end = player.position + Vector3.up * 1.5f;
            Vector3 rayDir = (end - start).normalized;

            if (!Physics.SphereCast(start, 0.5f, rayDir, out RaycastHit hit, distanceToPlayer, visionObstacles))
                inView = true;
        }

        playerInSightRange = inView;
        playerInAttackRange = distanceToPlayer <= attackRange;

        if (enemyType == EnemyType.Bee && angered)
            ChangeState(new TransitionState(0.5f,new SwarmLeaderState()));

        currentState.CheckTransitions(this, playerInSightRange, playerInAttackRange, distanceToPlayer);
    }

    // ========= STATE MANAGEMENT =========
    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public Coroutine ChangeStateCoroutine(System.Collections.IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    // ========= GET ATTACK STATE =========
    public IState GetAttackState()
    {
        switch (enemyType)
        {
            case EnemyType.OrbWeaver:
                return new OrbWeaverAttackState();
            case EnemyType.AcidBeetle:
                return new AcidSpitState();
            case EnemyType.JetBeetle:
                return new ChargeAttackState();
            case EnemyType.Bee:
                return new SwarmLeaderState();
            default:
                return new ChargeAttackState();
        }
    }

    // ========= STAMINA =========
    private void RegenerateStamina()
    {
        if (isInCombat) return;
        if (stamina < 100f)
        {
            stamina += staminaRecoverRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }
    }

    // ========= DAMAGE HANDLING =========
    void OnCollisionEnter(Collision other)
    {
        if (Time.time - lastHitTime < damageCooldown) return;
        if (other.gameObject.CompareTag("Player") && isCharging)
        {
            var list = new List<GameObject> { other.collider.gameObject };
            GetComponent<DamageObject>().Damage(DamageObject.GetPlayerHealths(list));
        }
    }

    public void DealAcidDamage(int damageAmount, PlayerStats otherPlayer)
    {
        if (Time.time - lastHitTime < damageCooldown) return;
        playerStats = otherPlayer;
        if (playerStats != null)
            DealDamage(damageAmount);
        else
            Debug.LogWarning("PlayerStats not found on object!");
    }

    public void DealDamage(int damageAmount)
    {
        lastHitTime = Time.time;
        if (enemyType == EnemyType.OrbWeaver && currentState is OrbWeaverAgroState)
            wasRecentlyHit = true;
        if (playerStats != null)
            playerStats.TakeDamage(damageAmount);
        else
            Debug.LogWarning("PlayerStats not assigned!");
    }

    // ========= RESET TIPS =========
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, leftLimit * viewDistance);
        Gizmos.DrawRay(transform.position + Vector3.up * 1.5f, rightLimit * viewDistance);
    }

    public float getSpeed()
    {
        return this.GetComponent<NavMeshAgent>().speed;
    }

    public bool isSwarmLeader { get; private set; }

    public void SetSwarmLeader(bool on)
    {
        isSwarmLeader = on;
    }
}