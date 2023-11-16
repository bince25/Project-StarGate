using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // The prefab of the object to pool.
    public int poolSize = 10; // The initial size of the object pool.

    private Queue<GameObject> objectQueue = new Queue<GameObject>();

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            objectQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (objectQueue.Count == 0)
        {
            // If the pool is empty, create a new object.
            GameObject newObj = Instantiate(prefab, transform);
            newObj.SetActive(false);
            return newObj;
        }

        GameObject obj = objectQueue.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        objectQueue.Enqueue(obj);
    }
}
