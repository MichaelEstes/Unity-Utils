using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static object lockObj = new object();
    protected static T instance;
    public static T Instance
    {
        get
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        Debug.LogError("Missing singleton object For: " + typeof(T));
                    }
                }

                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this as T;
    }

    private void OnApplicationQuit()
    {
        instance = null;
    }
}
