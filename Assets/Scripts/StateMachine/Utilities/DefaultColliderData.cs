using UnityEngine;

[System.Serializable]
public class DefaultColliderData
{
    [field: SerializeField] public float Height { get; private set; } = 1.7f;
    [field: SerializeField] public float CenterY { get; private set; } = 0.8f;
    [field: SerializeField] public float Radius { get; private set; } = 0.2f;
}
