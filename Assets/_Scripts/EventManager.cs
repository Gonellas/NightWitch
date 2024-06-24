using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventsType
{
    Movement,
    Ground_Attack,
    Ice_Attack,
    Fire_Attack,
    Thunder_Attack,
    Shield_Activated,
    Shield_Deactivated,
}

public class EventManager 
{
    public delegate void MethodToSubscribe(params object[] parameters);

    private static Dictionary<EventsType, MethodToSubscribe> _events;

    public static void SubscribeToEvent(EventsType eventsType, MethodToSubscribe methodToSubscribe)
    {
        if(_events == null) _events = new Dictionary<EventsType, MethodToSubscribe>();

        if(!_events.ContainsKey(eventsType)) _events.Add(eventsType, null);

        _events[eventsType] += methodToSubscribe;
    }

    public static void UnsubscribeToEvent(EventsType eventType, MethodToSubscribe methodToSubscribe)
    {
        if (_events == null) return;

        if(!_events.ContainsKey(eventType)) return;

        _events[eventType] -= methodToSubscribe;
    }

    public static void TriggerEvent(EventsType eventType, params object[] parameters)
    {
        if (_events == null) return;

        if (!_events.ContainsKey(eventType)) return;

        if (_events[eventType] == null) return;

        _events[eventType](parameters);
    }

    public static void ClearEvents()
    {
        _events.Clear();
    }
}
