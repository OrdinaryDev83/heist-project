using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillSystem : MonoBehaviour
{
    public int drillTime = 120;
    
    public GadgetDrill drillPrefab;
    GadgetDrill _actualDrill;
    
    public bool Drilling()
    {
        return _actualDrill != null;
    }

    public void SpawnDrill(IInteractable interactable)
    {
        var a = Instantiate(drillPrefab.gameObject, transform.position, Quaternion.identity, transform);
        var dr = a.GetComponent<GadgetDrill>();
        dr.transform.localRotation = Quaternion.identity;
        _actualDrill = dr;
        dr.SetRemainingTime(interactable, drillTime);
    }
}
