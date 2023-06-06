using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateEnum<T> where T : System.Enum
{
    private T _state;

    public T state
    {
        get { return _state; }
        set
        {
            OnStateChanged(value, _state);
            _state = value;
        }
    }

    public System.Action<T,T> OnStateChanged;
}
