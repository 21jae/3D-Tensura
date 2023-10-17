using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "RPG/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string characterName;
    public float maxHealth;
    public float currentHealth;
    public float attackPower;
    public float defense;
}
