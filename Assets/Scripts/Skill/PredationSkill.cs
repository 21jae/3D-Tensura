using System;
using System.Collections;
using UnityEngine;

public class PredationSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;

    [Header("��ų ������")]
    [SerializeField] private SOSkill predationSkillData;
    [SerializeField] private GameObject predationPrefab;

    [Header("��� ����")]
    [SerializeField] private GameObject predationPosition;
    [SerializeField] private float predationRaidus;
    [SerializeField] private float predationForce;
    [SerializeField] private LayerMask layerMask;

    //��� ����, ���ӽð�, ����
    private const float PREDATION_ANGLE = 60f;
    private const float PREDATION_DURATION = 5f;
    private const float THRESHOLD = 3f; 

    private bool isPredationActive;

    #region �ʱ�ȭ �� ������Ʈ
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }

    private void Update()
    {
        if (isPredationActive)
        {
            AbsorbObjectInRadius();
        }
    }
    #endregion

    #region ��� ����
    private void AbsorbObjectInRadius()
    {
        Collider[] objectInRange = Physics.OverlapSphere(playerController.transform.position, predationRaidus, layerMask);

        foreach (Collider obj in objectInRange)
        {
            if (ObjectInPredationAngle(obj))
            {
                EnemyController enemy = obj.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    ApplyDamageToEnemy(enemy);
                    EnoughAbsorb(enemy, obj);
                }
            }
        }
    }

    private bool ObjectInPredationAngle(Collider obj)
    {
        Vector3 directionToObject = (obj.transform.position - playerController.transform.position).normalized;      //������ ������Ʈ ������ ���� ���� ���
        float angle = Vector3.Angle(playerController.transform.forward, directionToObject);                         //�÷��̾ �ٶ󺸴� ����� ������Ʈ ���� ������ ����

        return angle < PREDATION_ANGLE; //���� ������ �������� ��� ������ ũ�ٸ� ��� �����ϴ�.
    }

    private void ApplyDamageToEnemy(EnemyController enemy)
    {
        float damaegeToDeal = predationSkillData.CalculateSkillDamage(playerController.playerStatManager.currentAttackPower);
        IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();

        if (damageableEnemy != null)
        {
            damageableEnemy.TakeDamage(damaegeToDeal, true);
            Debug.Log($"���� ������ : {damaegeToDeal}");
        }
    }

    private void EnoughAbsorb(EnemyController enemy, Collider obj)
    {
        if (enemy.enemyStats.currentHealth <= enemy.enemyStats.maxHealth * 0.4f)
        {
            Vector3 directionToAbsorb = (playerController.transform.position - obj.transform.position).normalized;
            float distancToPlayer = Vector3.Distance(playerController.transform.position, obj.transform.position);
            float absorptionSpeed = (1 - (distancToPlayer / predationRaidus)) * predationForce;

            obj.transform.position += directionToAbsorb * absorptionSpeed * Time.deltaTime;
            UpdateObjectScale(obj);
        }
    }

    private void UpdateObjectScale(Collider obj)
    {
        float distanceToPredationPos = Vector3.Distance(obj.transform.position, predationPosition.transform.position);
        float scaleReduce = Mathf.Clamp(distanceToPredationPos / 10f, 0.1f, 1f);
        obj.transform.localScale = Vector3.one * scaleReduce;

        if (obj.transform.localScale.x <= 0.2f && distanceToPredationPos <= THRESHOLD)
        {
            Destroy(obj.gameObject);
            Debug.Log("����");    
        }
    }
    #endregion

    #region ��ų Ȱ��ȭ
    public void ActivateSkill()
    {
        isPredationActive = true;
        Instantiate(predationPrefab, predationPosition.transform.position, playerController.transform.rotation);
        StartCoroutine(ActivatePredation());
    }

    private IEnumerator ActivatePredation()
    {
        yield return new WaitForSeconds(PREDATION_DURATION);
        isPredationActive =false;
    }   
    #endregion
}