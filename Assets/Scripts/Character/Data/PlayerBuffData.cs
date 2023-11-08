using UnityEngine;

[System.Serializable]
public class PlayerBuffData
{
    [field: SerializeField] [field: Range(0f, 1f)] public float attackPowerBuffPercentage = 0.6f;
    [field: SerializeField] [field: Range(0f, 60f)] public float buffDuration = 30f;
    [field: SerializeField] public float originalAttackPower {get; set;}
    [field: SerializeField] public float increaseAttackPower { get; set; }
}
