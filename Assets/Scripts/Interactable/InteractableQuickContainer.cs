using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableQuickContainer : InteractableBase
{
    [SerializeField]
    private int moneyAmount;

    public override bool CanHover(InventoryHandler inv)
    {
        return true;
    }

    public override bool CanInteract(InventoryHandler inv)
    {
        return true;
    }

    public override void OnInteract(InventoryHandler inv, string[] parameters)
    {
        PlayersManager.AddQuickMoney(moneyAmount);
        Destroy(gameObject);
    }

    public override InteractableData GetData(InventoryHandler inv)
    {
        return new InteractableData(string.Concat("Grab ", moneyAmount.ToString(), "$ Bill"));
    }
}
