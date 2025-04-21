using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;

    // Patrol
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    // Attack
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    // States & Detection
    public float sightRange, attackRange, retreatRange;
    public bool playerInSightRange, playerInAttackRange;

    private Vector3 lastPlayerPosition;
    private float playerSpeed;

    // Charging
    public float chargeUpTime = 1.5f;
    private bool isCharging = false;

    // Idle
    private float idleTimer;
    public float idleDuration = 3f;

    // Rigidbody
    private Rigidbody rb;

    enum State { Idle, Patrol, Chase, Retreat, ChargeAttack }
    State currentState;

    private void Awake()
    { 
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lastPlayerPosition = player.position;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        float distance = Vector3.Distance(transform.position, player.position);
        playerSpeed = (player.position - lastPlayerPosition).magnitude / Time.deltaTime;

        // If player is gone from all ranges → patrol
        if (!playerInSightRange && !playerInAttackRange && distance > retreatRange)
        {
            if (currentState != State.Patrol && currentState != State.Idle)
            {
                SetState(State.Patrol);
            }
        }
        else
        {
            // Transition to states based on priority
            if (distance < retreatRange)
            {
                SetState(State.Retreat);
            }
            else if (playerInAttackRange && playerSpeed < 1f)
            {
                SetState(State.ChargeAttack);
            }
            else if (playerInSightRange)
            {
                SetState(State.Chase);
            }
        }

        // Execute current state
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrol:
                Patroling();
                break;
            case State.Chase:
                ChasePlayer();
                break;
            case State.Retreat:
                RetreatFromPlayer();
                break;
            case State.ChargeAttack:
                ChargeAttack();
                break;
        }

        lastPlayerPosition = player.position;
    }
    private void SetState(State newState)
    {
        if (currentState != newState)
        {
            idleTimer = 0f;
            walkPointSet = false;
            currentState = newState;
        }
    }
    private void Idle()
    {
        idleTimer += Time.deltaTime;
        agent.SetDestination(transform.position);
        if (idleTimer >= idleDuration)
        {
            idleTimer = 0f;
            walkPointSet = false;
            currentState = State.Patrol;
        }
    }
    
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
                SetState(State.Idle); // ← transition here only
            }
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX,
                                transform.position.y,
                                transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void RetreatFromPlayer()
    {
        Vector3 retreatDirection = (transform.position - player.position).normalized;
        Vector3 retreatPosition = transform.position + retreatDirection * 5f;
        agent.SetDestination(retreatPosition);
    }

    private void ChargeAttack()
    {
        if (!alreadyAttacked && !isCharging)
            StartCoroutine(ChargeAndSmash());
    }

    private IEnumerator ChargeAndSmash()
    {
        isCharging = true;
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        yield return new WaitForSeconds(chargeUpTime);

        // If player left attack range, cancel
        if (!playerInAttackRange)
        {
            isCharging = false;
            SetState(State.Patrol);
            yield break;
        }

        Vector3 jumpDirection = (player.position - transform.position).normalized;
        float jumpForce = 15f;

        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(jumpDirection * jumpForce*1600 + Vector3.up * 3200f, ForceMode.Impulse);

        yield return new WaitForSeconds(1.5f);

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        agent.enabled = true;

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        isCharging = false;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
