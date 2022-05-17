using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

public class AIHelper : MonoBehaviour
{
    public static AIHelper I = null;

    private void Awake()
    {
        I = this;
        PlayersManager.OnPlayerSpawned += SetPlayer;
        _playersSensorsSqrDist = new float[maxRay];
    }

    private Transform _player;
    private Collider2D _playerCollider;
    void SetPlayer(Transform player)
    {
        this._player = player;
        this._playerCollider = player.GetComponentInChildren<Collider2D>();
        _lastPlayerPos = player.position;
    }

    // c'est le ai qui devrait calculer la distance, si elle est trop grande demander à changer
    public float thinkClockRate = 2f;
    float _thinkClockCooldown = 0f;
    Vector2 _lastPlayerPos;
    private void Update()
    {
        if (_thinkClockCooldown > 0f)
            _thinkClockCooldown -= Time.deltaTime;
        else if (_thinkClockCooldown <= 0f)
        {
            _thinkClockCooldown = 1f / thinkClockRate;
            if (ThinkClock != null)
                ThinkClock();
            UpdateSensors();
            if (((Vector2)_player.position - _lastPlayerPos).sqrMagnitude > 0.25f)
            {
                if (RecalculatePath != null)
                    RecalculatePath();
                _lastPlayerPos = _player.position;
            }
        }
    }

    #region NodesAndSensors

    // gives all the defending nodes
    // conditions : not full, in range
    public AINode[] nodes;

    float[] _playersSensorsSqrDist;
    [SerializeField]
    private int maxRay = 8;
    private void UpdateSensors()
    {
        Vector2 origin = _player.transform.position;
        for (int i = 0; i < maxRay; i++)
        {
            Vector2 dir = GetDirAngle(i);
            Vector2 dest = MaxLineOfSight(_playerCollider, origin, dir);

            Debug.DrawLine(dest, origin, Color.white, 1f / thinkClockRate);

            Vector2 v = dest - origin;
            _playersSensorsSqrDist[i] = v.sqrMagnitude;
        }
    }

    private Vector2 GetFleePosition()
    {
        Transform[] v = WaveManager.I.GetSpawnPoints;
        int r = Random.Range(0, v.Length);
        return v[r].position;
    }

    private Vector2 GetDirAngle(int i)
    {
        float angle = ((float)i / (float)maxRay) * Mathf.PI * 2f;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    private Vector2 GetPositionNearPlayer()
    {
        Vector2 origin = _player.transform.position;
        List<Vector2> valid = new List<Vector2>();
        for (int i = 0; i < maxRay; i++)
        {
            float sqrLen = _playersSensorsSqrDist[i];
            if (sqrLen < minSight * minSight)
                continue;
            Vector2 dir = GetDirAngle(i);
            Vector2 p = origin + dir * Random.Range(minSight, Mathf.Sqrt(sqrLen));
            valid.Add(p);
        }
        int r = Random.Range(0, valid.Count);
        Vector2 point = valid[r];

        float q = (Mathf.PI * 2f) / maxSight;
        Vector2 rPoint = RotatePointAroundPivot(point, origin, Random.Range(-q, q) * Mathf.Rad2Deg * 0.5f);
        return rPoint;
    }

    Vector2 RotatePointAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        var dir = point - pivot;
        dir = Quaternion.Euler(Vector3.forward * angle) * dir;
        point = dir + pivot;
        return point;
     }

    public Vector2 PlayerPosition()
    {
        return _player.position;
    }

    public bool HasLineOfSightToPlayer(Collider2D self)
    {
        // only the map is colliding
        if ((self.transform.position - _player.transform.position).sqrMagnitude > maxSight * maxSight)
            return false;
        RaycastHit2D[] cds = Physics2D.LinecastAll(_player.position, self.transform.position, rayLayers);
        foreach (var item in cds)
        {
            return false;
        }
        return true;
    }

    // tell the ai that the player moved
    public delegate void RecalculatePathDelegate();
    public static RecalculatePathDelegate RecalculatePath;

    // the rate at which the brain of ais updates
    public delegate void ThinkClockDelegate();
    public static ThinkClockDelegate ThinkClock;

    public float maxSight = 5f;
    public float minSight = 1.5f;
    public LayerMask rayLayers;

    // dir needs to be normalized !
    Vector2 MaxLineOfSight(Collider2D targetCollider, Vector2 origin, Vector2 dir)
    {
        RaycastHit2D[] cds = Physics2D.RaycastAll(origin, dir, maxSight, rayLayers);
        foreach (var item in cds)
        {
            if (item.collider != targetCollider)
            {
                return item.point - dir * 0.5f;
            }
        }
        return origin + dir * maxSight;
    }
    #endregion

    // get a random node
    public Vector2 GetTarget(HealthHandler subscriber, ControllerPolice.Type type)
    {
        switch (type)
        {
            case ControllerPolice.Type.Attacking:
                {
                    if (WaveManager.I.State == WaveManager.AssaultState.Running)
                        return GetPositionNearPlayer();
                    return GetFleePosition();
                }
            case ControllerPolice.Type.Defending:
                {
                    int r = Random.Range(0, nodes.Length);
                    nodes[r].Subscribe(subscriber);
                    return (Vector2)nodes[r].transform.position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                }
            default:
                return Vector2.zero;
        }
    }
}
