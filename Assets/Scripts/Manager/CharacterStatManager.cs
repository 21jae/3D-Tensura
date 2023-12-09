using System;
using UnityEngine;

public class CharacterStatManager : MonoBehaviour
{
    [Header("플레이어 스탯")]
    [SerializeField] private CharacterStats baseStats;
    [field: SerializeField] public PlayerCurrentStatData currentData { get; private set; }
    public static CharacterStatManager instance;
    public event Action OnStatsChanged;

    private void Awake()
    {
        instance = this;
        InitilizeStats();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            currentData.currentHP *= 1.25f;
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
        OnStatsChanged?.Invoke();
    }

    public void ModifyMaxHealth(float amount)
    {
        currentData.currentMaxHP += amount;
        OnStatsChanged?.Invoke();
    }

    public void ModifyAttackPower(float amount)
    {
        currentData.currentAttackPower += amount;
        OnStatsChanged?.Invoke();
    }

    public void ModifyDefence(float amount)
    {
        currentData.currentDefense += amount;
        OnStatsChanged?.Invoke();
    }

    public float GetCurrentAttackPower()
    {
        return currentData.currentAttackPower;
    }
    public float GetCurrentDefense()
    {
        return currentData.currentDefense;
    }
    public float GetCurrentHP()
    {
        return currentData.currentHP;
    }
}
