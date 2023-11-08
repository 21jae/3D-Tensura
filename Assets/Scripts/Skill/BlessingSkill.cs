using System;
using System.Collections;
using UnityEngine;

public class BlessingSkill : MonoBehaviour, ISkill
{
    [Header("가호 프리팹")]
    public GameObject blessingHuman;
    public GameObject blessingEffect;
    public GameObject buffEffect;

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
        ActivateBlessing();
    }

    private void ActivateBlessing()
    {
        Vector3 spawnPosition = playerController.transform.position - playerController.transform.forward * skillManager.skillData.BlessingData.offsetDistance;
        Quaternion rotationTowardsPlayer = Quaternion.LookRotation(playerController.transform.position - spawnPosition);

        GameObject blessingIntance = Instantiate(blessingHuman, spawnPosition, rotationTowardsPlayer);
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
        Instantiate(blessingEffect, playerController.transform.position, Quaternion.identity);
        Debug.Log("체력 및 스테이터스 상승!");
    }
    #endregion

    #region 가호 버프 로직 
    private void CalculateAndApplyStatsBuff()
    {
        //HP 버프
        skillManager.skillData.BlessingData.modifiedHP = CharacterStatManager.instance.currentData.currentHP * skillManager.skillData.BlessingData.healthBuffPercentage;
        CharacterStatManager.instance.ModifyHealth(skillManager.skillData.BlessingData.modifiedHP);

        //공격력 버프
        skillManager.skillData.BlessingData.originalAttackPower = CharacterStatManager.instance.currentData.currentAttackPower;
        skillManager.skillData.BlessingData.modifiedAttackPower = skillManager.skillData.BlessingData.originalAttackPower * skillManager.skillData.BlessingData.attackPowerBuffPercentage;
        CharacterStatManager.instance.ModifyAttackPower(skillManager.skillData.BlessingData.modifiedAttackPower);

        //방어력 버프
        skillManager.skillData.BlessingData.originalDefense = CharacterStatManager.instance.currentData.currentDefense;
        skillManager.skillData.BlessingData.modifiedDefense = skillManager.skillData.BlessingData.originalDefense * skillManager.skillData.BlessingData.defenseBuffPercentage;
        CharacterStatManager.instance.ModifyDefence(skillManager.skillData.BlessingData.modifiedDefense);

        DebugPlayerStats();
    }

    private void ShowBuffEffectOnPlayer()
    {
        GameObject buffInstance = Instantiate(buffEffect, playerController.transform.position, Quaternion.identity);
        buffInstance.transform.SetParent(playerController.transform);
    }

    private IEnumerator ApplyStatusBuff()
    {
        yield return new WaitForSeconds(skillManager.skillData.BlessingData.buffDuration);
        Debug.Log("가호의 지속시간 종료");

        //공격력 및 방어력 원상 복구
        CharacterStatManager.instance.ModifyAttackPower(-skillManager.skillData.BlessingData.modifiedAttackPower);
        CharacterStatManager.instance.ModifyDefence(-skillManager.skillData.BlessingData.modifiedDefense);

        Debug.Log($" ATK : {CharacterStatManager.instance.currentData.currentAttackPower}, DEF : {CharacterStatManager.instance.currentData.currentDefense}");
    }

    private void DebugPlayerStats()
    {
        Debug.Log($" MaxHP : {CharacterStatManager.instance.currentData.currentAttackPower}, " +
                     $" HP : {CharacterStatManager.instance.currentData.currentAttackPower}, " +
                    $" ATK : {CharacterStatManager.instance.currentData.currentAttackPower}, " +
                    $" DEF : {CharacterStatManager.instance.currentData.currentAttackPower} ");
    }
    #endregion
}