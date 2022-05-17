using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSafe : InteractableContainer
{
    public DrillSystem drillSystem;
    public bool unlocked;

    public override void OnInteract(InventoryHandler inv, string[] parameters)
    {
        if (parameters.Length > 2)
        {
            bool unlock = bool.Parse(parameters[2]);
            if (unlock)
                unlocked = true;
        }
        if (!unlocked)
        {
            var c = new CollectibleData("Safekey");
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
            base.OnInteract(inv, parameters);
        }
    }

    public override InteractableData GetData(InventoryHandler inv)
    {
        if (!unlocked)
        {
            var c = new CollectibleData("Safekey");
            if (inv.HasCollectible(c))
            {
                return new InteractableData("Unlock with Safekey");
            }
            else
            {
                return new InteractableData("Place a drill");
            }
        }

        return base.GetData(inv);
    }
}