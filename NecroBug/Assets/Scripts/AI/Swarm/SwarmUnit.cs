using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwarmUnit : MonoBehaviour
{
    private EnemyAI _leader;
    private readonly List<SwarmSoldier> _soldiers = new();

    // Per-soldier phase/jitter and a global orbit angle for smooth rotation
    private readonly Dictionary<SwarmSoldier, float> _angleJitter = new();
    private float _orbitAngle;

    public bool AllSoldiersDead => _soldiers.All(s => s == null);

    public void Bind(EnemyAI leader)
    {
        _leader = leader;
        _soldiers.Clear();
        _angleJitter.Clear();

        // Collect only objects named EXACTLY "Bee Soldier" (keeps your strict filter)
        // (Matches original selection logic):contentReference[oaicite:1]{index=1}
        var trs = leader.GetComponentsInChildren<Transform>(true);
        foreach (var tr in trs)
        {
            if (!tr || tr.gameObject == leader.gameObject) continue;
            if (!string.Equals(tr.name, "Bee Soldier", System.StringComparison.Ordinal)) continue;

            var s = tr.GetComponent<SwarmSoldier>();
            if (!s) s = tr.gameObject.AddComponent<SwarmSoldier>();
            s.Bind(leader);

            // Detach from leader so hierarchy/parent transforms don't make motion rigid
            // We retain the soldier's world position.
            s.transform.SetParent(null, true);

            _soldiers.Add(s);
            // Small per-soldier jitter to avoid perfect robotic spacing
            _angleJitter[s] = Random.Range(-0.35f, 0.35f);
        }

        // Start orbit neutral
        _orbitAngle = 0f;
    }

    public void TickFormation()
    {
        PurgeNulls();
        if (_soldiers.Count == 0 || _leader == null) return;

        // Smooth orbit around leader; tweak speed via leader.swarmReformLerp as a multiplier
        // If you prefer a dedicated field, add e.g. _leader.swarmOrbitSpeed.
        float orbitSpeed = Mathf.Max(0.2f, _leader.swarmReformLerp) * 0.8f;
        _orbitAngle += orbitSpeed * Time.deltaTime;

        int count = Mathf.Max(1, _soldiers.Count);
        for (int i = 0; i < _soldiers.Count; i++)
        {
            var s = _soldiers[i];
            if (!s || !s.CanHoldFormation) continue;

            // Base even spacing + global orbit + per-soldier jitter
            float baseAngle = (Mathf.PI * 2f) * (i / (float)count);
            float angle = baseAngle + _orbitAngle + _angleJitter[s];

            // Ring around the leader at the SAME HEIGHT as the leader (no ground raycast/bobbing)
            Vector3 ringXZ = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _leader.swarmRadius;
            Vector3 target = _leader.transform.position + ringXZ;
            target.y = _leader.transform.position.y;  // lock altitude to leader

            // Smooth follow toward ring position (smooth inside SwarmSoldier.MoveTo)
            s.MoveTo(target, _leader.swarmReformLerp);

            // Face tangentially along the orbit for a more “alive” feel
            // Tangent = rotate radial by 90deg in XZ plane
            Vector3 radial = s.transform.position - _leader.transform.position; radial.y = 0f;
            if (radial.sqrMagnitude > 0.0001f)
            {
                Vector3 tangent = new Vector3(-radial.z, 0f, radial.x).normalized; // 90° left-hand
                // Blend tangent facing with slight outward bias to keep a dynamic posture
                Vector3 faceDir = Vector3.Slerp(tangent, radial.normalized, 0.2f);
                s.Face(faceDir);
            }
        }
    }

    public bool TryOrderOneDive()
    {
        PurgeNulls();
        var next = _soldiers.FirstOrDefault(s => s && s.CanDive);
        if (!next) return false;

        next.StartDive(_leader.player.position, _leader.diveWindup);
        return true;
    }

    private void PurgeNulls()
    {
        for (int i = _soldiers.Count - 1; i >= 0; i--)
            if (_soldiers[i] == null) _soldiers.RemoveAt(i);
    }
}
