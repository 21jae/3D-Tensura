using System;
using System.Collections;
using UnityEngine;

public class BlessingSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;
    private SkillManager skillManager;

    #region 초기화
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerController = FindObjectOfType<PlayerController>();
        skillManager = GetComponent<SkillManager>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }
    #endregion

    #region 버프 처리 메서드
    public void ActivateSkill()
    {
        SoundManager.Instance.PlayBleesingStartSound();
        ActivateBlessing();
    }

    private void ActivateBlessing()
    {
        Vector3 spawnPosition = playerController.transform.position - playerController.transform.forward * skillManager.skillData.BlessingData.offsetDistance;
        Quaternion rotationTowardsPlayer = Quaternion.LookRotation(playerController.transform.position - spawnPosition);

        GameObject blessingIntance = Instantiate(skillManager.skillData.BlessingData.blessingHuman, spawnPosition, rotationTowardsPlayer);
        StartCoroutine(WaitAndBless(blessingIntance, skillManager.skillData.BlessingData.destoryPrefabDuration));
    }

    private IEnumerator WaitAndBless(GameObject blessingIntance, float destoryPrefabDuration)
    {
        yield return new WaitForSeconds(destoryPrefabDuration);
        Destroy(blessingIntance);

        BuffToPlayer();
    }

    private void BuffToPlayer()
    {
        ShowBlessingEffectAtPlayer();

        CalculateAndApplyStatsBuff();

        ShowBuffEffectOnPlayer();

        StartCoroutine(ApplyStatusBuff());
    }

    private void ShowBlessingEffectAtPlayer()
    {
        Instantiate(skillManager.skillData.BlessingData.blessingEffect, playerController.transform.position, Quaternion.identity);
        Debug.Log("체력 및 스테이터스 상승!");
    }
    #endregion

    #region 가호 버프 로직 
    private void CalculateAndApplyStatsBuff()
    {
        //HP 버프
        skillManager.skillData.ChangeStats.modifiedHP = CharacterStatManager.instance.currentData.currentHP * skillManager.skillData.BlessingData.healthBuffPercentage;
        CharacterStatManager.instance.ModifyHealth(skillManager.skillData.ChangeStats.modifiedHP);

        ////공격력 버프
        skillManager.skillData.ChangeStats.originalAttack = CharacterStatManager.instance.currentData.currentAttackPower;
        skillManager.skillData.ChangeStats.modifiedAttack = skillManager.skillData.ChangeStats.originalAttack * skillManager.skillData.BlessingData.attackPowerBuffPercentage;
        CharacterStatManager.instance.ModifyAttackPower(skillManager.skillData.ChangeStats.modifiedAttack);

        ////방어력 버프
        skillManager.skillData.ChangeStats.originalDefense = CharacterStatManager.instance.currentData.currentDefense;
        skillManager.skillData.ChangeStats.modifiedDefense = skillManager.skillData.ChangeStats.originalDefense * skillManager.skillData.BlessingData.defenseBuffPercentage;
        CharacterStatManager.instance.ModifyDefence(skillManager.skillData.ChangeStats.modifiedDefense);

        DebugPlayerStats();
    }

    private void ShowBuffEffectOnPlayer()
    {
        GameObject buffInstance = Instantiate(skillManager.skillData.BlessingData.buffEffect, playerController.transform.position, Quaternion.identity);
        buffInstance.transform.SetParent(playerController.transform);
    }

    private IEnumerator ApplyStatusBuff()
    {
        yield return new WaitForSeconds(skillManager.skillData.BlessingData.buffDuration);
        Debug.Log("가호의 지속시간 종료");

        //공격력 및 방어력 원상 복구
        CharacterStatManager.instance.ModifyAttackPower(-skillManager.skillData.ChangeStats.modifiedAttack);
        CharacterStatManager.instance.ModifyDefence(-skillManager.skillData.ChangeStats.modifiedDefense);

        Debug.Log($" ATK : {CharacterStatManager.instance.currentData.currentAttackPower}, DEF : {CharacterStatManager.instance.currentData.currentDefense}");
    }

    private void DebugPlayerStats()
    {
        Debug.Log($" MaxHP : {playerController.playerStatManager.currentData.currentMaxHP}, " +
                     $" HP : {playerController.playerStatManager.currentData.currentHP}, " +
                    $" ATK : {CharacterStatManager.instance.currentData.currentAttackPower}, " +
                    $" DEF : {CharacterStatManager.instance.currentData.currentDefense} ");
    }
    #endregion
}