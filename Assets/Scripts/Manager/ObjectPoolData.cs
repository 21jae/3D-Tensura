using UnityEngine;

[System.Serializable]
public class ObjectPoolData
{
    [SerializeField] public string tag;
    [SerializeField] public GameObject prefab;
    [SerializeField] public int size;
}
