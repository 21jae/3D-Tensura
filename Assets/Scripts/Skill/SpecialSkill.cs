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
    private float skillDelay = 1f;


    [Header("����")]
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private GameObject wingMesh;
    [SerializeField] private GameObject maskMesh;

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

        if (wingMesh != null && maskMesh != null)
        {
            wingMesh.gameObject.SetActive(true);
            maskMesh.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("WingMesh & MaskMesh not Found!");
        }
    }

    public void ActivateSkill()
    {
        StartCoroutine(ExeCuteJump());
    }

    private IEnumerator ExeCuteJump()
    {
        ActivateMeshes();

        // megidoCircle Prefab�� �÷��̾ �ٶ󺸴� ���⿡�� 10 ��ŭ ������ ������ ������Ų��.
        Vector3 megidoCirclePos = playerController.transform.position + playerController.transform.forward * 10f;
        GameObject createdMegidoCircle = Instantiate(megidoCircle, megidoCirclePos, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_1") || state.normalizedTime >= 1f; });

        //������
        animator.Play("Player_Skill05_2");
        float targetY = playerController.transform.position.y + jumpHeight;

        while (playerController.transform.position.y < targetY)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, new Vector3(playerController.transform.position.x,
                targetY, playerController.transform.position.z), jumpSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(CreateMagicPosWithInterval(15, 2f));

        yield return new WaitForSeconds(skillDelay);

        // ������ �߻��� Pos�� ���� �Ǿ��ٸ� ���� for�� ����
        // ���� Ÿ�� ���� for��
        for (int i = 0; i < 30; i++)
        {
            //������Ŭ ��ġ���� x,z������ ����, �����ϰ� �������κ��� 1 ������ �κк��� 8���� �κе� ����Ͽ� ����
            Vector3 randomTargetOffset = new Vector3(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(0f, 10f), UnityEngine.Random.Range(-15f, 15f));

            // ���� megidoTargetPrefab�� MegidoCircle�ȿ� ������Ų��.
            Transform magicTarget = Instantiate(megidoTargetPrefab, createdMegidoCircle.transform.position + randomTargetOffset, Quaternion.identity);

            magicTargetPosition.Add(magicTarget);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_2") || state.normalizedTime >= 1f; });
        animator.Play("Player_Skill05_3");

        //������ ���� �� �ؽ�Ʈ ȣ���ϱ�
        Debug.Log("������ : �ޱ⵵ �غ� �Ϸ�Ǿ����ϴ�.");

        //�߻��ϱ� �� ��� ���
        yield return new WaitForSeconds(0.5f);

        //animaotr.Play("Player_Skill05_4") �߻��ϴ� �ִϸ��̼� �����Ѵ�.
        animator.Play("Player_Skill05_4");
        
        //�ִϸ��̼��� ����Ǹ� magicPos���� magicTarget���� ���ϴ� ������ 1ȸ �߻��Ѵ�.
        //List�� ������ magicStartPosition�� ����� �� ��ġ����
        var tempList = new List<Transform>(magicStartPosition);

        foreach (Transform startPos in tempList)
        {
            int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
            Transform endPos = magicTargetPosition[randomTargetIndex];

            //���� �߻� : �߻�� ������ �����ӵ��� magicTarget���� �������� ��ǥ�� ���Ͽ� ������ ƨ���.
            StartCoroutine(ShootRayFromTo(startPos, endPos, 15f));
        }

        //10�ʰ� �ݺ��ȴ�.
        yield return new WaitForSeconds(10f);

        //10�ʵڿ� ������ �����Ƿ� magicPos,magicTarget,magicCircle�� ������Ų��.

        foreach (Transform startPos in tempList)
        {
            Destroy(startPos.gameObject);
        }

        magicStartPosition.Clear();

        foreach (Transform targetPos in magicTargetPosition)
        {
            Destroy(targetPos.gameObject);
        }

        magicTargetPosition.Clear();    //����Ʈ �����

        Destroy(createdMegidoCircle);
    }

    private IEnumerator ShootRayFromTo(Transform from, Transform to, float speed)
    {
        Vector3 startPosition = from.position;
        Vector3 endPosition = to.position;

        //������ 1ȸ �߻��ߴٸ� megidoPos �ش� ��ġ�� List�� ����� magicPosition�� ���Ž���, �ٽ� pos�� ���ư����ʰ��մϴ�.
        if (magicStartPosition.Contains(from))
        {
            magicStartPosition.Remove(from);
        }

        while (Vector3.Distance(startPosition, endPosition) > 0.1f) 
        {
            Ray ray = new Ray(startPosition, endPosition - startPosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, speed * Time.deltaTime))
            {
                if (hitInfo.transform == to)    //������ ��ǥ�� �����ߴٸ�
                {
                    Debug.Log("������ ��ǥ�� ����");
                    //���ο� ���� Ÿ�� ����
                    int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
                    to = magicTargetPosition[randomTargetIndex];
                    endPosition = to.position;
                }
            }

            Debug.DrawLine(startPosition, endPosition, Color.red, 0.2f);

            Debug.Log("���ο� ��ǥ�� �� ã�°�?");
            startPosition = ray.GetPoint(speed * Time.deltaTime);
            yield return null;
        }
    }


    private IEnumerator CreateMagicPosWithInterval(int count, float interval)
    {
        Vector3 centerPos = playerController.transform.position + new Vector3(0f , 3f, -1f);

        List<Vector3> possiblePosition = new List<Vector3>();

        for (int i = 0; i < 6; i++)
        {
            possiblePosition.Add(centerPos + new Vector3((i - 2.5f) * interval, UnityEngine.Random.Range(0f, 1.5f), -interval));
        }
        for (int i = 0; i < 5; i++)
        {
            possiblePosition.Add(centerPos + new Vector3((i - 2f) * interval, UnityEngine.Random.Range(0f, 1.5f), 0));
        }
        for (int i = 0; i < 4; i++)
        {
            possiblePosition.Add(centerPos + new Vector3((i - 1.5f) * interval, UnityEngine.Random.Range(0f, 1.5f), interval));
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
            
            yield return new WaitForSeconds(0.05f);
        }

    }
}