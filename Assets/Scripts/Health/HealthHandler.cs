using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour {

    public int startHealth;
    [SerializeField]
    protected int health;
    public int Health {
        get {
            return health;
        }
        set {
            health = value;
            if(health <= 0) {
                OnDeath();
                health = 0;
            }else if (health > startHealth) {
                health = startHealth;
            }
            UpdateUI();
        }
    }

    public delegate void OnDeathEventDelegate();
    public OnDeathEventDelegate OnDeathEvent;

    public bool dead;

    protected virtual void Awake() {
        dead = false;
        health = startHealth;
    }

    public virtual void Damage(int damage) {
        Health -= damage;
    }

    public virtual void Heal(int healing) {
        Health += healing;
    }

    public virtual HealthHandler GetHhClosestInRadius(float radius) {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        HealthHandler h = null;
        float min = radius;
        foreach (var hit in hits) {
            HealthHandler hh = null;
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if(hit.TryGetComponent<HealthHandler>(out hh) && dist < min) {
                h = hh;
            }
        }
        return h;
    }

    protected virtual void UpdateUI()
    {

    }

    public virtual void OnDeath() {
        dead = true;
        gameObject.SetActive(false);
        OnDeathEvent?.Invoke();
        Destroy(gameObject, 2f);
    }
}
