using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPolice : ControllerBase {

    MovementPolice _mvtPl;
    HealthHandler _healthHandler;
    Collider2D _selfCollider2D;

    [Range(0f, 1f)]
    public float damagesPercentage;

    public float shootingMaxDistance = 5f;

    protected void Awake() {
        base.Start();
        _healthHandler = GetComponent<HealthHandler>();
        _mvtPl = GetComponent<MovementPolice>();
        _selfCollider2D = GetComponentInChildren<Collider2D>();
        int max = System.Enum.GetValues(typeof(Type)).Length;
        type = (Random.Range(0, 100) < 15 ? Type.Defending : Type.Attacking);

        AIHelper.ThinkClock += Think;
        MoveToTarget();
        Think();
    }

    float _distanceFromTarget = Mathf.Infinity;
    Vector2 _target = Vector2.zero;
    bool _seeing = false;

    private void FixedUpdate()
    {
        _seeing = AIHelper.I.HasLineOfSightToPlayer(_selfCollider2D);

        _distanceFromTarget = Vector2.Distance(_target, transform.position);
        if (_distanceFromTarget > 1f)
        {
            _mvtPl.PathWork();
            Anim.StopAiming();
        }
        else
        {
            if (_seeing)
                Anim.Aim(AIHelper.I.PlayerPosition());
            _mvtPl.SetAxis(Vector2.zero);
            _traveling = false;
        }
    }

    public enum Type
    {
        Attacking,
        Defending
    }
    public Type type;

    public float interactionMaxDistance = 2f;

    protected virtual void Think() {
        if (_healthHandler.dead)
            return;
        if (_traveling)
            OpenDoorOnTravel();
        switch (type)
        {
            case Type.Attacking:
                AttackingBrain();
                break;
            case Type.Defending:
                DefendingBrain();
                break;
            default:
                break;
        }
    }

    bool _traveling = false;
    protected virtual void MoveToTarget()
    {
        if (_mvtPl == null || Anim == null || _healthHandler.dead)
            return;
        _target = AIHelper.I.GetTarget(_healthHandler, type);

        Anim.StopAiming();
        _mvtPl.SearchTarget(_target);
        _traveling = true;
    }

    void OpenDoorOnTravel()
    {
        IInteractable b = Interaction.GetInteraction(Anim.sprite.right, interactionMaxDistance);
        if (b != null && b.CanHover(Inv) && b.CanInteract(Inv))
        {
            if (b is InteractableDoor && !(b as InteractableDoor).IsOpened)
            {
                b.OnInteract(Inv, new string[] { transform.position.x.ToString(), transform.position.y.ToString() });
            }
        }
    }


    // KILL THEM WHEN WAVE PAUSE STATE AND COMING BACK AT SPAWN
    void AttackingBrain()
    {
        if (!_traveling)
        {
            if (_seeing)
            {
                Shoot();
            }
            else
            {
                MoveToTarget();
            }
        }
        /*if (!player)
            return;

        Vector2 dir = player.position - transform.position;

        if (LineOfSightToTarget(dir))
        {
            float d = dir.magnitude;
            if (d > shootingMaxDistance)
            {
                anim.StopAiming();
                mvt_pl.PathWork();
            }
            else
            {
                mvt_pl.SetAxis(Vector2.zero);
                anim.Aim((Vector2)transform.position + dir);
                ShootTowards(dir);
            }
        }
        else
        {
            InteractableBase b = interaction.GetInteraction(anim.sprite.right, interactionMaxDistance);
            if (b != null && b.active && b.CanInteract(inv))
            {
                if (b is InteractableDoor && !(b as InteractableDoor).IsOpened)
                {
                    b.OnInteract(inv, new string[] { transform.position.x.ToString(), transform.position.y.ToString() });
                }
            }
            HealthHandler h = interaction.GetHealthHandler(health, anim.sprite.right, interactionMaxDistance);
            if (h != null && !h.dead && (h is HealthHandler_Obstacle))
            {
                Vector2 dirFromObstacle = ((Vector2)h.transform.position + (Vector2.left + Vector2.down) * 0.5f - (Vector2)transform.position).normalized;
                if (dirFromObstacle.sqrMagnitude < 2f)
                {
                    mvt_pl.SetAxis(-dirFromObstacle);
                }

                anim.Aim();
                ShootTowards(dirFromObstacle);
            }
            else
            {
                anim.StopAiming();
                mvt_pl.PathWork();
            }
        }*/
    }

    void DefendingBrain()
    {
        
    }

    public virtual void Shoot() {
        Inv.UseItem();
    }
}
