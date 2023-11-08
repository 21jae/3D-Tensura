using System;
using System.Collections;
using UnityEngine;

public class BossDashSkill : MonoBehaviour
{
    [Header("보스 스킬 데이터")]
    private CharacterStatManager characterStatManager;
    private Enemy enemy;
    [SerializeField] private SOSkill dashSKill;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject dashSkillPos;
    [SerializeField] private LayerMask layerMask;
    private Transform playerTransform;

    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashSpeed = 3f;

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
        yield return new WaitForSeconds(2.6f);

        Instantiate(dashEffect, dashSkillPos.transform.position, Quaternion.identity);

        Collider[] hitColliders = Physics.OverlapSphere(dashSkillPos.transform.position, 3f, layerMask);

        float damageToDeal = dashSKill.CalculateSkillDamage(characterStatManager.currentData.currentAttackPower);

        foreach (Collider hit in hitColliders)
        {
            IDamageable damageableplayer = playerTransform.GetComponent<IDamageable>();

            if (hit.CompareTag("Player"))
            {
                Debug.Log("hit Thunder");
                damageableplayer.TakeDamage(damageToDeal);
                break;
            }
        }

        yield return new WaitForSeconds(1.5f);
    }
}
