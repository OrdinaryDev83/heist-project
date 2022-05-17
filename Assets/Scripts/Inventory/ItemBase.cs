using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

    new public string name = "default";

    public virtual void OnPick(InventoryHandler inv) {

    }

    public virtual void OnDrop() {

    }
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        
    }
}
