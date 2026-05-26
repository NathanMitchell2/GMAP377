using UnityEngine;

/// <summary>
/// Identifies which enemy archetype an EnemyAI belongs to.
/// Defined here so both EnemyData and EnemyAI can share it without nesting.
/// </summary>
public enum EnemyType { JetBeetle, AcidBeetle, OrbWeaver, Bee, LaserSpider }

/// <summary>
/// All tunable, read-only configuration for one enemy type.
/// Create one asset per enemy archetype via Assets > Create > NecroBug > Enemy Data,
/// then assign it to the EnemyAI component on the prefab.
/// </summary>
[CreateAssetMenu(fileName = "NewEnemyData", menuName = "NecroBug/Enemy Data")]
public class EnemyData : ScriptableObject
{
    // ========== IDENTITY ==========
    public EnemyType enemyType;

    // ========== DETECTION ==========
    [Header("Detection")]
    public float fieldOfView = 90f;
    public float viewDistance = 20f;
    public LayerMask visionObstacles;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    // ========== RANGES ==========
    [Header("Ranges")]
    public float sightRange = 15f;
    public float attackRange = 5f;
    public float retreatRange = 8f;

    // ========== MOVEMENT & PATROL ==========
    [Header("Movement & Patrol")]
    public float walkPointRange = 10f;
    public float idleDuration = 2f;

    // ========== COMBAT TIMING ==========
    [Header("Combat Timing")]
    public float timeBetweenAttacks = 2f;
    public float chargeUpTime = 0.5f;
    public float damageCooldown = 0.5f;
    public float maxPlayerSpeedCharge = 10f;

    // ========== STAMINA ==========
    [Header("Stamina")]
    public float staminaDrainPerCharge = 30f;
    public float staminaRecoverRate = 10f;

    // ========== JET BEETLE ==========
    [Header("Jet Beetle")]
    public int chargeDamage = 20;
    public float jumpForce = 15f;

    // ========== ACID BEETLE ==========
    [Header("Acid Beetle")]
    public GameObject acidProjectilePrefab;
    public float acidSpitForce = 20f;

    // ========== ORB WEAVER ==========
    [Header("Orb Weaver")]
    public int maxWebStacks = 3;
    public float webProjectileSpeed = 20f;
    public int biteDamage = 15;
    public GameObject webProjectilePrefab;

    // ========== BEE / SWARM ==========
    [Header("Bee / Swarm")]
    public int swarmMaxMembers = 6;
    public float swarmRadius = 3.5f;
    public float swarmReformLerp = 8f;
    public float swarmAttackCooldown = 1.2f;

    [Header("Bee Dive Attack")]
    public float diveWindup = 0.15f;
    public float diveSpeed = 18f;
    public float diveArcHeight = 1.0f;
    public float diveHitRadius = 0.6f;
    public int diveDamage = 12;
    public LayerMask robotMask;
    public LayerMask groundMask;
    public GameObject leaderExplosion;
    public GameObject soldierExplosion;
}
