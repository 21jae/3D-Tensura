using System;
using System.Collections;
using UnityEngine;

public class PredationSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;

    [Header("스킬 데이터")]
    [SerializeField] private SOSkill predationSkillData;
    [SerializeField] private GameObject predationPrefab;

    [Header("흡수 설정")]
    [SerializeField] private GameObject predationPosition;
    [SerializeField] private float predationRaidus;
    [SerializeField] private float predationForce;
    [SerializeField] private LayerMask layerMask;

    //흡수 각도, 지속시간, 범위
    private const float PREDATION_ANGLE = 60f;
    private const float PREDATION_DURATION = 5f;
    private const float THRESHOLD = 3f; 

    private bool isPredationActive;

    #region 초기화 및 업데이트
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

    #region 흡수 로직
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
        Vector3 directionToObject = (obj.transform.position - playerController.transform.position).normalized;      //감지된 오브젝트 사이의 방향 각도 계산
        float angle = Vector3.Angle(playerController.transform.forward, directionToObject);                         //플레이어가 바라보는 방향과 오브젝트 방향 사이의 각도

        return angle < PREDATION_ANGLE; //방향 사이의 각도보다 흡수 각도가 크다면 흡수 가능하다.
    }

    private void ApplyDamageToEnemy(EnemyController enemy)
    {
        float damaegeToDeal = predationSkillData.CalculateSkillDamage(playerController.playerStatManager.currentAttackPower);
        IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();

        if (damageableEnemy != null)
        {
            damageableEnemy.TakeDamage(damaegeToDeal, true);
            Debug.Log($"포식 데미지 : {damaegeToDeal}");
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
            Debug.Log("포식");    
        }
    }
    #endregion

    #region 스킬 활성화
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