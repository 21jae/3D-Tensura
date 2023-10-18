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
    /// 스테이터스 상승
    /// </summary>
    private PlayerController playerController;

    private float healthBuff = 0.3f;        //30%증가
    private float attackPowerBuff = 0.2f;   //15%증가
    private float defenseBuff = 0.2f;       //20%증가
    private float buffDuration = 10f;       //버프 지속시간
    private float originalAttackPower;      //기본 ATK 값
    private float originalDefense;          //기본 DEF 값

    //증가시킬 스탯 값
    private float modifiedHP;               //증가 될 HP 값
    private float modifiedAttackPower;      //증가 될 ATK 값
    private float modifiedDefense;          //증가 될 DEF 값


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
        Debug.Log("체력 및 스테이터스 상승!");

        //현재 체력 증가량 계산 및 증가 적용
        modifiedHP = playerController.playerStatManager.currentHP * healthBuff;
        playerController.playerStatManager.ModifyHealth(modifiedHP);

        //버프 전 공격력 증가량 계산
        originalAttackPower = playerController.playerStatManager.currentAttackPower;    
        //공격력 증가량 계산 및 증가 적용
        modifiedAttackPower = originalAttackPower * attackPowerBuff;
        playerController.playerStatManager.ModifyAttackPower(modifiedAttackPower);

        //버프 전 방어력 증가량 계산
        originalDefense = playerController.playerStatManager.currentDefense;

        //방어력 증가 계산 및 증가 적용
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
        Debug.Log("가호의 지속시간 종료");

        //공격력 및 방어력 원상 복구
        playerController.playerStatManager.ModifyAttackPower(-modifiedAttackPower);
        playerController.playerStatManager.ModifyDefence(-modifiedDefense);

        Debug.Log($" ATK : {playerController.playerStatManager.currentAttackPower}, DEF : {playerController.playerStatManager.currentDefense}");
    }
}