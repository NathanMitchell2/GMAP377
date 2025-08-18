using UnityEngine;

public class SwarmSoldier : MonoBehaviour
{
    private EnemyAI _leader;
    private bool _charging;
    private float _chargeTimer;
    private float _chargeUpTime;
    private bool _diving;
    private Vector3 _diveTarget;

    public bool CanHoldFormation => !_charging && !_diving;
    public bool CanDive => !_charging && !_diving;

    public void Bind(EnemyAI leader)
    {
        _leader = leader;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void MoveTo(Vector3 worldPos, float lerp)
    {
        transform.position = Vector3.Lerp(transform.position, worldPos, Time.deltaTime * Mathf.Max(1f, lerp));
    }

    public void Face(Vector3 forwardXZ)
    {
        var look = Quaternion.LookRotation(forwardXZ, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 8f);
    }

    public void StartDive(Vector3 playerPos, float chargeUp)
    {
        _charging = true;
        _chargeTimer = 0f;
        _chargeUpTime = chargeUp;
        _diveTarget = playerPos;

    }

    void Update()
    {
        if (_leader == null) return;

        if (_charging)
        {
            _chargeTimer += Time.deltaTime;
            if (_chargeTimer >= _chargeUpTime)
            {
                _charging = false;
                _diving = true;
            }
            return;
        }

        if (_diving) DoDive();
    }

    private void DoDive()
    {
        Vector3 start = transform.position;

        float groundY = _diveTarget.y;
        if (Physics.Raycast(_diveTarget + Vector3.up * 20f, Vector3.down, out var gHit, 60f, _leader.groundMask))
            groundY = gHit.point.y;

        Vector3 toTarget = (_diveTarget - start);
        Vector3 dir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector3.forward;

        Vector3 step = dir * _leader.diveSpeed * Time.deltaTime;
        if (Vector3.Distance(start, _diveTarget) > 3f)
            step += Vector3.up * (_leader.diveArcHeight * 0.5f) * Time.deltaTime;
        else
            step += Vector3.down * 0.5f * Time.deltaTime;

        Vector3 next = start + step;

        Vector3 seg = next - start;
        float segLen = Mathf.Max(seg.magnitude, 0.0001f);
        if (Physics.SphereCast(start, _leader.diveHitRadius, seg / segLen, out var hit, segLen, _leader.robotMask))
        {
            DealDamageViaTeamSystem(hit.collider, _leader.diveDamage);
            Destroy(gameObject);
            return;
        }

        transform.position = next;

        if (next.y <= groundY + 0.05f)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 f = (next - start); f.y = 0f;
        if (f.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(f.normalized, Vector3.up);
    }

    private void DealDamageViaTeamSystem(Collider c, int amount)
    {
        if (!c) return;

        var list = DamageObject.GetPlayerHealths(c.gameObject);
        if (list.Count == 0)
        {
            var parentHealth = c.GetComponentInParent<PlayerHealth>();
            if (parentHealth != null) list.Add(parentHealth);
        }
        if (list.Count > 0) DamageObject.Damage(amount, list[0]);
    }
}
