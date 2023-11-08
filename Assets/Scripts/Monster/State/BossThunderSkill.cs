using System.Collections;
using UnityEngine;

public class BossThunderSkill : MonoBehaviour
{
    [Header("보스 스킬 데이터")]
    private CharacterStatManager characterStatManager;
    [SerializeField] private SOSkill thunderSkillData;  //ScritableObject로 만든 스킬데이터
    [SerializeField] private GameObject thunder;
    [SerializeField] private GameObject randomThunder;

    private Transform playerTransform;
    private float targetThunderRadius = 10f;
    private float randomThunderRadius = 6f;
    private float randomThunderTransform = 12f;
    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        characterStatManager = GetComponent<CharacterStatManager>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    public void CastThunderSkill()
    {
        StartCoroutine(ThunderStrikeRoutine());

        for (int i = 0; i < 8; i++)
        {
            StartCoroutine(ThunderRandomStrike());
        }
    }

    private IEnumerator ThunderStrikeRoutine()
    {
        //플레이어 위치에 경고선 생성
        Vector3 warningPosition = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);

        GameObject warningLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        warningLine.transform.position = warningPosition;
        warningLine.transform.localScale = new Vector3(targetThunderRadius, 0.05f, targetThunderRadius);

        Destroy(warningLine.GetComponent<Collider>());

        Renderer renderer = warningLine.GetComponent<Renderer>();
        renderer.material.color = Color.red;
        Destroy(warningLine, 2);

        yield return new WaitForSeconds(2);

        GameObject instantiateThunder = Instantiate(thunder, warningPosition, Quaternion.identity);
        Destroy(instantiateThunder, 2f);

        Collider[] hitColliders = Physics.OverlapSphere(warningPosition, 6f, layerMask);

        float damageToDeal = thunderSkillData.CalculateSkillDamage(characterStatManager.currentData.currentAttackPower);

        foreach (Collider hit in hitColliders)
        {
            IDamageable damageableplayer = playerTransform.GetComponent<IDamageable>();

            if (hit.CompareTag("Player"))
            {
                Debug.Log("hit Thunder");
                damageableplayer.TakeDamage(damageToDeal);
                break;
            }
        }
    }

    private IEnumerator ThunderRandomStrike()
    {
        Vector3 bossPosition = transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-10, 10), 0f, Random.Range(-10, 10));
        Vector3 warningPosition = bossPosition + randomOffset;

        GameObject warningLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        warningLine.transform.position = warningPosition;
        warningLine.transform.localScale = new Vector3(randomThunderRadius, 0.03f, randomThunderRadius);

        Destroy(warningLine.GetComponent<Collider>());

        Renderer renderer = warningLine.GetComponent<Renderer>();
        renderer.material.color = Color.blue;
        Destroy(warningLine, 2);

        yield return new WaitForSeconds(2);

        GameObject random = Instantiate(randomThunder, warningPosition, Quaternion.identity);
        Destroy(random, 2f);

        Collider[] hitColliders = Physics.OverlapSphere(warningPosition, 3f, layerMask);

        float damageToDeal = thunderSkillData.CalculateSkillDamage(characterStatManager.currentData.currentAttackPower);

        foreach (Collider hit in hitColliders)
        {
            IDamageable damageableplayer = playerTransform.GetComponent<IDamageable>();

            if (hit.CompareTag("Player"))
            {
                Debug.Log("hit Thunder");
                damageableplayer.TakeDamage(damageToDeal);
                break;
            }
        }
    }
}