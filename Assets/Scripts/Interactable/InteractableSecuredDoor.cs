using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableSecuredDoor : InteractableDoor {

    public DrillSystem drillSystem;

    public bool unlocked;

    public override void OnInteract(InventoryHandler inv, string[] parameter)
    {
        if (parameter.Length > 2)
        {
            bool unlock = bool.Parse(parameter[2]);
            if (unlock)
                unlocked = true;
        }
        if (!unlocked)
        {
            var c = new CollectibleData("Keycard");
            if (inv != null && inv.HasCollectible(c))
            {
                inv.RemoveCollectible(c);
                unlocked = true;
            }
            else
            {
                drillSystem.SpawnDrill(this);
            }
        }

        if (unlocked)
        {
            base.OnInteract(inv, parameter);
        }
    }

    public override bool CanInteract(InventoryHandler inv)
    {
        return !drillSystem.Drilling();
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
