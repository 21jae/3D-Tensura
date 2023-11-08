using System;
using System.Collections;
using UnityEngine;

public class PredationSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;
    private SkillManager skillManager;

    [Header("��ų ������")]
    [SerializeField] private SOSkill predationSkillData;
    [SerializeField] private GameObject predationPrefab;
    [SerializeField] private GameObject predationPosPrefab;

    #region �ʱ�ȭ �� ������Ʈ
    private void Awake()
    {
        Initialize();
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

    private void Update()
    {
        if (skillManager.skillData.PredationData.isPredationActive)
        {
            if (playerController.isMoving())
            {
                CancelPredation();
            }
            else
            {
                AbsorbObjectInRadius();
            }
        }
    }
    #endregion

    #region ��� ����
    private void AbsorbObjectInRadius()
    {
        //���� ���

        Collider[] objectInRange = Physics.OverlapSphere(playerController.transform.position, skillManager.skillData.PredationData.predationRaidus, playerController.Data.GroundData.LayerData.GroundLayer);

        foreach (Collider obj in objectInRange)
        {
            if (ObjectInPredationAngle(obj))
            {
                FieldEnemy enemy = obj.GetComponent<FieldEnemy>();

                if (enemy != null)
                {
                    ApplyDamageToEnemy(enemy);
                    EnoughAbsorb(enemy, obj);
                }
            }
        }
    }

    private void CancelPredation()
    {
        skillManager.skillData.PredationData.isPredationActive = false;

        if (predationPrefab != null)
        {
            playerController.StopPlayer();
            Destroy(predationPrefab);
        }
    }


    private bool ObjectInPredationAngle(Collider obj)
    {
        Vector3 directionToObject = (obj.transform.position - playerController.transform.position).normalized;      //������ ������Ʈ ������ ���� ���� ���
        float angle = Vector3.Angle(playerController.transform.forward, directionToObject);                         //�÷��̾ �ٶ󺸴� ����� ������Ʈ ���� ������ ����

        return angle < skillManager.skillData.PredationData.PREDATION_ANGLE; //���� ������ �������� ��� ������ ũ�ٸ� ��� �����ϴ�.
    }

    private void ApplyDamageToEnemy(FieldEnemy enemy)
    {
        float damaegeToDeal = predationSkillData.CalculateSkillDamage(playerController.playerStatManager.currentData.currentAttackPower);
        IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();

        if (damageableEnemy != null)
        {
            damageableEnemy.TakeDamage(damaegeToDeal, true);
            Debug.Log($"Predation Deal : {damaegeToDeal}");
        }
    }

    private void EnoughAbsorb(FieldEnemy enemy, Collider obj)
    {
        if (enemy.characterStatManager.currentData.currentHP <= enemy.characterStatManager.currentData.currentMaxHP * 0.3f)
        {
            Vector3 directionToAbsorb = (playerController.transform.position - obj.transform.position).normalized;
            float distancToPlayer = Vector3.Distance(playerController.transform.position, obj.transform.position);
            float absorptionSpeed = (1 - (distancToPlayer / skillManager.skillData.PredationData.predationRaidus)) * skillManager.skillData.PredationData.predationForce;

            obj.transform.position += directionToAbsorb * absorptionSpeed * Time.deltaTime;
            UpdateObjectScale(obj);
        }
    }

    private void UpdateObjectScale(Collider obj)
    {
        float distanceToPredationPos = Vector3.Distance(obj.transform.position, skillManager.skillData.PredationData.predationPosition.transform.position);
        float scaleReduce = Mathf.Clamp(distanceToPredationPos / 10f, 0.1f, 1f);
        obj.transform.localScale = Vector3.one * scaleReduce;

        if (obj.transform.localScale.x <= 0.2f && distanceToPredationPos <= skillManager.skillData.PredationData.THRESHOLD)
        {
            //���� ���

            Destroy(obj.gameObject);
            Debug.Log("����");
        }
    }
    #endregion

    #region ��ų Ȱ��ȭ
    public void ActivateSkill()
    {
        skillManager.skillData.PredationData.isPredationActive = true;
        Instantiate(predationPosPrefab, skillManager.skillData.PredationData.predationPosition.transform.position + new Vector3(0f, -1f, 0f), playerController.transform.rotation);
        Instantiate(predationPrefab, skillManager.skillData.PredationData.predationPosition.transform.position, playerController.transform.rotation);
        StartCoroutine(ActivatePredation());
    }

    private IEnumerator ActivatePredation()
    {
        yield return new WaitForSeconds(skillManager.skillData.PredationData.PREDATION_DURATION);
        skillManager.skillData.PredationData.isPredationActive = false;
    }
    #endregion
}