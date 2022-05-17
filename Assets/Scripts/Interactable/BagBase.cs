using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagBase : InteractableBase {

    public Valuable containing;
    public int capacity = 10000;

    protected virtual void Start() {

    }

    public bool InInventory {
        set {
            gameObject.SetActive(!value);
        }
        get {
            return !gameObject.activeSelf;
        }
    }

    public override bool CanHover(InventoryHandler inv)
    {
        return !InInventory;
    }

    public override void OnInteract(InventoryHandler inv, string[] parameters) {
        base.OnInteract(inv, parameters);
        InInventory = inv.AddBag(this);
    }

    public override InteractableData GetData(InventoryHandler inv) {
        return new InteractableData(inv.bag == null ? "Pickup Bag" : "Already carrying a bag", 1f);
    }

    public override bool CanInteract(InventoryHandler inv) {
        return inv.bag == null;
    }
}
