using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "RPG/CharacterSkills")]
public class SOSkill : ScriptableObject
{
    [Tooltip("��ų �������� ������� �����մϴ�.")]
    public float skillDamageMultiplier;
    public float coolTime;

    public string animationName;
    public Sprite skillIcon;

    public float CalculateSkillDamage(float playerAttackPower)
    {
        return playerAttackPower * skillDamageMultiplier;
    }
}
