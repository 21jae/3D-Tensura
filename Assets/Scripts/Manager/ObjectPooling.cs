using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance { get; private set; }
    public List<ObjectPoolData> poolData;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializePool();
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in poolData)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Queue<GameObject> objectQueue = poolDictionary[tag];

            if (objectQueue.Count > 0)
            {
                GameObject objSpawn = objectQueue.Dequeue();
                objSpawn.SetActive(true);
                return objSpawn;
            }
            else
                return null;
        }
        else
            return null;
    }

    public void ReturnObjectToPool(string tag, GameObject obj)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
        else
            Debug.LogWarning("Pool with tag " + tag + " doesn`t exist.");
    }
}
