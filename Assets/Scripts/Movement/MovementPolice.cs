using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MovementPolice : MovementBase {

    [SerializeField]
    ControllerPolice controller;
    [SerializeField]
    Seeker seeker;

    protected override void Start() {
        base.Start();
        
        step = 1;
    }

    Path _path;
    public float sqrtNextWaypointDistance = 3;

    private int _currentWaypoint = 0;

    public bool reachedEndOfPath;

    public void SearchTarget(Vector2 target)
    {
        seeker.StartPath(transform.position, target, OnPathComplete);
    }

    public void PathWork() {
        if (_path == null) {
            return;
        }

        reachedEndOfPath = false;

        float distanceToWaypoint;
        while (true) {
            distanceToWaypoint = Vector3.SqrMagnitude(_path.vectorPath[_currentWaypoint] - transform.position);
            if (distanceToWaypoint < sqrtNextWaypointDistance) {
                if (_currentWaypoint + 1 < _path.vectorPath.Count)
                {
                    _currentWaypoint++;
                } else {
                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }

        Vector2 dir = (_path.vectorPath[_currentWaypoint] - transform.position);
        SetAxis(dir);
    }

    public void OnPathComplete(Path p) {
        if (!p.error) {
            if (_path != null) {
                _path.Release(this);
            }
            _path = p;
            _path.Claim(this);
            _currentWaypoint = 0;
        }
    }
}