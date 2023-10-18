using System;
using System.Collections;
using UnityEngine;

public class PredationSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;

    [SerializeField] private SOSkill predationSkillData;
    [SerializeField] private GameObject predationSkillPos;
    [SerializeField] private GameObject predationPrefab;

    [SerializeField] private float predationRaidus;    //흡수 범위
    [SerializeField] private float predationForce;     //흡수 속도
    [SerializeField] private LayerMask layerMask;           //흡수 가능한 오브젝트

    private Vector3 directionToPredation;   //흡수 방향
    private float distanceForce;            //흡입 속도 조절
    private float angleOfPredation = 45f;   //흡수 각도
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
        //범위 안에 있는 오브젝트를 흡수 범위만큼 감지합니다.
        Collider[] objectInRange = Physics.OverlapSphere(playerController.transform.position, predationRaidus, layerMask);
        
        foreach (Collider obj in objectInRange)
        {
            Debug.Log($"감지된 오브젝트 이름 : {obj.name}");

            //감지된 오브젝트 사이의 방향 각도 계산
            Vector3 directionToObject = (obj.transform.position - playerController.transform.position).normalized;

            //플레이어가 바라보는 방향과 오브젝트 방향 사이의 각도
            float angle = Vector3.Angle(playerController.transform.forward, directionToObject);

            //오브젝트가 부채꼴 범위(45도) 내에 있다면
            if (angle < angleOfPredation)
            {
                directionToPredation = (playerController.transform.position - obj.transform.position).normalized;
                float distance = Vector3.Distance(playerController.transform.position, obj.transform.position);

                //흡수 속도 거리에 따라 조절
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
                            Debug.Log($"오의 데미지 : {damaegeToDeal}");
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