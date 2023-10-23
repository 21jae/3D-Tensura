using System;
using System.Collections;
using UnityEngine;

public class BlessingSkill : MonoBehaviour, ISkill
{
    [Header("��ȣ ������")]
    public GameObject blessingHuman;
    public GameObject blessingEffect;
    public GameObject buffEffect;

    [Header("��ȣ ����")]
    private float offsetDistance = 2f;
    private float destoryPrefabDuration = 4f;
    private float buffDuration = 10f;

    [Header("���� ����")]
    private float healthBuffPercentage = 0.3f;
    private float attackPowerBuffPercentage = 0.2f;
    private float defenseBuffPercentage = 0.2f;

    private PlayerController playerController;
    private float originalAttackPower;
    private float originalDefense;
    private float modifiedHP;
    private float modifiedAttackPower;
    private float modifiedDefense;

    #region �ʱ�ȭ
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }
    #endregion

    #region ���� ó�� �޼���
    public void ActivateSkill()
    {
        ActivateBlessing();
    }

    private void ActivateBlessing()
    {
        Vector3 spawnPosition = playerController.transform.position - playerController.transform.forward * offsetDistance;
        Quaternion rotationTowardsPlayer = Quaternion.LookRotation(playerController.transform.position - spawnPosition);

        GameObject blessingIntance = Instantiate(blessingHuman, spawnPosition, rotationTowardsPlayer);
        StartCoroutine(WaitAndBless(blessingIntance, destoryPrefabDuration));
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
        Debug.Log("ü�� �� �������ͽ� ���!");
    }
    #endregion

    #region ��ȣ ���� ���� 
    private void CalculateAndApplyStatsBuff()
    {
        //HP ����
        modifiedHP = playerController.playerStatManager.currentHP * healthBuffPercentage;
        playerController.playerStatManager.ModifyHealth(modifiedHP);

        //���ݷ� ����
        originalAttackPower = playerController.playerStatManager.currentAttackPower;
        modifiedAttackPower = originalAttackPower * attackPowerBuffPercentage;
        playerController.playerStatManager.ModifyAttackPower(modifiedAttackPower);

        //���� ����
        originalDefense = playerController.playerStatManager.currentDefense;
        modifiedDefense = originalDefense * defenseBuffPercentage;
        playerController.playerStatManager.ModifyDefence(modifiedDefense);

        DebugPlayerStats();
    }

    private void ShowBuffEffectOnPlayer()
    {
        GameObject buffInstance = Instantiate(buffEffect, playerController.transform.position, Quaternion.identity);
        buffInstance.transform.SetParent(playerController.transform);
    }

    private IEnumerator ApplyStatusBuff()
    {
        yield return new WaitForSeconds(buffDuration);
        Debug.Log("��ȣ�� ���ӽð� ����");

        //���ݷ� �� ���� ���� ����
        playerController.playerStatManager.ModifyAttackPower(-modifiedAttackPower);
        playerController.playerStatManager.ModifyDefence(-modifiedDefense);

        Debug.Log($" ATK : {playerController.playerStatManager.currentAttackPower}, DEF : {playerController.playerStatManager.currentDefense}");
    }

    private void DebugPlayerStats()
    {
        Debug.Log($" MaxHP : {playerController.playerStatManager.currentMaxHP}, " +
                     $" HP : {playerController.playerStatManager.currentHP}, " +
                    $" ATK : {playerController.playerStatManager.currentAttackPower}, " +
                    $" DEF : {playerController.playerStatManager.currentDefense} ");
    }
    #endregion
}