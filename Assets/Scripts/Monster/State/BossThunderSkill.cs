using System.Collections;
using UnityEngine;

public class BossThunderSkill : MonoBehaviour
{
    [Header("보스스킬 데이터")]
    [SerializeField] private SOSkill thunderSkillData;  //ScritableObject로 만든 스킬데이터
    [SerializeField] private GameObject thunder;

    private Enemy enemy;
    private Transform playerTransform;
    [SerializeField] private float thunderRadius = 8.0f;
    [SerializeField] private LayerMask layerMask;

    private void Awake()
    {
        Initialize();

    }

    private void Initialize()
    {
        enemy = FindObjectOfType<Enemy>();

        if (enemy == null)
        {
            Debug.Log("enemy를 찾을 수 없음.");
            return;
        }

        playerTransform = FindObjectOfType<PlayerController>().transform;

        if (playerTransform == null)
        {
            Debug.Log("플레이어 위치 찾을 수 없음");
        }
    }

    public void CastThunderSkill()
    {
        StartCoroutine(ThunderStrikeRoutine());
    }

    private IEnumerator ThunderStrikeRoutine()
    {
        //플레이어 위치에 경고선 생성
        Vector3 warningPosition = new Vector3(playerTransform.position.x, 0.1f, playerTransform.position.z);

        GameObject warningLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        warningLine.transform.position = warningPosition;
        warningLine.transform.localScale = new Vector3(thunderRadius, 0.1f, thunderRadius);

        Renderer renderer = warningLine.GetComponent<Renderer>();
        renderer.material.color = Color.red;
        Destroy(warningLine.transform, 1f);

        yield return new WaitForSeconds(1f);

        GameObject instantiateThunder = Instantiate(thunder, playerTransform.position, Quaternion.identity);
        Destroy(instantiateThunder, 2f);

        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, thunderRadius, layerMask);

        foreach (var hit in hitColliders)
        {
            {
                if (hit.CompareTag("Player"))
                {
                    Debug.Log("hit Thunder");
                    break;
                }
            }
        }

        //보스는 번개를 플레이어의 위치를 추적해서 떨어트림
        //떨어트리기 전에 바닥에 Color.Red 경고선을 생성
        //1초후 그 위치에 thunder 파티클 시스템 프리팹 생성
        //만약 번개가 플레이어에게 적중했다면 Debug.Log("hit thunder"); 출력
    }
}