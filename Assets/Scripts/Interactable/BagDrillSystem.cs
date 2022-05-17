using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagDrillSystem : MonoBehaviour, IDrillSystem
{
    public int drillTime = 120;
    
    public GadgetDrill drillPrefab;
    GadgetDrill _actualDrill;

    public bool Drilling()
    {
        return _actualDrill != null;
    }

    public bool HasDrillBagDropped()
    {
        return drillBag != null;
    }

    public void SpawnDrill(IInteractable interactable)
    {
        if (!HasDrillBagDropped())
            return;
        var a = Instantiate(drillPrefab.gameObject, transform.position, Quaternion.identity, transform);
        var dr = a.GetComponent<GadgetDrill>();
        dr.transform.localRotation = Quaternion.identity;
        _actualDrill = dr;
        dr.SetRemainingTime(interactable, drillTime);
    }
    
    private BagBase drillBag;

    public void OnTEnter(Collider2D collision)
    {
        BagBase c;
        if(collision.TryGetComponent(out c) && drillBag == null && c.containing.label == "Heavy Drill")
        {
            drillBag = c;
        }
    }

    public void OnTExit(Collider2D collision) {
        BagBase c;
        if(collision.TryGetComponent(out c) && drillBag != null && c.containing.label == "Heavy Drill")
        {
            drillBag = null;
        }
    }
}
