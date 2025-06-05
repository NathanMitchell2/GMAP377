using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float fieldOfView;
    public float viewDistance;
    public int rayCount;
    public LayerMask visionObstacles;
    public float noiseDetectionSpeed;
    public float sightRange, attackRange, retreatRange;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float idleDuration;
    public float chargeUpTime;
    public float jumpForce;
    public float maxPlayerSpeedCharge;

    public Vector3 patrolCenter;
    public Vector3 walkPoint;
    public bool walkPointSet;
    public bool alreadyAttacked;
    public bool isCharging;
    public bool isInCombat = false;
    public Vector3 lastPlayerPosition;
    public float playerSpeed;

    private IState currentState;

    public bool playerInSightRange, playerInAttackRange;

    public float stamina = 100f;
    public float staminaDrainPerCharge = 30f;
    public float staminaRecoverRate = 10f;
    public bool isExhausted => stamina <= 0f;
    public float attackCooldown = 0f;

    public enum EnemyType { JetBeetle, AcidBeetle }
    public EnemyType enemyType;


    public Transform spitPoint;
    public GameObject acidProjectilePrefab;
    public float acidSpitForce = 20f;

    public PlayerStats playerStats;
    public int chargeDamage = 20;
    // public int acidDamage = 10;
    public float damageCooldown = 0.5f;
    private float lastHitTime = 0.5f;


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
        patrolCenter = gameObject.GetComponent<Transform>().position;
        ChangeState(new IdleState());
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
            playerStats = other.gameObject.GetComponentInParent<PlayerStats>();

            if (playerStats != null)
            {
                DealDamage(chargeDamage);
            }
            else
            {
                Debug.LogWarning("PlayerStats not found on object!");
            }
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
        // Debug.Log(damageAmount);
        playerStats.TakeDamage(damageAmount);
        lastHitTime = Time.time;
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