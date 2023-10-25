using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoltextSkill : MonoBehaviour
{
    [Header("보스 스킬 데이터")]
    private CharacterStatManager characterStatManager;
    [SerializeField] private SOSkill boltexSkillData; 
    [SerializeField] private SOSkill bolteExploisionSkillData;
    [SerializeField] private GameObject boltexSpear;
    [SerializeField] private GameObject explositionboltextSpear;
    [SerializeField] private float spearSpeed = 15f;
    [SerializeField] private LayerMask layerMask;


    private Transform playerTransform;
    private Enemy enemy;
    private bool isCatchSpear;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        characterStatManager = GetComponent<CharacterStatManager>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
        enemy = GetComponent<Enemy>();
    }

    public void CastBoltextSkill()
    {
        isCatchSpear = false;
        StartCoroutine(BoltextReady());
    }

    private IEnumerator BoltextReady()
    {
        enemy.animator.SetBool("Boss_Skill02-1", true);
        yield return new WaitForSeconds(2f);

        enemy.animator.SetBool("Boss_Skill02-1", false);
        yield return new WaitForSeconds(1f);
        enemy.LookTowardsPoint(playerTransform.position);
        enemy.animator.SetBool("Boss_Skill02-2", true);
        enemy.swordPrefab.SetActive(false);
        yield return new WaitForSeconds(2f);

        GameObject thrownSpear = Instantiate(boltexSpear, transform.position, Quaternion.identity);
        StartCoroutine(ThorwSpearAndReturn(thrownSpear));
    }

    private IEnumerator ThorwSpearAndReturn(GameObject spear)
    {
        Vector3 targetPosition = playerTransform.position;
        Vector3 originalPosition = transform.position;

        float journeyLength = Vector3.Distance(originalPosition, targetPosition);
        float journeyDuration = 1.5f;
        float startTime = Time.time;

        while (true)
        {
            float distanceToplayer = Vector3.Distance(spear.transform.position, targetPosition);

            if (distanceToplayer < 0.1f)
            {
                break;
            }

            float distanceCovered = (Time.time - startTime) * journeyLength / journeyDuration;
            float fractionOfJourney = distanceCovered / journeyDuration;

            spear.transform.position = Vector3.Lerp(spear.transform.position, targetPosition, fractionOfJourney);

            yield return null;
        }

        Collider[] hitColliders = Physics.OverlapSphere(spear.transform.position, 1f, layerMask);

        float damageToDeal = boltexSkillData.CalculateSkillDamage(characterStatManager.currentAttackPower);

        foreach (Collider hit in hitColliders)
        {
            IDamageable damageableplayer = playerTransform.GetComponent<IDamageable>();

            if (hit.CompareTag("Player"))
            {
                Debug.Log("hit boltex");
                damageableplayer.TakeDamage(damageToDeal);
                break;
            }
        }

        yield return new WaitForSeconds(0.8f);

        Instantiate(explositionboltextSpear, spear.transform.position, Quaternion.identity);
        enemy.animator.SetBool("Boss_Skill02-2", false);

        Collider[] hitExplosiion = Physics.OverlapSphere(spear.transform.position, 4f, layerMask);

        damageToDeal = bolteExploisionSkillData.CalculateSkillDamage(characterStatManager.currentAttackPower);

        foreach (Collider hit in hitExplosiion)
        {
            IDamageable damageableplayer = playerTransform.GetComponent<IDamageable>();

            if (hit.CompareTag("Player"))
            {
                Debug.Log("hit boltexExplosion");
                damageableplayer.TakeDamage(damageToDeal);
                break;
            }
        }

        while (!isCatchSpear)
        {
            Vector3 directionToBoss = (transform.position - spear.transform.position).normalized;
            spear.transform.position += directionToBoss * spearSpeed * Time.deltaTime;

            float distanceToBoss = Vector3.Distance(spear.transform.position, transform.position + Vector3.up) ;

            if (distanceToBoss <= 1f)
            {
                enemy.animator.SetBool("Boss_Skill02-3", true);
                yield return new WaitForSeconds(2f);
                enemy.animator.SetBool("Boss_Skill02-3", false);
                isCatchSpear = true;
                enemy.swordPrefab.SetActive(true);
            }

            yield return null;
        }

        Destroy(spear.gameObject);
    }
}
