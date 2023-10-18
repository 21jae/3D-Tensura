using System;
using System.Collections;
using UnityEngine;

public class PredationSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;

    [SerializeField] private SOSkill predationSkillData;
    [SerializeField] private GameObject predationSkillPos;
    [SerializeField] private GameObject predationPrefab;

    [SerializeField] private float predationRaidus;    //��� ����
    [SerializeField] private float predationForce;     //��� �ӵ�
    [SerializeField] private LayerMask layerMask;           //��� ������ ������Ʈ

    private Vector3 directionToPredation;   //��� ����
    private float distanceForce;            //���� �ӵ� ����
    private float angleOfPredation = 45f;   //��� ����
    private float predationDuration = 5f;

    private bool isPredation;

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
        if (isPredation)
        {
            Predation();
        }
    }

    private void Predation()
    {
        //���� �ȿ� �ִ� ������Ʈ�� ��� ������ŭ �����մϴ�.
        Collider[] objectInRange = Physics.OverlapSphere(playerController.transform.position, predationRaidus, layerMask);
        
        foreach (Collider obj in objectInRange)
        {
            Debug.Log($"������ ������Ʈ �̸� : {obj.name}");

            //������ ������Ʈ ������ ���� ���� ���
            Vector3 directionToObject = (obj.transform.position - playerController.transform.position).normalized;

            //�÷��̾ �ٶ󺸴� ����� ������Ʈ ���� ������ ����
            float angle = Vector3.Angle(playerController.transform.forward, directionToObject);

            //������Ʈ�� ��ä�� ����(45��) ���� �ִٸ�
            if (angle < angleOfPredation)
            {
                directionToPredation = (playerController.transform.position - obj.transform.position).normalized;
                float distance = Vector3.Distance(playerController.transform.position, obj.transform.position);

                //��� �ӵ� �Ÿ��� ���� ����
                distanceForce = (1 - (distance / predationRaidus)) * predationForce;
                obj.transform.position += directionToPredation * distanceForce * Time.deltaTime;
                
                EnemyController enemy = obj.GetComponent<EnemyController>();

                if (distance <= 1f)
                {
                    float damaegeToDeal = predationSkillData.CalculateSkillDamage(playerController.playerStatManager.currentAttackPower);

                    if (enemy != null && enemy.enemyStats.currentHealth <= enemy.enemyStats.maxHealth * 0.5f)
                    {
                        IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();
                        if (damageableEnemy != null)
                        {
                            damageableEnemy.TakeDamage(damaegeToDeal);
                            Debug.Log($"���� ������ : {damaegeToDeal}");
                        }
                    }
                }

            }
        }
    }
    public void ActivateSkill()
    {
        ActivatePredation();
        isPredation = true;
        Instantiate(predationPrefab, predationSkillPos.transform.position, playerController.transform.rotation);
        StartCoroutine(ActivatePredation());

    }

    private IEnumerator ActivatePredation()
    {
        yield return new WaitForSeconds(predationDuration);
        isPredation = false;
    }

}