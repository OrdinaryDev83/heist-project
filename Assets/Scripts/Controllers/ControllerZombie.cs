using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerZombie : ControllerBase {

    HealthHandler _healthHandler;
    public Transform player;
    [HideInInspector]
    public Collider2D playerCollider;

    public int damages;

    protected override void Start() {
        base.Start();
        _healthHandler = GetComponent<HealthHandler>();
        playerCollider = player.GetComponentInChildren<Collider2D>();
    }

    public float damageDist = 0.5f;
    public float damageRate = 60;

    float _cooldown;
    protected void Update() {
        if(_cooldown > 0f) {
            _cooldown -= Time.deltaTime;
        } else if (_cooldown < 0f){
            _cooldown = 0f;
        }
        if(_cooldown <= 0f) {
            if (!player) return;
            Vector2 dir = player.position - transform.position;
            if (dir.magnitude <= damageDist) {
                OnAttack();
            }
        }
    }

    public void OnAttack() {
        _cooldown = 1f / damageRate;
        HealthHandler h = null;
        h = _healthHandler.GetHhClosestInRadius(1f);
        if (h != null) {
            h.Damage(damages);
        }
        
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, damageDist);
    }
}
