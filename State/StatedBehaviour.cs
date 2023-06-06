using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatedBehaviour<T> : MonoBehaviour
{
    [SerializeField]
    private T _state;
    public T state
    {
        get { return _state; }
        set {
            OnStateChange(value, _state);
            _state = value;
        }
    }

    public virtual void OnStateChange(T newState, T oldState)
    {
        //Override me
    }

}
