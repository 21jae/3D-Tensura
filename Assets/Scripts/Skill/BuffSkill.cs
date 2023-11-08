using System.Collections;
using UnityEngine;

public class BuffSkill : MonoBehaviour, ISkill
{
    public GameObject attackBuffPrefab;
    private PlayerController playerController;
    private SkillManager skillManager;

    #region 초기화
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        skillManager = GetComponent<SkillManager>();
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }
    #endregion

    #region 스킬 활성화 및 버프 로직
    public void ActivateSkill()
    {
        ActivateTime();
    }

    private void ActivateTime()
    {
        GameObject buffInstance = Instantiate(attackBuffPrefab, playerController.transform.position, Quaternion.identity);
        buffInstance.transform.SetParent(playerController.transform);

        StartCoroutine(ApplyAttackBuff());
    }

    private IEnumerator ApplyAttackBuff()
    {
        CalculateAndApplyAttackPower();

        yield return new WaitForSeconds(skillManager.skillData.BuffData.buffDuration);

        ResetAttackPowerToOriginal();
    }

    private void CalculateAndApplyAttackPower()
    {
        skillManager.skillData.ChangeStats.originalAttack = CharacterStatManager.instance.currentData.currentAttackPower;
        skillManager.skillData.ChangeStats.modifiedAttack = skillManager.skillData.ChangeStats.originalAttack * skillManager.skillData.BuffData.attackPowerBuffPercentage;
        CharacterStatManager.instance.ModifyAttackPower(skillManager.skillData.ChangeStats.modifiedAttack);
        Debug.Log($" ATK : {CharacterStatManager.instance.currentData.currentAttackPower}");
    }

    private void ResetAttackPowerToOriginal()
    {
        CharacterStatManager.instance.ModifyAttackPower(-skillManager.skillData.ChangeStats.modifiedAttack);
        Debug.Log($" ATK : {CharacterStatManager.instance.currentData.currentAttackPower}");
    }
    #endregion
}