using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Laser : MonoBehaviour
{
    public bool activated = true;
    public bool playerLaser = false;

    public ParticleSystem chargeParticles;
    public ParticleSystem sparkleParticle;

    LineRenderer lineRenderer;
    [SerializeField] LaserRendererSettings laserRendererSettings;

    Vector3 sourcePosition;
    const float farDistance = 1000f;
    List<Vector3> bouncePositions;
    int maxBounces = 100;

    LaserSensor prevStruckLaserSensor = null;

    [SerializeField]
    public string laserSound = "event:/Spider/Spider charge and Laser";

    private void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        laserRendererSettings.Apply(lineRenderer);
        sourcePosition = transform.position + transform.forward * 0.2501f;
        bouncePositions = new List<Vector3>();
    }

    public void Update()
    {
        // if (Input.GetMouseButtonDown(0) && playerLaser) { StartCoroutine(ActivateLaser()); }
    }

    private void FixedUpdate()
    {
        if (!activated)
        {
            // Debug.Log("not activated");
            lineRenderer.positionCount = 0;
            if (prevStruckLaserSensor != null)
            {
                LaserSensor.HandleLaser(this, prevStruckLaserSensor, null);
                prevStruckLaserSensor = null;
            }
            return;
        }
        sourcePosition = transform.position + transform.forward * 0.2501f;
        bouncePositions = new List<Vector3>() { sourcePosition };
        // Debug.Log(bouncePositions.Count);

        CastBeam(sourcePosition, transform.forward);

        lineRenderer.positionCount = bouncePositions.Count;
        lineRenderer.SetPositions(bouncePositions.ToArray());
    }

    public void CastBeam(Vector3 origin, Vector3 direction)
    {
        if (bouncePositions.Count > maxBounces)
            return;

        var ray = new Ray(origin, direction);

        bool didHit = Physics.Raycast(ray, out RaycastHit hitInfo, farDistance);
        
        RuntimeManager.PlayOneShot(laserSound, Camera.main.transform.position);
        Debug.Log("laser sound played");
        if (!didHit)
        {
            var endPoint = origin + direction * farDistance;
            bouncePositions.Add(endPoint);
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
            reflectiveObject.Reflect(this, ray, hitInfo);

        else
        {
            var currentLaserSensor = hitInfo.collider.GetComponent<LaserSensor>();
            if (currentLaserSensor != prevStruckLaserSensor)
            {
                Debug.Log("sensed");
                LaserSensor.HandleLaser(this, prevStruckLaserSensor, currentLaserSensor);
                prevStruckLaserSensor = currentLaserSensor;
            }
        }
    }

    public void ToggleLaser()
    {
        if (activated) { activated = false; }
        else if (!activated) { activated=true; }
    }

    public void PlayerShoot()
    {
        StartCoroutine(ActivateLaser());
    }

    private IEnumerator ActivateLaser()
    {
        // Debug.Log("laser shooting");
        if (this.activated)
        {
            this.activated = false;
            yield break;
        }

        if (chargeParticles != null)
        {
            chargeParticles.Play();
        }
        yield return new WaitForSeconds(chargeParticles.main.duration);

        sparkleParticle.Play();

        yield return new WaitForSeconds(0.1f);

        this.activated = true;

        yield return new WaitForSeconds(0.5f);

        this.activated = false;

    }

}
