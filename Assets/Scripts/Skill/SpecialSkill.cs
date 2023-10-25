using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour, ISkill
{
    [Header("��ų ������")]
    [SerializeField] private SOSkill specialSkillData;
    [SerializeField] private GameObject megidoCircle;
    [SerializeField] private Transform megidoPosPrefab;
    [SerializeField] private Transform megidoTargetPrefab;
    [SerializeField] private GameObject megidoRayPrefab;
    private float skillDelay = 1f;


    [Header("����")]
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private GameObject wingMesh;
    [SerializeField] private GameObject maskMesh;
    [SerializeField] private GameObject swordMesh;
    private Vector3 originalPosition;

    private List<Transform> magicStartPosition = new List<Transform>();
    private List<Transform> magicTargetPosition = new List<Transform>();

    private PlayerController playerController;
    private Animator animator;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerController = FindObjectOfType<PlayerController>();
        animator = playerController.GetComponentInChildren<Animator>();

        if (playerController == null)
        {
            Debug.Log("��Ʈ�ѷ� ����");
        }
    }
    
    private void ActivateMeshes()
    {

        if (wingMesh != null && maskMesh != null && swordMesh != null)
        {
            wingMesh.gameObject.SetActive(true);
            maskMesh.gameObject.SetActive(true);
            swordMesh.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Meshes not Found!");
        }
    }

    public void ActivateSkill()
    {
        StartCoroutine(ExeCuteJump());

        //������ ���� �� �ؽ�Ʈ ȣ���ϱ�
        //Debug.Log("������ : �ޱ⵵ �غ� �Ϸ�Ǿ����ϴ�.");
    }

    private IEnumerator ExeCuteJump()
    {
        ActivateMeshes();

        originalPosition = playerController.transform.position;

        Vector3 megidoCirclePos = playerController.transform.position + playerController.transform.forward * 10f;
        GameObject createdMegidoCircle = Instantiate(megidoCircle, megidoCirclePos, Quaternion.identity);

        //�ִϸ��̼� ������Ʈ üũ Ÿ�̹� ���ļ� 0.1�ʰ� ���� ���.
        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_1") || state.normalizedTime >= 1f; });

        //������
        animator.Play("Player_Skill05_2");
        float targetZ = playerController.transform.position.z + 3f;
        float targetY = playerController.transform.position.y + jumpHeight;

        while (playerController.transform.position.y < targetY)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, new Vector3(playerController.transform.position.x,
                targetY, targetZ), jumpSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(CreateMagicPosWithInterval(15, 2f));

        yield return new WaitForSeconds(skillDelay);

        for (int i = 0; i < 40; i++)
        {
            Vector3 randomTargetOffset = new Vector3(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(0f, 16f), UnityEngine.Random.Range(-15f, 15f));
            Transform magicTarget = Instantiate(megidoTargetPrefab, createdMegidoCircle.transform.position + randomTargetOffset, Quaternion.identity);

            magicTargetPosition.Add(magicTarget);
            yield return new WaitForSeconds(0.05f);
        }

        animator.Play("Player_Skill05_4");

        //�߻��ϱ� �� ��� ���
        yield return new WaitForSeconds(1f);

        
        //StartPosition�� ������ ������ tempList
        var tempList = new List<Transform>(magicStartPosition);

        foreach (Transform startPos in tempList)
        {
            int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
            Transform endPos = magicTargetPosition[randomTargetIndex];

            StartCoroutine(ShootRayFromTo(startPos, endPos, 150f));
        }

        //�ޱ⵵ ���ӽð�
        yield return new WaitForSeconds(30f);

        //���ӽð� ��������Ƿ� ������ �� List�� ����� �͵� ����
        foreach (Transform startPos in tempList)
        {
            Destroy(startPos.gameObject);
        }

        magicStartPosition.Clear();

        foreach (Transform targetPos in magicTargetPosition)
        {
            Destroy(targetPos.gameObject);
        }

        magicTargetPosition.Clear();

        Destroy(createdMegidoCircle);

        //���� ��ġ�� ����
        while (Vector3.Distance(playerController.transform.position, originalPosition) > 0.1f)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, originalPosition, jumpSpeed * Time.deltaTime);

            yield return null;
        }

        animator.SetFloat("MoveSpeed", 0f);
    }


    private IEnumerator ShootRayFromTo(Transform from, Transform to, float raySpeed)
    {
        Vector3 startPosition = from.position;
        Vector3 endPosition = to.position;

        //������ 1ȸ �߻��ߴٸ� megidoPos �ش� ��ġ�� List�� ����� magicPosition�� ���Ž���, �ٽ� pos�� ���ư����ʰ��մϴ�.
        if (magicStartPosition.Contains(from))
        {
            magicStartPosition.Remove(from);
        }

        GameObject megidoRayInstance = Instantiate(megidoRayPrefab, startPosition, Quaternion.identity);
        MegidoRay megidoScript = megidoRayInstance.GetComponent<MegidoRay>();

        while (true) 
        {
            megidoScript.MoveToWards(endPosition, raySpeed);

            Ray ray = new Ray(megidoRayInstance.transform.position, endPosition - megidoRayInstance.transform.position);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, raySpeed * Time.deltaTime))
            {
                if (hitInfo.transform)
                {
                    //���ο� ���� Ÿ�� ����
                    int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
                    to = magicTargetPosition[randomTargetIndex];
                    endPosition = to.position;
                }
            }
            
            if (Vector3.Distance(megidoRayInstance.transform.position, endPosition) <= 0.01f)
            {
                //���ο� ���� Ÿ�� ����
                int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
                to = magicTargetPosition[randomTargetIndex];
                endPosition = to.position;
            }

            yield return null;
        }
    }

    private IEnumerator CreateMagicPosWithInterval(int count, float interval)
    {
        Vector3 centerPos = playerController.transform.position + new Vector3(0f , 3.5f, -1f);

        List<Vector3> possiblePosition = new List<Vector3>();

        for (int i = 0; i < 6; i++)
        {
            possiblePosition.Add(centerPos + new Vector3((i - 2.5f) * interval, 0, -interval));
        }
        for (int i = 0; i < 5; i++)
        {
            possiblePosition.Add(centerPos + new Vector3((i - 2f) * interval, 0, 0));
        }
        for (int i = 0; i < 4; i++)
        {
            possiblePosition.Add(centerPos + new Vector3((i - 1.5f) * interval, 0, interval));
        }

        int createdCount = 0;   //������ magicPos�� �� ����

        while (createdCount < 15)
        {
            int randomIndex = UnityEngine.Random.Range(0, possiblePosition.Count);  //������ ��ġ�� �ε����� ����
            Vector3 randomPos = possiblePosition[randomIndex];
            possiblePosition.RemoveAt(randomIndex); //�̹� ������ ��ġ�� �ٽ� ������������

            Transform magicPos = Instantiate(megidoPosPrefab, randomPos, Quaternion.identity);
            magicStartPosition.Add(magicPos);

            createdCount++;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}