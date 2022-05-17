using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBase : MonoBehaviour {

    private MovementBase _mvt;
    protected HealthHandler Health;
    protected AnimationBase Anim;
    protected InventoryHandler Inv;
    protected InteractionHandler Interaction;

    protected virtual void Start() {
        _mvt = GetComponent<MovementBase>();
        Health = GetComponent<HealthHandler>();
        Anim = GetComponent<AnimationBase>();
        Inv = GetComponent<InventoryHandler>();
        Interaction = GetComponent<InteractionHandler>();
    }

    protected void MoveEntity(Vector2 vect) {
        if (_mvt && _mvt.enabled) _mvt.SetAxis(vect);
    }
}
