using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float sightRange, attackRange, retreatRange;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float idleDuration;
    public float chargeUpTime;
    public float jumpForce;
    public float maxPlayerSpeedCharge;

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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lastPlayerPosition = player.position;
    }

    private void Start()
    {
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
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        float distance = Vector3.Distance(transform.position, player.position);
        playerSpeed = (player.position - lastPlayerPosition).magnitude / Time.deltaTime;

        currentState.CheckTransitions(this, playerInSightRange, playerInAttackRange, distance);
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
}