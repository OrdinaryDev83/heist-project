using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableSecuredDoor : InteractableDoor {

    public GadgetDrill drillPrefab;
    GadgetDrill _actualDrill;

    [FormerlySerializedAs("drilled")] public bool unlocked;

    public int drillTime = 120;

    public bool Drilling() {
        return _actualDrill != null;
    }

    public override void OnInteract(InventoryHandler inv, string[] parameter)
    {
        if (!unlocked)
        {
            var c = new CollectibleData("Keycard");
            if (inv.HasCollectible(c))
            {
                base.OnInteract(inv, parameter);
                inv.RemoveCollectible(c);
                unlocked = true;
            }
            else
            {
                var a = Instantiate(drillPrefab.gameObject, transform.position, Quaternion.identity, transform);
                var dr = a.GetComponent<GadgetDrill>();
                dr.transform.localRotation = Quaternion.identity;
                _actualDrill = dr;
                dr.SetRemainingTime(this, drillTime);
            }
        }

        if (unlocked)
        {
            base.OnInteract(inv, parameter);
        }
    }

    public override bool CanInteract(InventoryHandler inv)
    {
        return _actualDrill == null;
    }

    public override bool CanHover(InventoryHandler inv)
    {
        return CanInteract(inv);
    }

    public override InteractableData GetData(InventoryHandler inv) {
        if (!unlocked)
        {
            var c = new CollectibleData("Keycard");
            if (inv.HasCollectible(c))
            {
                return new InteractableData("Unlock with Keycard");
            }
            else
            {
                return new InteractableData("Place a drill");
            }
        }

        return base.GetData(inv);
    }
}
