using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    [SerializeField] private CharacterStats baseStats;
    
    public float currentAttackPower { get; set; }
    public float currentMaxHP {get; set;}
    public float currentHP {get; set;}
    public float currentDefense {get; set;}

    private void Awake()
    {
        currentAttackPower = baseStats.attackPower;
        currentMaxHP = baseStats.maxHealth;
        currentHP = baseStats.currentHealth;
        currentDefense = baseStats.defense;
    }

    private void Start()
    {
        Debug.Log($" MaxHP : {baseStats.maxHealth}, HP : {baseStats.currentHealth}, ATK : {baseStats.attackPower}, DEF : {baseStats.defense}");
    }

    public void ModifyHealth(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, baseStats.maxHealth);
    }

    public void ModifyAttackPower(float amount)
    {
        currentAttackPower += amount;
    } 

    public void ModifyDefence(float amount)
    { 
        currentDefense += amount;
    }
}
