using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetAmmoKit : GadgetBase
{
    private int _charges;
    [SerializeField]
    private int _maxCharges;

    private void Start()
    {
        _charges = _maxCharges;
    }

    public override bool CanHover(InventoryHandler inv)
    {
        return true;
    }

    public override bool CanInteract(InventoryHandler inv)
    {
        return true;
    }

    public override InteractableData GetData(InventoryHandler inv)
    {
        return new InteractableData("Replenish Ammo", 5f, true, (float)_charges / (float)_maxCharges);
    }

    public override void OnInteract(InventoryHandler inv, string[] parameters)
    {
        if (_charges <= 0)
            return;
        InventoryHandler hh = null;
        if (inv.TryGetComponent(out hh))
        {
            ReplenishAmmo(hh);
            _charges--;
            if (_charges <= 0)
                Destroy(gameObject);
        }
        else
        {
            Debug.LogError("No inventory handler");
        }
    }

    void ReplenishAmmo(InventoryHandler hh)
    {
        foreach (var firearm in hh.firearms)
        {
            firearm.Ammo = firearm.maxAmmo;
            firearm.MagAmmo = firearm.maxMagAmmo;
        }
    }
}
