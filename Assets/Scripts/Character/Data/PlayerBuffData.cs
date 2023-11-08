using UnityEngine;

[System.Serializable]
public class PlayerBuffData
{
    [field: SerializeField] [field: Range(0f, 1f)] public float attackPowerBuffPercentage { get; private set; } = 0.6f;
    [field: SerializeField] [field: Range(0f, 60f)] public float buffDuration { get; private set; } = 30f;
}
