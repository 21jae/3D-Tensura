using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePoison : MonoBehaviour
{
    public PlayerSlimeController slimeController;

    [SerializeField] private SOSkill poisonSkillData;
    [SerializeField] private GameObject poisonPrefab;
    [SerializeField] private Transform poisonPosPrefab;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float skillRange = 10f;
    [SerializeField] private float projectileSpeed = 20f;

    public void ActivateSkill()
    {
        GameObject poisonParticle = Instantiate(poisonPrefab, poisonPosPrefab.transform.position, Quaternion.identity);

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, skillRange, layerMask); 

        foreach (var enemy in enemiesInRange)
        {
            float damaegeToDeal = poisonSkillData.CalculateSkillDamage(slimeController.playerStatManager.currentAttackPower);
            IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();

            if (damageableEnemy != null)
            {
                damageableEnemy.TakeDamage(damaegeToDeal, true);
            }
        }
    }
}
  
