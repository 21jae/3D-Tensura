using System;
using System.Collections;
using UnityEngine;

public class BlessingSkill : MonoBehaviour, ISkill
{
    public GameObject blessingHuman;
    public GameObject blessingEffect;
    public GameObject buffEffect;
    private float offsetDistance = 2f;
    private float destoryPrefab = 4f;

    /// <summary>
    /// �������ͽ� ���
    /// </summary>
    private PlayerController playerController;

    private float healthBuff = 0.3f;        //30%����
    private float attackPowerBuff = 0.2f;   //15%����
    private float defenseBuff = 0.2f;       //20%����
    private float buffDuration = 10f;       //���� ���ӽð�
    private float originalAttackPower;      //�⺻ ATK ��
    private float originalDefense;          //�⺻ DEF ��

    //������ų ���� ��
    private float modifiedHP;               //���� �� HP ��
    private float modifiedAttackPower;      //���� �� ATK ��
    private float modifiedDefense;          //���� �� DEF ��


    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }

    public void ActivateSkill()
    {
        ActivateBless();
    }

    private void ActivateBless()
    {
        //
        Vector3 spawnPosition = playerController.transform.position - playerController.transform.forward * offsetDistance;
        Quaternion rotationTowardsPlayer = Quaternion.LookRotation(playerController.transform.position - spawnPosition);

        GameObject bless = Instantiate(blessingHuman, spawnPosition, rotationTowardsPlayer);
        StartCoroutine(WaitAndBless(bless, destoryPrefab));
    }

    private IEnumerator WaitAndBless(GameObject blessing, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(blessing);
        BuffToPlayer();
    }

    private void BuffToPlayer()
    {
        Instantiate(blessingEffect, playerController.transform.position, Quaternion.identity);
        Debug.Log("ü�� �� �������ͽ� ���!");

        //���� ü�� ������ ��� �� ���� ����
        modifiedHP = playerController.playerStatManager.currentHP * healthBuff;
        playerController.playerStatManager.ModifyHealth(modifiedHP);

        //���� �� ���ݷ� ������ ���
        originalAttackPower = playerController.playerStatManager.currentAttackPower;    
        //���ݷ� ������ ��� �� ���� ����
        modifiedAttackPower = originalAttackPower * attackPowerBuff;
        playerController.playerStatManager.ModifyAttackPower(modifiedAttackPower);

        //���� �� ���� ������ ���
        originalDefense = playerController.playerStatManager.currentDefense;

        //���� ���� ��� �� ���� ����
        modifiedDefense = originalDefense * defenseBuff;
        playerController.playerStatManager.ModifyDefence(modifiedDefense);

        Debug.Log($" MaxHP : {playerController.playerStatManager.currentMaxHP}, " + $"HP : {playerController.playerStatManager.currentHP}," +
            $"ATK : {playerController.playerStatManager.currentAttackPower}, DEF : {playerController.playerStatManager.currentDefense}");

        GameObject buff = Instantiate(buffEffect, playerController.transform.position, Quaternion.identity);
        buff.transform.SetParent(playerController.transform);

        StartCoroutine(ApplyStatusBuff());
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
}