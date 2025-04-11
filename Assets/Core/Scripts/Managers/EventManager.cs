using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    private static readonly Dictionary<string, Action<object>> eventTable = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Subscribe(string eventName, Action<object> listener)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("[EventManager] Tried to subscribe with null or empty event name.");
            return;
        }

        if (!eventTable.ContainsKey(eventName))
            eventTable[eventName] = delegate { };

        eventTable[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action<object> listener)
    {
        if (string.IsNullOrEmpty(eventName) || !eventTable.ContainsKey(eventName))
            return;

        eventTable[eventName] -= listener;
    }

    public static void TriggerEvent(string eventName, object parameter = null)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("[EventManager] Tried to trigger an event with null or empty name.");
            return;
        }

        if (eventTable.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent?.Invoke(parameter);
        }
        else
        {
            Debug.LogWarning($"[EventManager] No listeners for event: {eventName}");
        }
    }
}
