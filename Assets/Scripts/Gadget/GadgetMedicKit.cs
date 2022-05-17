using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetMedicKit : GadgetBase
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
        return new InteractableData("Heal", 5f, true, (float)_charges / (float)_maxCharges);
    }

    public override void OnInteract(InventoryHandler inv, string[] parameters)
    {
        if (_charges <= 0)
            return;
        HealthHandler hh = null;
        if (inv.TryGetComponent(out hh))
        {
            hh.Health = hh.startHealth;
            _charges--;
            if (_charges <= 0)
                Destroy(gameObject);
        }
        else
        {
            Debug.LogError("No health handler");
        }
    }
}
