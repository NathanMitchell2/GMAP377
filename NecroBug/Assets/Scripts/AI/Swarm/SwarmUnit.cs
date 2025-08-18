using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwarmUnit : MonoBehaviour
{
    private EnemyAI _leader;                 
    private readonly List<SwarmSoldier> _soldiers = new();

    private readonly Dictionary<SwarmSoldier, float> _bobPhase = new();

    public bool AllSoldiersDead => _soldiers.All(s => s == null);

    public void Bind(EnemyAI leader)
    {
        _leader = leader;
        _soldiers.Clear();
        _bobPhase.Clear();

        var trs = leader.GetComponentsInChildren<Transform>(true);
        foreach (var tr in trs)
        {
            if (!tr || tr.gameObject == leader.gameObject) continue;
            if (!string.Equals(tr.name, "Bee Soldier", System.StringComparison.Ordinal)) continue;

            var s = tr.GetComponent<SwarmSoldier>();
            if (!s) s = tr.gameObject.AddComponent<SwarmSoldier>();
            s.Bind(leader);

            _soldiers.Add(s);
            _bobPhase[s] = Random.value * Mathf.PI * 2f;
        }

    }

    public void TickFormation()
    {
        PurgeNulls();

        int count = Mathf.Max(1, _soldiers.Count);
        for (int i = 0; i < _soldiers.Count; i++)
        {
            var s = _soldiers[i];
            if (!s || !s.CanHoldFormation) continue;

            float angle = (Mathf.PI * 2f) * (i / (float)count);
            Vector3 ring = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _leader.swarmRadius;

            Vector3 target = _leader.transform.position + ring;

            float y = target.y;
            if (Physics.Raycast(target + Vector3.up * 20f, Vector3.down, out var hit, 60f, _leader.groundMask))
                y = hit.point.y + _leader.hoverHeight;
            else
                y = _leader.transform.position.y + _leader.hoverHeight;

            float phase = _bobPhase[s] += Time.deltaTime * _leader.hoverBobSpeed;
            y += Mathf.Sin(phase) * _leader.hoverBobAmplitude;

            target.y = y;

            s.MoveTo(target, _leader.swarmReformLerp);

            Vector3 outward = s.transform.position - _leader.transform.position; outward.y = 0;
            if (outward.sqrMagnitude > 0.0001f)
                s.Face(outward.normalized);
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
