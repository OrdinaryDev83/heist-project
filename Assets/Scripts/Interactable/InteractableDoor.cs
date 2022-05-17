using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : InteractableBase {
    
    private Collider2D _coll;

    public string openedLabel;
    public string closedLabel;

    private float _startRotZ;
    private void Start() {
        _coll = GetComponent<Collider2D>();
        _startRotZ = transform.eulerAngles.z;
    }

    private bool _isOpened = false;
    public bool IsOpened {
        get {
            return _isOpened;
        }
    }

    public override void OnInteract(InventoryHandler inv, string[] parameter) {
        base.OnInteract(inv, parameter);
        _isOpened = !_isOpened;

        var from = new Vector2(float.Parse(parameter[0]), float.Parse(parameter[1]));

        float dot = Vector2.Dot(from - (Vector2)transform.position, transform.right);
        float offset = IsOpened ? 90f : 0f;
        if (dot > 0)
            transform.eulerAngles = Vector3.forward * (_startRotZ + offset);
        else
            transform.eulerAngles = Vector3.forward * (_startRotZ - offset);

        _coll.isTrigger = IsOpened;
    }

    public override InteractableData GetData(InventoryHandler inv) {
        return new InteractableData(!IsOpened ? closedLabel : openedLabel, !IsOpened ? interactionTime : 0);
    }
}