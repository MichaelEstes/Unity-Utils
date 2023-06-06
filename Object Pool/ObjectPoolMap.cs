using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMap
{
    private Dictionary<string, Stack<GameObject>> objectMap;
    private Transform parent;
    private Transform pooledObjects;
    private ObjectPoolBehaviour poolBehaviour;

    private WaitForEndOfFrame delay;

    public ObjectPoolMap(string poolName = "Pool", Transform parent = null)
    {
        objectMap = new Dictionary<string, Stack<GameObject>>();
        pooledObjects = new GameObject(poolName).transform;
        pooledObjects.parent = parent;
        poolBehaviour = pooledObjects.gameObject.AddComponent<ObjectPoolBehaviour>();
        this.parent = parent;

        delay = new WaitForEndOfFrame();
    }

    public void Dispose(GameObject obj, bool setParent = true)
    {
        string name = obj.name;
        if (!Has(name))
        {
            objectMap[name] = new Stack<GameObject>();
        }

        objectMap[name].Push(obj);
        obj.SetActive(false);
        if (setParent) obj.transform.parent = pooledObjects;
    }

    public void Dispose(ICollection<GameObject> objArr, bool setParent = true, bool clear = true)
    {
        foreach(GameObject obj in objArr)
        {
            Dispose(obj, setParent);
        }

        if (clear) objArr.Clear();
    }

    public void DelayedDispose(GameObject obj, bool setParent = true)
    {
        poolBehaviour.StartCoroutine(Delay(() =>
        {
            Dispose(obj, setParent);
        }));
    }

    public void DelayedDispose(ICollection<GameObject> objArr, bool setParent = true, bool clear = true)
    {
        poolBehaviour.StartCoroutine(Delay(() =>
        {
            Dispose(objArr, setParent, clear);
        }));
    }

    IEnumerator Delay(System.Action callback)
    {
        yield return delay;
        callback();
    }

    public GameObject Get(string name, bool enable = true)
    {
        if (HasItem(name))
        {
            GameObject pooled = objectMap[name].Pop();
            pooled.transform.parent = parent;
            pooled.SetActive(enable);
            return pooled;
        }

        return null;
    }

    public GameObject GetOrCreate(GameObject prefab, Transform parent = null)
    {
        GameObject pooled;
        string key = prefab.name;
        if (HasItem(key))
        {
            pooled = objectMap[key].Pop();
            pooled.transform.parent = parent;
            pooled.SetActive(true);
        } 
        else
        {
            pooled = GameObject.Instantiate(prefab, parent, false);
            pooled.name = key;
        }

        return pooled;
    }

    private bool Has(string name)
    {
        return objectMap.ContainsKey(name);
    }

    public bool HasItem(string name)
    {
        return Has(name) && objectMap[name].Count > 0;
    }
}
