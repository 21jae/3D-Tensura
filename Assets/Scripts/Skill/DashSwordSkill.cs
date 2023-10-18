using System;
using UnityEngine;

public class DashSwordSkill : MonoBehaviour, ISkill
{
    [SerializeField] private SOSkill dashSwordSkillData;
    public GameObject dashSwordPrefab;
    private PlayerController playerController;

    private float dashDistance = 5.0f;          //�뽬 ��Ÿ�
    private float dashSpeed = 10.0f;            //�뽬 �ӵ�
    [SerializeField] float damageRadius = 8.0f; //���� ����
    [SerializeField] LayerMask layerMask;

    private bool isDashing;
    private Vector3 dashTarget; //��ǥ ����

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
            //�÷��̾ �ٶ󺸴� �������� ������ �Ÿ���ŭ Dash�մϴ�.
            dashTarget = transform.position + transform.forward * dashDistance;
            isDashing = true;
        }
    }

    private void Dashing()
    {
        if (isDashing)
        {
            //���� ��ġ���� ��ǥ ������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

            //�뽬�� ���� ��ǥ������ �ſ� ��������� ����Ʈ�� �߻���ŵ�ϴ�
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
        //����Ʈ ���� �ȿ� Enemy�� �����ϴ� �ݶ��̴��� ���̾� ����
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, damageRadius, layerMask);
        
        //�÷��̾��� ���� ���ݷ��� �����Ͽ� ��ų �������� ȣ���մϴ�.
        float damageToDeal = dashSwordSkillData.CalculateSkillDamage(playerController.playerStatManager.currentAttackPower);

        //������ �ݶ��̴� �ȿ� ���� �ִٸ� �������� �����մϴ�.
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
