using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableContainer : InteractableBase {

    public Valuable containing;
    public GameObject bagPrefab;

    public override InteractableData GetData(InventoryHandler inv) {
        return new InteractableData(inv.bag == null ? "Bag " + containing : "Already carrying a bag", interactionTime);
    }

    public override bool CanInteract(InventoryHandler inv) {
        return inv.bag == null || containing.amount > 0;
    }
    public override void OnInteract(InventoryHandler inv, string[] parameters) {
        if(containing.amount <= 0) {
            return;
        }

        base.OnInteract(inv, parameters);

        var bag = (GameObject)Instantiate(bagPrefab, inv.transform.position, Quaternion.identity);
        var bb = bag.GetComponent<BagBase>();
        bb.containing.label = containing.label;

        var last = containing.amount;
        containing.amount -= bb.capacity;
        if(containing.amount < 0) {
            bb.containing.amount = -containing.amount;
            containing.amount = 0;
        } else {
            bb.containing.amount = bb.capacity;
        }

        bb.OnInteract(inv, null);
    }

    public override bool CanHover(InventoryHandler inv)
    {
        return containing.amount > 0;
    }
}

[System.Serializable]
public struct Valuable {
    public string label;
    public int amount;

    public override string ToString() {
        return amount.ToString() + " " + label;
    }
}