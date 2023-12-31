using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PredationSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;
    private SkillManager skillManager;
    public LayerMask layerMask;


    #region 초기화 및 업데이트
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerController = FindObjectOfType<PlayerController>();
        skillManager = GetComponent<SkillManager>();

        if (playerController == null)
            Debug.LogError("Not found playerController");
    }

    private void Update()
    {
        if (skillManager.skillData.PredationData.isPredationActive)
        {
            AbsorbObjectInRadius();
        }
    }
    #endregion

    #region 흡수 로직
    private void AbsorbObjectInRadius()
    {
        Collider[] objectInRange = Physics.OverlapSphere(playerController.transform.position, 
            skillManager.skillData.PredationData.predationRaidus, layerMask);
        foreach (Collider obj in objectInRange)
        {
            if (ObjectInPredationAngle(obj))
            {
                Enemy enemy = obj.GetComponent<Enemy>();

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

        return angle < skillManager.skillData.PredationData.PREDATION_ANGLE;                                        //방향 사이의 각도보다 흡수 각도가 크다면 흡수 가능하다.
    }

    private void ApplyDamageToEnemy(Enemy enemy)
    {
        if (Time.time >= skillManager.skillData.PredationData.lastDamageTIme + skillManager.skillData.PredationData.damageCooldown)
        {
            skillManager.skillData.PredationData.lastDamageTIme = Time.time;

            float damaegeToDeal = skillManager.skillData.PredationData.
                predationSkillData.CalculateSkillDamage(CharacterStatManager.instance.currentData.currentAttackPower);
            IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();

            if (damageableEnemy != null)
                damageableEnemy.TakeDamage(damaegeToDeal);
        }
    }

    private void EnoughAbsorb(Enemy enemy, Collider obj)
    {
        if (enemy.characterStatManager.currentData.currentHP <= enemy.characterStatManager.currentData.currentMaxHP * 0.5f)
        {
            Vector3 directionToAbsorb = (playerController.transform.position - obj.transform.position).normalized;
            float distancToPlayer = Vector3.Distance(playerController.transform.position, obj.transform.position);
            float absorptionSpeed = (1 - (distancToPlayer / skillManager.skillData.PredationData.predationRaidus))
                * skillManager.skillData.PredationData.predationForce;

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
            Destroy(obj.gameObject);
            string absorbedText = skillManager.skillData.PredationData.absorbedObjectText.text = "포식 결과: " + obj.gameObject.name + "입니다.";
            StartCoroutine(ShowAndFadeOutText(absorbedText, 4f));
            SoundManager.Instance.PlayHumanPredationSound();
        }
    }
    private IEnumerator ShowAndFadeOutText(string text, float duration)
    {
        TMP_Text absorbedText = skillManager.skillData.PredationData.absorbedObjectText;
        absorbedText.text = text;

        // 텍스트의 투명도를 천천히 감소시키기
        float elapsedTime = 0;
        Color originalColor = absorbedText.color;
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            absorbedText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        absorbedText.text = ""; // 텍스트 내용을 비움
    }
    #endregion

    #region 스킬 활성화
    public void ActivateSkill()
    {
        skillManager.skillData.PredationData.SetActivePredation(true);
        Instantiate(skillManager.skillData.PredationData.predationPosPrefab, skillManager.skillData.PredationData.predationPosition.transform.position + new Vector3(0f, -1f, 0f), playerController.transform.rotation);
        Instantiate(skillManager.skillData.PredationData.predationPrefab, skillManager.skillData.PredationData.predationPosition.transform.position, playerController.transform.rotation);
        StartCoroutine(ActivatePredation());
    }

    private IEnumerator ActivatePredation()
    {
        yield return CoroutineHelper.WaitForSeconds(skillManager.skillData.PredationData.PREDATION_DURATION);
        skillManager.skillData.PredationData.SetActivePredation(false);
    }
    #endregion
}