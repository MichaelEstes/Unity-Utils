using System;
using System.Collections.Generic;

namespace Scrim
{
    public static class Events<T> where T : Delegate
    {
        class EventContainer
        {
            public EventContainer(T callback, bool once = false)
            {
                this.callback = callback;
                this.once = once;
            }

            public readonly T callback;
            public readonly bool once;
        }

        // This set will allow multiple of essentially the same EventContainer (Same callback, same once value), need to do more 
        // research around how to make a unique hashcode of a delgate depending on the method and target
        private static Dictionary<string, HashSet<EventContainer>> listeners = new Dictionary<string, HashSet<EventContainer>>();

        public static void Emit(string name, params object[] args)
        {
            if (listeners.TryGetValue(name, out HashSet<EventContainer> toInvoke))
            {
                List<EventContainer> toRemove = new List<EventContainer>();
                foreach (var listener in toInvoke)
                {
                    listener.callback?.Method.Invoke(listener.callback.Target, args);
                    if (listener.once) toRemove.Add(listener);
                }

                foreach (var listener in toRemove)
                {
                    toInvoke.Remove(listener);
                }
            }
        }

        static public void On(string eventName, T callback)
        {
            AddListener(eventName, callback, false);
        }

        static public void Once(string eventName, T callback)
        {
            AddListener(eventName, callback, true);
        }

        private static void AddListener(string eventName, T callback, bool once)
        {
            if (!listeners.ContainsKey(eventName))
            {
                listeners[eventName] = new HashSet<EventContainer>();
            }

            EventContainer gameEvent = new EventContainer(callback, once);
            listeners[eventName].Add(gameEvent);
        }

        static public void Remove(string eventName, T callback)
        {
            if (listeners.ContainsKey(eventName))
            {
                var toInvoke = listeners[eventName];
                toInvoke.RemoveWhere(listener => listener.callback == callback);
            }
        }

        static public void RemoveAll(string eventName)
        {
            listeners.Remove(eventName);
        }
    }
}
