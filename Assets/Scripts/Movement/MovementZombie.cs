using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MovementZombie : MovementBase {

    ControllerZombie _controller;
    Seeker _seeker;
    Collider2D _selfCollider2D;

    protected override void Start() {
        base.Start();
        _controller = GetComponent<ControllerZombie>();
        _seeker = GetComponent<Seeker>();
        _selfCollider2D = GetComponentInChildren<Collider2D>();
        step = 1;
    }

    Path _path;
    public float sqrtNextWaypointDistance = 3;

    private int _currentWaypoint = 0;

    public bool reachedEndOfPath;


    void SearchPlayer() {
        _seeker.StartPath(transform.position, _controller.player.position, OnPathComplete);
    }

    Vector2 _lastPos;
    protected override void Update() {
        base.Update();
        if (!_controller.player) return;

        Vector2 dist = (Vector2)_controller.player.position - _lastPos;

        Vector2 dir = _controller.player.position - transform.position;

        if (dist.sqrMagnitude > 1f) {
            SearchPlayer();
            _lastPos = _controller.player.position;
        }

        if (LineOfSightToTarget(dir)) { //If close, directly follow like an electron
            if(dir.magnitude > _controller.damageDist) {
                SetAxis(dir);
            } else {
                SetAxis(Vector2.zero);
            }
        } else {
            PathWork();
        }
    }

    bool LineOfSightToTarget(Vector2 dir) {
        RaycastHit2D[] cds = Physics2D.LinecastAll(transform.position, _controller.player.position);
        foreach (var item in cds) {
            if (item.collider != _selfCollider2D && item.collider != _controller.playerCollider) return false;
        }
        return true;
    }

    void PathWork() {
        if (_path == null) {
            return;
        }

        reachedEndOfPath = false;

        float distanceToWaypoint;
        while (true) {
            distanceToWaypoint = Vector3.SqrMagnitude(_path.vectorPath[_currentWaypoint] - transform.position);
            if (distanceToWaypoint < sqrtNextWaypointDistance) {
                if (_currentWaypoint + 1 < _path.vectorPath.Count) {
                    _currentWaypoint++;
                } else {

                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }

        Vector3 dir = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
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