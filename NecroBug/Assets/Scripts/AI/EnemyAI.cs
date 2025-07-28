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
    public enum EnemyType { JetBeetle, AcidBeetle, OrbWeaver }
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
    public bool wasRecentlyHit = false;
    public int biteDamage = 15;
    public GameObject webProjectilePrefab;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lastPlayerPosition = player.position;
        patrolCenter = gameObject.GetComponent<Transform>().position;
        // Debug.Log(Time.time);
    }

    private void Start()
    {
        patrolCenter = transform.position;

        switch (enemyType)
        {
            case EnemyType.OrbWeaver:
                ChangeState(new PatrolState());
                break;
            case EnemyType.AcidBeetle:
            case EnemyType.JetBeetle:
            default:
                ChangeState(new IdleState());
                break;
        }
    }

    private void Update()
    {
        RegenerateStamina();

        if (attackCooldown > 0f)
            attackCooldown -= Time.deltaTime;

        UpdatePlayerDetection();
        currentState.Update(this);
        lastPlayerPosition = player.position;

    }

    public IState GetAttackState()
    {
        switch (enemyType)
        {
            case EnemyType.AcidBeetle:
                return new AcidSpitState();
            case EnemyType.JetBeetle:
            default:
                return new ChargeAttackState();
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit(this);

        currentState = newState;

        if (currentState != null)
            currentState.Enter(this);
    }

    private void UpdatePlayerDetection()
    {
        playerSpeed = (player.position - lastPlayerPosition).magnitude / Time.deltaTime;
        lastPlayerPosition = player.position;

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
            {
                inView = true;
            }
        }

        playerInSightRange = inView;
        playerInAttackRange = distanceToPlayer <= attackRange;

        currentState.CheckTransitions(this, playerInSightRange, playerInAttackRange, distanceToPlayer);
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
    public Coroutine ChangeStateCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
    private void RegenerateStamina()
    {
        if (isInCombat) return;

        if (stamina < 100f)
        {
            stamina += staminaRecoverRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }
    }
    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Added by Patrick, damage system

    // Jet Beetle Damage
    void OnCollisionEnter(Collision other)
    {
        if (Time.time - lastHitTime < damageCooldown) return;

        if (other.gameObject.CompareTag("Player") && isCharging)
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(other.gameObject);

            GetComponent<DamageObject>().Damage(DamageObject.GetPlayerHealths(list));
            /*
            playerStats = other.gameObject.GetComponentInParent<PlayerStats>();

            if (pHealth != null)
            {

                DealDamage(chargeDamage);
            }
            else
            {
                Debug.LogWarning("PlayerStats not found on object!");
            }
            */
        }
    }

    public void DealAcidDamage(int damageAmount, PlayerStats otherPlayer)
    {
        if (Time.time - lastHitTime < damageCooldown) return;

        playerStats = otherPlayer;
        if (playerStats != null)
        {
            DealDamage(damageAmount);
        }
        else { Debug.LogWarning("PlayerStats not found on object!"); }
    }

    public void DealDamage(int damageAmount)
    {
        lastHitTime = Time.time;

        // Signal retreat behavior if orb weaver is in attack state
        if (enemyType == EnemyType.OrbWeaver && currentState is OrbWeaverAgroState)
        {
            wasRecentlyHit = true;
        }

        // Apply damage to the player
        if (playerStats != null)
        {
            playerStats.TakeDamage(damageAmount);
        }
        else
        {
            Debug.LogWarning("PlayerStats not assigned!");
        }
    }

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

        // Reset state machine
        ChangeState(new PatrolState());
    }
}