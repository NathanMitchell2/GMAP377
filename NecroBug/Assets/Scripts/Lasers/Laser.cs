using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Laser : MonoBehaviour
{
    public bool activated = true;
    public bool playerLaser = false;          // set TRUE on the player’s laser

    [Header("Raycast")]
    public LayerMask hitMask = ~0;            // set to exclude your own layer if needed

    [Header("VFX")]
    public ParticleSystem chargeParticles;
    public ParticleSystem sparkleParticle;
    [SerializeField] LaserRendererSettings laserRendererSettings;

    [Header("Damage (tick-based)")]
    [Tooltip("Damage applied each tick while the beam is on a valid target.")]
    public int damagePerTick = 2;
    [Tooltip("Time between ticks while the beam is on a valid target.")]
    public float tickInterval = 0.1f;

    [Header("Audio")]
    [SerializeField] public string laserSound = "event:/Spider/Spider charge and Laser";

    // internals
    private LineRenderer lineRenderer;
    private Vector3 sourcePosition;
    private const float farDistance = 1000f;
    private List<Vector3> bouncePositions;
    private int maxBounces = 100;
    private LaserSensor prevStruckLaserSensor = null;

    // per-target throttle (works for PlayerHealth, EnemyHealth, WallHealth)
    private readonly Dictionary<UnityEngine.Object, float> _nextTickAt = new();

    // Optional: use DamageObject only for enemy lasers hitting the Player
    private DamageObject _damageObject;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (!lineRenderer) lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = true;

        if (laserRendererSettings) laserRendererSettings.Apply(lineRenderer);

        _damageObject = GetComponent<DamageObject>();

        sourcePosition = transform.position + transform.forward * 0.2501f;
        bouncePositions = new List<Vector3>();
    }

    private void FixedUpdate()
    {
        if (!activated)
        {
            lineRenderer.positionCount = 0;
            if (prevStruckLaserSensor != null)
            {
                LaserSensor.HandleLaser(this, prevStruckLaserSensor, null);
                prevStruckLaserSensor = null;
            }
            _nextTickAt.Clear();
            return;
        }

        sourcePosition = transform.position + transform.forward * 0.2501f;
        bouncePositions.Clear();
        bouncePositions.Add(sourcePosition);

        CastBeam(sourcePosition, transform.forward);

        lineRenderer.positionCount = bouncePositions.Count;
        lineRenderer.SetPositions(bouncePositions.ToArray());
    }

    public void CastBeam(Vector3 origin, Vector3 direction)
    {
        if (bouncePositions.Count > maxBounces) return;

        var ray = new Ray(origin, direction);
        if (!Physics.Raycast(ray, out var hitInfo, farDistance, hitMask, QueryTriggerInteraction.Ignore))
        {
            bouncePositions.Add(origin + direction * farDistance);
            if (prevStruckLaserSensor != null)
            {
                LaserSensor.HandleLaser(this, prevStruckLaserSensor, null);
                prevStruckLaserSensor = null;
            }
            return;
        }

        bouncePositions.Add(hitInfo.point);

        var reflectiveObject = hitInfo.collider.GetComponent<ILaserReflective>();
        if (reflectiveObject != null)
        {
            reflectiveObject.Reflect(this, ray, hitInfo);
            return;
        }

        if (playerLaser)
        {
            var eHealth = hitInfo.collider.GetComponentInParent<EnemyHealth>();
            if (eHealth != null)
            {
                if (!_nextTickAt.TryGetValue(eHealth, out float nextT) || Time.time >= nextT)
                {
                    eHealth.dealDamage(damagePerTick);
                    _nextTickAt[eHealth] = Time.time + tickInterval;
                }
            }

            var wHealth = hitInfo.collider.GetComponentInParent<WallHealth>();
            if (wHealth != null)
            {
                if (!_nextTickAt.TryGetValue(wHealth, out float nextTw) || Time.time >= nextTw)
                {
                    wHealth.dealDamage(damagePerTick);
                    _nextTickAt[wHealth] = Time.time + tickInterval;
                }
            }
        }
        else
        {
            var pHealth = hitInfo.collider.GetComponentInParent<PlayerHealth>(); 
            if (pHealth != null) { 
                if (!_nextTickAt.TryGetValue(pHealth, out float nextT) || Time.time >= nextT) { 
                    DamageObject.Damage(damagePerTick, pHealth); 
                    _nextTickAt[pHealth] = Time.time + tickInterval; 
                } 
            }
        }

        var currentLaserSensor = hitInfo.collider.GetComponent<LaserSensor>();
        if (currentLaserSensor != prevStruckLaserSensor)
        {
            LaserSensor.HandleLaser(this, prevStruckLaserSensor, currentLaserSensor);
            prevStruckLaserSensor = currentLaserSensor;
        }
    }

    public void ToggleLaser() => activated = !activated;

    public void PlayerShoot() { StartCoroutine(ActivateLaser()); }

    private IEnumerator ActivateLaser()
    {
        RuntimeManager.PlayOneShot(laserSound, Camera.main.transform.position);

        if (activated) { activated = false; yield break; }

        if (chargeParticles) chargeParticles.Play();
        float chargeDur = chargeParticles ? chargeParticles.main.duration : 0f;
        yield return new WaitForSeconds(chargeDur);

        if (sparkleParticle) sparkleParticle.Play();
        yield return new WaitForSeconds(0.1f);

        activated = true;
        yield return new WaitForSeconds(0.5f);
        activated = false;
    }
}
