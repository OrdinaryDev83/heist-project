using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTooltip : InteractableBase
{
    public string message;
    public override bool CanHover(InventoryHandler inv)
    {
        return true;
    }

    public override InteractableData GetData(InventoryHandler inv)
    {
        return new InteractableData(message);
    }

    public override bool CanInteract(InventoryHandler inv)
    {
        return false;
    }
}
