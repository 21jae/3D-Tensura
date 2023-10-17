using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "RPG/CharacterSkills")]
public class SOSkill : ScriptableObject
{
    [Tooltip("스킬 데미지를 백분율로 설정합니다.")]
    public float skillDamageMultiplier;
    public float coolTime;

    public string animationName;
    public Sprite skillIcon;

    public float CalculateSkillDamage(float playerAttackPower)
    {
        return playerAttackPower * skillDamageMultiplier;
    }
}
