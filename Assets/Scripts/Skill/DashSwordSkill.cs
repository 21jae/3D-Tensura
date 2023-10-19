using System;
using UnityEngine;

public class DashSwordSkill : MonoBehaviour, ISkill
{
    [Header("스킬 데이터")]
    [SerializeField] private SOSkill dashSwordSkillData;
    [SerializeField] private GameObject dashSwordPrefab;

    [Header("대쉬 설정")]
    private float dashDistance = 5.0f;
    private float dashSpeed = 10.0f;

    [Header("공격 설정")]
    [SerializeField] float damageRadius = 8.0f;
    [SerializeField] LayerMask layerMask;

    private PlayerController playerController;
    private Vector3 dashTarget;
    private bool isDashing;

    #region 초기화 및 업데이트
    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        HandleDashing();
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

    #region 대쉬로직
    private void StartDash()
    {
        if (!isDashing)
        {
            dashTarget = playerController.transform.position + playerController.transform.forward * dashDistance;
            isDashing = true;
        }
    }

    private void HandleDashing()
    {
        if (isDashing)
        {
            MoveAndDashAttack();
        }
    }

    private void MoveAndDashAttack()
    {
        playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, dashTarget, dashSpeed * Time.deltaTime);

        if (Vector3.Distance(playerController.transform.position, dashTarget) < 0.5f)
        {
            Instantiate(dashSwordPrefab, playerController.transform.position, playerController.transform.rotation);
            DashDamageInRadius();

            isDashing = false;
        }
    }

    private void DashDamageInRadius()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(playerController.transform.position, damageRadius, layerMask);
        float damageToDeal = dashSwordSkillData.CalculateSkillDamage(playerController.playerStatManager.currentAttackPower);

        foreach (Collider enemy in hitEnemies)
        {
            IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();
            if (damageableEnemy != null)
            {
                damageableEnemy.TakeDamage(damageToDeal);
            }
        }
    }
    #endregion

    #region 스킬 활성화
    public void ActivateSkill()
    {
        StartDash();
    }
    #endregion 
}