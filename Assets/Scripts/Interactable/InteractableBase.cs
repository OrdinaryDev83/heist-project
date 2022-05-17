using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable {

    public virtual InteractableData GetData(InventoryHandler inv)
    {
        return new InteractableData("Interact", interactionTime);
    }

    [SerializeField]
    protected float interactionTime;

    public virtual void OnInteract(InventoryHandler inv, string[] parameters) {

    }

    public virtual bool CanInteract(InventoryHandler inv) {
        return true;
    }
    
    public virtual bool CanHover(InventoryHandler inv) {
        return true;
    }
}

public interface IInteractable {
    public InteractableData GetData(InventoryHandler inv);

    public void OnInteract(InventoryHandler inv, string[] parameters);
    public bool CanInteract(InventoryHandler inv);

    public bool CanHover(InventoryHandler inv);
}

public struct InteractableData
{
    public string label;
    public float interactionTime;
    public bool hasProgression;
    public float progression;
    public InteractableData(string label, float interactionTime = 0f, bool hasProgression = false, float progression = 0f)
    {
        this.label = label;
        this.interactionTime = interactionTime;
        this.hasProgression = hasProgression;
        this.progression = progression;
    }
}