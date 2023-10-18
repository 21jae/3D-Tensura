using System;
using UnityEngine;

public class DashSwordSkill : MonoBehaviour, ISkill
{
    [SerializeField] private SOSkill dashSwordSkillData;
    public GameObject dashSwordPrefab;
    private PlayerController playerController;

    private float dashDistance = 5.0f;          //대쉬 사거리
    private float dashSpeed = 10.0f;            //대쉬 속도
    [SerializeField] float damageRadius = 8.0f; //공격 범위
    [SerializeField] LayerMask layerMask;

    private bool isDashing;
    private Vector3 dashTarget; //목표 방향

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }


    private void Update()
    {
        Dashing();
    }

    public void ActivateSkill()
    {
        ActivateDash();
    }

    private void ActivateDash()
    {
        if (!isDashing)
        {
            //플레이어가 바라보는 방향으로 설정한 거리만큼 Dash합니다.
            dashTarget = transform.position + transform.forward * dashDistance;
            isDashing = true;
        }
    }

    private void Dashing()
    {
        if (isDashing)
        {
            //현재 위치에서 목표 방향까지 이동
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

            //대쉬를 통해 목표지점에 매우 가까워지면 이펙트를 발생시킵니다
            if (Vector3.Distance(transform.position, dashTarget) < 0.5f)
            {
                Instantiate(dashSwordPrefab, transform.position, transform.rotation);
                DashDamageInRadius();

                isDashing = false;
            }
        }
    }

    private void DashDamageInRadius()
    {
        //이펙트 범위 안에 Enemy만 감지하는 콜라이더와 레이어 생성
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, damageRadius, layerMask);
        
        //플레이어의 현재 공격력을 전달하여 스킬 데미지를 호출합니다.
        float damageToDeal = dashSwordSkillData.CalculateSkillDamage(playerController.playerStatManager.currentAttackPower);

        //생성된 콜라이더 안에 적이 있다면 데미지를 전달합니다.
        foreach (Collider enemy in hitEnemies)
        {
            IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();
            if (damageableEnemy != null)
            {
                damageableEnemy.TakeDamage(damageToDeal);
            }
        }
    }
}
