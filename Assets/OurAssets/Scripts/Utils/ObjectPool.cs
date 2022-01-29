using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private List<T> _pooledObjects;
    private T _referencePrefab;

    public ObjectPool(T prefab, int amount)
    {
        _pooledObjects = new List<T>(amount);
        _referencePrefab = prefab;

        // Spawn the objects
        for (int i = 0; i < amount; ++i)
        {
            T newObj = GameObject.Instantiate(prefab);
            newObj.gameObject.SetActive(false);
            _pooledObjects.Add(newObj);
        }
    }

    public T RequestObject()
    {
        // Find the first not-active object in the pool
        foreach (T obj in _pooledObjects)
        {
            if (!obj.gameObject.activeSelf)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        // If no object was found, add a new one
        T newObj = GameObject.Instantiate(_referencePrefab);
        _pooledObjects.Add(newObj);
        return newObj;
    }

    public T RequestObject(Vector3 position)
    {
        T newObj = RequestObject();
        newObj.transform.position = position;
        return newObj;
    }
}
