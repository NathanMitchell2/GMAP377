using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool activated = true;
    public bool playerLaser = false;

    LineRenderer lineRenderer;
    [SerializeField] LaserRendererSettings laserRendererSettings;

    Vector3 sourcePosition;
    const float farDistance = 1000f;
    List<Vector3> bouncePositions;
    int maxBounces = 100;

    LaserSensor prevStruckLaserSensor = null;

    private void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        laserRendererSettings.Apply(lineRenderer);
        sourcePosition = transform.position + transform.forward * 0.2501f;
        bouncePositions = new List<Vector3>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerLaser) { ToggleLaser(); }
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
    
}
