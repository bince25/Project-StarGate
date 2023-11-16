using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    private Queue<GameObject> objectQueue = new Queue<GameObject>();
    private Transform poolParent;

    public void InitializePool(GameObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        poolParent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab, poolParent);
            obj.SetActive(false);
            objectQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (objectQueue.Count == 0)
        {
            GameObject newObj = Instantiate(prefab, poolParent);
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
