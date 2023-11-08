using UnityEngine;

[CreateAssetMenu(fileName = "ItemStat", menuName = "New Item/ItemStats")]
public class EquipmentStats : ScriptableObject
{
    public float attackPower;
    public float defense;
    public float health;

    public float percentageUp;
}
