using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static Dictionary<string, Action<object>> eventTable = new();
    

    public static void Subscribe(string eventName, Action<object> listener)
    {
        if (!eventTable.ContainsKey(eventName))
            eventTable[eventName] = delegate { };
        eventTable[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action<object> listener)
    {
        if (eventTable.ContainsKey(eventName))
            eventTable[eventName] -= listener;
    }

    public static void TriggerEvent(string eventName, object parameter = null)
    {
        if (eventTable.ContainsKey(eventName))
            eventTable[eventName].Invoke(parameter);
    }
}

