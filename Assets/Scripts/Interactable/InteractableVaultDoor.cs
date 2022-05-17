using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVaultDoor : InteractableSecuredDoor
{
    BagDrillSystem BagDrillSystem => drillSystem as BagDrillSystem;
    
    public override void OnInteract(InventoryHandler inv, string[] parameter)
    {
        if (IsOpened)
            return;
        if (parameter.Length > 2)
        {
            bool unlock = bool.Parse(parameter[2]);
            if (unlock)
                unlocked = true;
        }
        if (!unlocked)
        {
            drillSystem.SpawnDrill(this);
        }

        if (unlocked)
        {
            base.OnInteract(inv, parameter);
        }
    }

    public override bool CanHover(InventoryHandler inv)
    {
        return !IsOpened;
    }

    public override bool CanInteract(InventoryHandler inv)
    {
        return base.CanInteract(inv) && BagDrillSystem.HasDrillBagDropped();
    }

    public override InteractableData GetData(InventoryHandler inv)
    {
        if (!unlocked)
        {
            if (BagDrillSystem.HasDrillBagDropped())
            {
                return new InteractableData("Setup the Heavy Drill", 10f);
            }
            else
            {
                return new InteractableData("Requires a Heavy Drill Bag");
            }
        }

        return base.GetData(inv);
    }
}
