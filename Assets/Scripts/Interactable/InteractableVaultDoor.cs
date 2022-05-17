using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVaultDoor : InteractableSecuredDoor
{
    public override void OnInteract(InventoryHandler inv, string[] parameter)
    {
        if (IsOpened)
            return;
        base.OnInteract(inv, parameter);
    }
}
