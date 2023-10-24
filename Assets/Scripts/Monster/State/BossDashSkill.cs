using System;
using System.Collections;
using UnityEngine;

public class BossDashSkill : MonoBehaviour
{
    [Header("보스스킬 데이터")]
    private CharacterStatManager characterStatManager;
    private Enemy enemy;
    [SerializeField] private SOSkill dashSKill;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private LayerMask layerMask;
    private Transform playerTransform;

    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashSpeed = 5f;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        characterStatManager = GetComponent<CharacterStatManager>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
        enemy = GetComponent<Enemy>();
    }
    
    public void BossDash()
    {
        StartCoroutine(DashStrike());
    }

    private IEnumerator DashStrike()
    {
        Vector3 directionToPlayer = (playerTransform.position -enemy.transform.position).normalized;
        
        // 대쉬 동작을 시작합니다.
        enemy.animator.SetBool("isDashing", true);

        float distanceCovered = 0f;
        while (distanceCovered < dashDistance)
        {
            float moveDistance = dashSpeed * Time.deltaTime;
            transform.position += directionToPlayer * moveDistance;
            distanceCovered += moveDistance;
            yield return null;
        }
        Instantiate(dashEffect, transform.position + Vector3.forward, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        enemy.animator.SetBool("isDashing", false);
    }
}
