using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "RPG/CharacterSkills")]
public class SOSkill : ScriptableObject
{
    public string skillName;
    public string animationName;
    public Sprite skillIcon;
    public float skillDamageMultiplier; //������ �����
    public float skillCoolTime;

    [TextArea(3,10)]
    [SerializeField] private string skillDescription;

    public float CalculateSkillDamage(float playerAttackPower)
    {
        return playerAttackPower * skillDamageMultiplier;
    }
}
