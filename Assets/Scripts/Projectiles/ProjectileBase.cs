using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {

    Rigidbody2D _rigid;
    public float speed;
    public int damage;
    public int power;

    private void Start() {
        _rigid = GetComponent<Rigidbody2D>();
    }

    Collider2D _emi;
    public void Set(Collider2D emitter, Vector2 direction, int damage, int power) {
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.velocity = direction.normalized * speed;
        this._emi = emitter;
        this.damage = damage;
        this.power = power;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (Collider2D.Equals(_emi, collision) || collision.isTrigger) return;
        HealthHandler health = null;
        if (collision.gameObject.TryGetComponent(out health)) {
            health.Damage(damage);
            Rigidbody2D r = null;
            if(collision.TryGetComponent<Rigidbody2D>(out r)) {
                r.AddForce(transform.right * power, ForceMode2D.Impulse);
            }
            damage = 0;
        }
        Destroy(gameObject);
    }
}
