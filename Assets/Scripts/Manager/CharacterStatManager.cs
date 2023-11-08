using System.Collections.Generic;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    [Header("플레이어 스탯")]
    [SerializeField] private CharacterStats baseStats;
    [field: SerializeField] public PlayerCurrentStatData currentData { get; private set; }

    public static CharacterStatManager instance;

    private void Awake()
    {
        instance = this;

        InitilizeStats();
    }

    private void InitilizeStats()
    {
        currentData.currentAttackPower = baseStats.attackPower;
        currentData.currentMaxHP = baseStats.maxHealth;
        currentData.currentHP = baseStats.currentHealth;
        currentData.currentDefense = baseStats.defense;
        currentData.currentSpeed = baseStats.speed;
    }

    private void Start()
    {
        Debug.Log($" MaxHP : {baseStats.maxHealth}, HP : {baseStats.currentHealth}, ATK : {baseStats.attackPower}, DEF : {baseStats.defense}");
    }

    public void ModifyHealth(float amount)
    {
        currentData.currentHP += amount;
        currentData.currentHP = Mathf.Clamp(currentData.currentHP, 0, baseStats.maxHealth);
    }

    public void ModifyMaxHealth(float amount)
    {
        currentData.currentMaxHP += amount;
    }

    public void ModifyAttackPower(float amount)
    {
        currentData.currentAttackPower += amount;
    }

    public void ModifyDefence(float amount)
    {
        currentData.currentDefense += amount;
    }
}
