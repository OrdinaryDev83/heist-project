using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCollectible : InteractableBase
{
    public override InteractableData GetData(InventoryHandler inv)
    {
        return new InteractableData("Grab " + collectibleName);
    }

    [SerializeField]
    private string collectibleName;
    public override void OnInteract(InventoryHandler inv, string[] parameters)
    {
        inv.AddCollectible(new CollectibleData(collectibleName));
        Destroy(gameObject);
    }
}

[System.Serializable]
public struct CollectibleData
{
    public string ID;
    public CollectibleData(string id)
    {
        ID = id;
    }
}