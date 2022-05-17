using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FirearmBase : MonoBehaviour {

    AnimationBase _anim;
    Animator _selfAnim;

    new public string name;
    public enum Type {
        Pistol,
        Rifle,
        Shotgun,
        Smg
    }
    public Type type;

    public int maxAmmo;
    [SerializeField]
    protected int ammo;
    public int Ammo {
        get {
            return ammo;
        }
        set {
            ammo = value;
        }
    }

    public int maxMagAmmo;
    [SerializeField]
    protected int magAmmo;
    public int MagAmmo {
        get {
            return magAmmo;
        }
        set {
            magAmmo = value;
        }
    }

    [FormerlySerializedAs("Damage")] public int damage;

    [FormerlySerializedAs("FireRate")] public int fireRate; //shot per minute
    [FormerlySerializedAs("Stability")] public int stability; //0 to 100
    [FormerlySerializedAs("Power")] public int power;
    public float reloadTime;

    public bool reloading;

    public GameObject projectile;
    public GameObject trail;
    public GameObject muzzle;
    public Transform projStart;
    public ParticleSystem magThrow;
    public ParticleSystem shellThrow;

    public Transform spriteRoot;

    public LayerMask hitable;

    Collider2D _col;
    private void Awake() {
        _anim = GetComponentInParent<AnimationBase>();
        ammo = maxAmmo;
        reloading = false;
        _col = GetComponentInParent<Collider2D>();
        _selfAnim = GetComponent<Animator>();
    }

    public virtual void OnShoot() {
        if (_cooldown > 0f || reloading || !_anim.isAiming || _anim.IsDead()) return;

        if(MagAmmo <= 0) {
            TryReload();
            return;
        }

        MagAmmo--;
        _cooldown = 1f / ((float)fireRate / 60f);
        _anim.Fire() ;
        if(_selfAnim) _selfAnim.SetTrigger("Fire");
        float kick = (1f - ((float)stability / 100f));
        CameraController.I.Shake(kick * 5f);

        var s = projStart.right;
        if (projectile) {
            var proj = (GameObject)Instantiate(projectile, projStart.position, Quaternion.Euler(Vector3.forward * (Mathf.Atan2(s.y, s.x) * Mathf.Rad2Deg - 90f)));
            var b = proj.GetComponent<ProjectileBase>();
            b.Set(_col, (Vector2)projStart.up, damage, power / 5);
        }else if (trail) {
            RaycastHit2D[] hitl = Physics2D.RaycastAll(projStart.position, s, 30f, hitable);
            RaycastHit2D hit = new RaycastHit2D();
            foreach (var d in hitl) {
                if (!d.collider.isTrigger) {
                    hit = d;
                    HealthHandler h = null;
                    if (hit.collider.TryGetComponent<HealthHandler>(out h)) {
                        h.Damage(damage);
                        ApplyPower(hit.collider.gameObject);

                    } else {
                        Transform p = hit.collider.transform.parent;
                        if (p != null && p.TryGetComponent<HealthHandler>(out h)) {
                            h.Damage(damage);
                            ApplyPower(hit.collider.transform.parent.gameObject);
                        }
                    }
                    break;
                }
            }
            Debug.DrawRay(projStart.position, hit.distance * s, Color.red, 30f);
            Vector2 distantPoint = hit.collider == null ? (Vector2)projStart.position + (Vector2)s * 30f : (Vector2)hit.point;

            var tr = (GameObject)Instantiate(trail, Vector2.Lerp(projStart.position, distantPoint, 0.5f), Quaternion.Euler(Vector3.forward * (Mathf.Atan2(s.y, s.x) * Mathf.Rad2Deg - 90f)));
            var c = tr.GetComponent<SpriteRenderer>().size;
            c.y = Vector2.Distance(projStart.position, distantPoint);
            tr.GetComponent<SpriteRenderer>().size = c;
        }
        if (muzzle) {
            var muz = (GameObject)Instantiate(muzzle, projStart.position, Quaternion.Euler(Vector3.forward * (Mathf.Atan2(s.y, s.x) * Mathf.Rad2Deg + 90f)));
        }
        if (shellThrow) {
            shellThrow.Stop();
            shellThrow.Play();
        }
        float n = Mathf.Sqrt(((float)stability / 100f)); //maximise la stabilité
        n /= 2;
        n += 0.5f;
        _anim.BodyShake(kick, n * (float)fireRate / 60f);
    }

    void ApplyPower(GameObject go) {
        Rigidbody2D r = null;
        if (go.TryGetComponent<Rigidbody2D>(out r)) {
            r.AddForce(transform.right * power / 5f, ForceMode2D.Impulse);
        }
    }

    float _cooldown;
    float _reloadCooldown;
    protected virtual void Update() {
        if (_anim.IsDead()) return;
        if(_cooldown > 0f) {
            _cooldown -= Time.deltaTime;
        }else if(_cooldown < 0f) {
            _cooldown = 0f;
        }
        if (_reloadCooldown > 0f) {
            _reloadCooldown -= Time.deltaTime;
        } else if (_reloadCooldown < 0f) {
            int previous = MagAmmo;
            MagAmmo = Mathf.Min(maxMagAmmo, Ammo);
            if (Ammo < maxMagAmmo) {
                Ammo = 0;
            } else {
                Ammo -= maxMagAmmo - previous;
            }
            reloading = false;
            _reloadCooldown = 0f;
        }
        if(_anim.animator) _anim.animator.SetBool("Reloading", reloading);
        if(_anim.animator) _anim.animator.SetFloat("Firerate", ((float)fireRate / 60f));
        if(_selfAnim) _selfAnim.SetFloat("Firerate", ((float)fireRate / 60f));
    }

    public virtual void TryReload() {
        if (reloading || MagAmmo == maxMagAmmo || Ammo <= 0 || _anim.IsDead()) return;
        Reload();
    }

    void Reload() {
        reloading = true;
        _reloadCooldown = reloadTime;
        if (magThrow) {
            magThrow.Stop();
            magThrow.Play();
        }
    }
}
