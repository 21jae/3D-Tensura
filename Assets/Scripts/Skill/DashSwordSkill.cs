using System;
using UnityEngine;

public class DashSwordSkill : MonoBehaviour
{
    [SerializeField] private SOSkill dashSwordSkillData;
    public GameObject dashSwordPrefab;
    private PlayerController playerController;

    private float dashDistance = 5.0f;
    private float dashSpeed = 10.0f;
    [SerializeField] float damageRadius = 8.0f;
    [SerializeField] LayerMask layerMask;

    private bool isDashing;
    private Vector3 dashTarget;

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
        if (isDashing)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, dashTarget) < 0.5f)
            {
                Instantiate(dashSwordPrefab, transform.position, transform.rotation);

                DashDamageInRadius();

                isDashing = false;
            }
        }

    }

    public void ActivateDash()
    {
        if (!isDashing) 
        { 
            dashTarget = transform.position + transform.forward * dashDistance;

            isDashing = true;
        }
    }

    private void DashDamageInRadius()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, damageRadius, layerMask);
        float damageToDeal = dashSwordSkillData.CalculateSkillDamage(playerController.currentAttackPower);

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
