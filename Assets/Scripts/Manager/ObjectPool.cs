using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}


public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance { get ; private set; }

    //public GameObject objectToPool; //������Ʈ ������
    //public int amountToPool;    //Ǯ�� ������ ������Ʈ ��
    //private List<GameObject> pooledObjects; //Ǯ���� ������Ʈ ����Ʈ

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        InitializePool();
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
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
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn`t exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnObjectToPool(string tag, GameObject obj)
    {
        if (poolDictionary.ContainsKey(tag)) 
        {
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn`t exist.");
        }
    }
}
