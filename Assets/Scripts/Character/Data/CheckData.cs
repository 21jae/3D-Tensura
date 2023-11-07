using UnityEngine;

[System.Serializable]
public class CheckData
{
    [field: SerializeField] [field: Range(0f, 1f)] public float groundDistance { get; private set; } = 0.2f;
}
