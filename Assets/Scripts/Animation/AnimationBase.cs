using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBase : MonoBehaviour {
    [SerializeField]
    protected MovementBase mvt;
    [SerializeField]
    protected HealthHandler health;
    [SerializeField]
    protected InventoryHandler inv;

    public Animator animator;

    public bool isAiming;

    protected virtual void Update() {
        if(inv) {
            inv.ShowSelected(isAiming && !inv.IsReloading() && !health.dead);
        }
        if (animator == null) return;
        animator.SetFloat("Step", mvt.step);
        animator.SetBool("Dead", health.dead);
        animator.SetFloat("WeaponType", inv.GetWearingWeaponType());
        animator.SetBool("Aim", isAiming);

    }
    public void Aim(Vector2 pos) {
        isAiming = true;
        mvt.lockRotation = true;
        LookAt(pos);
    }

    public void Aim() {
        isAiming = true;
        mvt.lockRotation = true;
    }

    public void StopAiming() {
        mvt.lockRotation = false;
        isAiming = false;
    }

    protected float BodyShakeOffset = 0f;
    protected float Rate = 0f;
    protected float RecoverBodyShakeRate = 1f;
    protected void LookAt(Vector2 pos) {
        if(Rate > 0f) {
            Rate -= RecoverBodyShakeRate * Time.deltaTime;
        } else if(Rate < 0f){
            Rate = 0f;
        }

        var dir = (Vector2)transform.position - pos;
        Quaternion lookRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f + BodyShakeOffset * Rate)); //-90f is an offset
        var newRot = Quaternion.Euler(Vector3.forward * BodyShakeOffset * Rate);
        sprite.rotation = lookRotation;//Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
    }

    public Transform sprite;

    public bool IsDead() {
        return health.dead;
    }

    public void BodyShake(float amplitude, float recover) {
        int direction = Random.Range(0, 2) == 0 ? -1 : 1;
        RecoverBodyShakeRate = recover;
        BodyShakeOffset = amplitude * direction * 45f * Mathf.Sqrt(Random.Range(0f, 1f));
        Rate = 1f;
    }

    public void Fire() {
        if (!isAiming) return;
        if(animator) animator.SetTrigger("Fire");
    }

    protected void FixedUpdate() {
        
    }
}
