using System.Collections;
using UnityEngine;

public class BossThunderSkill : MonoBehaviour
{
    [Header("보스스킬 데이터")]
    private CharacterStatManager characterStatManager;
    [SerializeField] private SOSkill thunderSkillData;  //ScritableObject로 만든 스킬데이터
    [SerializeField] private GameObject thunder;

    private Transform playerTransform;
    private float thunderRadius = 8f;
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
    }

    private IEnumerator ThunderStrikeRoutine()
    {
        //플레이어 위치에 경고선 생성
        Vector3 warningPosition = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);

        GameObject warningLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        warningLine.transform.position = warningPosition;
        warningLine.transform.localScale = new Vector3(thunderRadius, 0.05f, thunderRadius);

        Destroy(warningLine.GetComponent<Collider>());

        Renderer renderer = warningLine.GetComponent<Renderer>();
        renderer.material.color = Color.red;
        Destroy(warningLine, 1.5f);

        yield return new WaitForSeconds(1.5f);

        GameObject instantiateThunder = Instantiate(thunder, warningPosition, Quaternion.identity);
        Destroy(instantiateThunder, 2f);

        Collider[] hitColliders = Physics.OverlapSphere(warningPosition, 3f, layerMask);

        float damageToDeal = thunderSkillData.CalculateSkillDamage(characterStatManager.currentAttackPower);

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