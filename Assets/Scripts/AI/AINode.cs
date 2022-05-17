using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINode : MonoBehaviour
{
    [SerializeField]
    private int aiCapacity = 3;
    [SerializeField]
    private List<HealthHandler> subscribers;

    public bool HasRoom
    {
        get { return subscribers.Count < aiCapacity; }
    }

    private void Awake()
    {
        AIHelper.ThinkClock += Think;
        subscribers = new List<HealthHandler>();
    }

    public void Subscribe(HealthHandler hh)
    {
        if (!HasRoom)
        {
            Debug.LogWarning("Full !", hh.gameObject);
            return;
        }
        else if (subscribers.Contains(hh))
        {
            Debug.LogWarning("Already subscribed !", hh.gameObject);
            return;
        }
        subscribers.Add(hh);
    }

    public void RemoveAllSubsribers()
    {
        subscribers.Clear();
    }

    public void Unsubcribe(HealthHandler hh)
    {
        subscribers.Remove(hh);
    }

    // need to remove dead ais
    private void Think()
    {
        var temp = new List<HealthHandler>(subscribers);
        foreach (var item in temp)
        {
            if (item == null)
            {
                Unsubcribe(item);
            }
            else
            {
                // TODO : check time since subscribe
                if (item.dead)
                    Unsubcribe(item);
            }
        }
    }
}
