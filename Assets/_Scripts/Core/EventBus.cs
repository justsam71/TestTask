using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventBus
{
    public static readonly Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();
    public static void Publish<T>(T publishedEvent)
    {
        if (_events.TryGetValue(typeof(T), out var del))
        {
            if (del is Action<T> action)
                action.Invoke(publishedEvent);
        }
    }

    public static void Subscribe<T>(Action<T> listener)
    {
        if (_events.TryGetValue(typeof(T), out var del))
            _events[typeof(T)] = (Action<T>)Delegate.Combine(del, listener);
        else
            _events[typeof(T)] = listener;
    }

    public static void Unsubscribe<T>(Action<T> listener)
    {
        if (_events.TryGetValue(typeof(T), out var del))
        {
            var currentDel = (Action<T>)Delegate.Remove(del, listener);
            if (currentDel == null)
                _events.Remove(typeof(T));
            else
                _events[typeof(T)] = currentDel;
        }
    }

    public static void Clear()
    {
        _events.Clear();
    }
}
