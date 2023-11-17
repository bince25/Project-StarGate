using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, ObjectPool> poolDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        poolDictionary = new Dictionary<string, ObjectPool>();

        foreach (Pool pool in pools)
        {
            GameObject poolHolder = new GameObject(pool.tag + " Pool");
            poolHolder.transform.parent = transform;

            ObjectPool newPool = poolHolder.AddComponent<ObjectPool>();
            newPool.InitializePool(pool.prefab, pool.size, poolHolder.transform);

            poolDictionary.Add(pool.tag, newPool);
        }
    }

    public GameObject GetObject(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        return poolDictionary[tag].GetObject();
    }

    public void ReturnObject(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return;
        }

        poolDictionary[tag].ReturnObject(objectToReturn);
    }

    public void ReturnObject(string tag, GameObject objectToReturn, float time)
    {
        StartCoroutine(ReturnObjectCoroutine(tag, objectToReturn, time));
    }

    private IEnumerator<WaitForSeconds> ReturnObjectCoroutine(string tag, GameObject objectToReturn, float time)
    {
        yield return new WaitForSeconds(time);
        poolDictionary[tag].ReturnObject(objectToReturn);
    }
}
