using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwarmUnit : MonoBehaviour
{
    private EnemyAI _leader;
    private readonly List<SwarmSoldier> _soldiers = new();

    private readonly Dictionary<SwarmSoldier, float> _angleJitter = new();
    private float _orbitAngle;

    public bool AllSoldiersDead => _soldiers.All(s => s == null);

    public void Bind(EnemyAI leader)
    {
        _leader = leader;
        _soldiers.Clear();
        _angleJitter.Clear();

         var trs = leader.GetComponentsInChildren<Transform>(true);
        foreach (var tr in trs)
        {
            if (!tr || tr.gameObject == leader.gameObject) continue;
            if (!string.Equals(tr.name, "Bee Soldier", System.StringComparison.Ordinal)) continue;

            var s = tr.GetComponent<SwarmSoldier>();
            if (!s) s = tr.gameObject.AddComponent<SwarmSoldier>();
            s.Bind(leader);

             s.transform.SetParent(null, true);

            _soldiers.Add(s);
            _angleJitter[s] = Random.Range(-0.35f, 0.35f);
        }

        _orbitAngle = 0f;
    }

    public void TickFormation()
    {
        PurgeNulls();
        if (_soldiers.Count == 0 || _leader == null) return;

        float orbitSpeed = Mathf.Max(0.2f, _leader.swarmReformLerp) * 0.8f;
        _orbitAngle += orbitSpeed * Time.deltaTime;

        int count = Mathf.Max(1, _soldiers.Count);
        for (int i = 0; i < _soldiers.Count; i++)
        {
            var s = _soldiers[i];
            if (!s || !s.CanHoldFormation) continue;

            float baseAngle = (Mathf.PI * 2f) * (i / (float)count);
            float angle = baseAngle + _orbitAngle + _angleJitter[s];

            Vector3 ringXZ = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _leader.swarmRadius;
            Vector3 target = _leader.transform.position + ringXZ;
            target.y = _leader.transform.position.y;  // lock altitude to leader

             s.MoveTo(target, _leader.swarmReformLerp);

            Vector3 radial = s.transform.position - _leader.transform.position; radial.y = 0f;
            if (radial.sqrMagnitude > 0.0001f)
            {
                Vector3 tangent = new Vector3(-radial.z, 0f, radial.x).normalized; // 90° left-hand
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
