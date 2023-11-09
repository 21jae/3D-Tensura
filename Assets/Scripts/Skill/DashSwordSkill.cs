using System;
using UnityEngine;

public class DashSwordSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;
    private SkillManager skillManager;
    public LayerMask layerMask;

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
        skillManager = GetComponent<SkillManager>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }
    #endregion

    #region 대쉬로직
    private void StartDash()
    {
        if (!skillManager.skillData.DashData.isDashing)
        {
            skillManager.skillData.DashData.SetDashTarget(playerController.transform.position + playerController.transform.forward * skillManager.skillData.DashData.dashDistance);
            skillManager.skillData.DashData.SetIsDashing(true);
        }
    }

    private void HandleDashing()
    {
        if (skillManager.skillData.DashData.isDashing)
        {
            MoveAndDashAttack();
        }
    }

    private void MoveAndDashAttack()
    {
        playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, skillManager.skillData.DashData.dashTarget, skillManager.skillData.DashData.dashSpeed * Time.deltaTime);
        Vector3 direction = (skillManager.skillData.DashData.dashTarget - playerController.transform.position).normalized;
        float distance = Vector3.Distance(playerController.transform.position, skillManager.skillData.DashData.dashTarget);

        if (distance > 0.5f)
        {
            playerController.characterController.Move(direction * skillManager.skillData.DashData.dashSpeed * Time.deltaTime);
        }
        else
        {
            Instantiate(skillManager.skillData.DashData.dashSwordPrefab, playerController.transform.position, playerController.transform.rotation);
            DashDamageInRadius();

            skillManager.skillData.DashData.SetIsDashing(false);
        }

    }

    private void DashDamageInRadius()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(playerController.transform.position, skillManager.skillData.DashData.damageRadius, layerMask);
        float damageToDeal = skillManager.skillData.DashData.dashSwordSkillData.CalculateSkillDamage(CharacterStatManager.instance.currentData.currentAttackPower);

        foreach (Collider enemy in hitEnemies)
        {
            IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();
            if (damageableEnemy != null)
            {
                Debug.Log("대쉬데미지??0");
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