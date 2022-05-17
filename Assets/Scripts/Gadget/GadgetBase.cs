using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetBase : MonoBehaviour, IInteractable {

    public virtual InteractableData GetData(InventoryHandler inv)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnInteract(InventoryHandler inv, string[] parameters)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CanInteract(InventoryHandler inv)
    {
        throw new System.NotImplementedException();
    }
    
    public virtual bool CanHover(InventoryHandler inv)
    {
        throw new System.NotImplementedException();
    }
}
