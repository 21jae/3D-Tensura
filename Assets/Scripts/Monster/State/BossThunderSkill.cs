using System.Collections;
using UnityEngine;

public class BossThunderSkill : MonoBehaviour
{
    [Header("������ų ������")]
    [SerializeField] private SOSkill thunderSkillData;  //ScritableObject�� ���� ��ų������
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
            Debug.Log("enemy�� ã�� �� ����.");
            return;
        }

        playerTransform = FindObjectOfType<PlayerController>().transform;

        if (playerTransform == null)
        {
            Debug.Log("�÷��̾� ��ġ ã�� �� ����");
        }
    }

    public void CastThunderSkill()
    {
        StartCoroutine(ThunderStrikeRoutine());
    }

    private IEnumerator ThunderStrikeRoutine()
    {
        //�÷��̾� ��ġ�� ��� ����
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

        //������ ������ �÷��̾��� ��ġ�� �����ؼ� ����Ʈ��
        //����Ʈ���� ���� �ٴڿ� Color.Red ����� ����
        //1���� �� ��ġ�� thunder ��ƼŬ �ý��� ������ ����
        //���� ������ �÷��̾�� �����ߴٸ� Debug.Log("hit thunder"); ���
    }
}