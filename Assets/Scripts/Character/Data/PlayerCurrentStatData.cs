using UnityEngine;

[System.Serializable]
public class PlayerCurrentStatData
{
    [field: SerializeField] public float currentAttackPower { get; set; }
    [field: SerializeField] public float currentMaxHP { get; set; }
    [field: SerializeField] public float currentHP { get; set; }
    [field: SerializeField] public float currentDefense { get; set; }
    [field: SerializeField] public float currentSpeed { get; set; }
}
