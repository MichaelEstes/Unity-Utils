using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class EventEmitter
{ 
    static EventEmitter()
    {
        Debug.Log("Event Emitter Initialized");
    }

    private static readonly Dictionary<string, HashSet<GameEvent>> listeners = new Dictionary<string, HashSet<GameEvent>>();

    static public void Emit(string eventName, MonoBehaviour context, params object[] args)
    {
        if (listeners.ContainsKey(eventName))
        {
            var toInvoke = listeners[eventName];
            foreach (var listener in toInvoke)
            {
                listener.action?.Invoke(context, args);
            }
            toInvoke.RemoveWhere(listener => listener.once);
        }
    }

    static public void On(string eventName, GameEvent.EventAction callback)
    {
        AddListener(eventName, callback, false);
    }

    static public void Once(string eventName, GameEvent.EventAction callback)
    {
        AddListener(eventName, callback, true);
    }

    private static void AddListener(string eventName, GameEvent.EventAction callback, bool once)
    {
        if (!listeners.ContainsKey(eventName))
        {
            listeners[eventName] = new HashSet<GameEvent>();
        }

        GameEvent gameEvent = new GameEvent(callback, once);
        listeners[eventName].Add(gameEvent);
    }

    static public void Remove(string eventName, GameEvent.EventAction callback)
    {
        if (listeners.ContainsKey(eventName))
        {
            var toInvoke = listeners[eventName];
            toInvoke.RemoveWhere(listener => listener.action == callback);
        }
    }

    static public void RemoveAll(string eventName)
    {
        listeners.Remove(eventName);
    }
}
