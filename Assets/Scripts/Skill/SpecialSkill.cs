using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour, ISkill
{
    [Header("스킬 데이터")]
    [SerializeField] private SOSkill specialSkillData;
    [SerializeField] private GameObject megidoCircle;
    [SerializeField] private Transform megidoPosPrefab;
    [SerializeField] private Transform megidoTargetPrefab;
    private float skillDelay = 1f;


    [Header("도약")]
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
            Debug.Log("컨트롤러 없음");
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

        // megidoCircle Prefab을 플레이어가 바라보는 방향에서 10 만큼 떨어진 지점에 생성시킨다.
        Vector3 megidoCirclePos = playerController.transform.position + playerController.transform.forward * 10f;
        GameObject createdMegidoCircle = Instantiate(megidoCircle, megidoCirclePos, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_1") || state.normalizedTime >= 1f; });

        //도약중
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

        // 광선을 발사할 Pos가 생성 되었다면 다음 for문 실행
        // 매직 타겟 생성 for문
        for (int i = 0; i < 30; i++)
        {
            //매직서클 위치에서 x,z값으로 생성, 랜덤하게 지면으로부터 1 떨어진 부분부터 8까지 부분도 고려하여 생성
            Vector3 randomTargetOffset = new Vector3(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(0f, 10f), UnityEngine.Random.Range(-15f, 15f));

            // 이후 megidoTargetPrefab을 MegidoCircle안에 생성시킨다.
            Transform magicTarget = Instantiate(megidoTargetPrefab, createdMegidoCircle.transform.position + randomTargetOffset, Quaternion.identity);

            magicTargetPosition.Add(magicTarget);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_2") || state.normalizedTime >= 1f; });
        animator.Play("Player_Skill05_3");

        //대현자 사운드 및 텍스트 호출하기
        Debug.Log("대현자 : 메기도 준비가 완료되었습니다.");

        //발사하기 전 잠깐 대기
        yield return new WaitForSeconds(0.5f);

        //animaotr.Play("Player_Skill05_4") 발사하는 애니메이션 실행한다.
        animator.Play("Player_Skill05_4");
        
        //애니메이션이 실행되면 magicPos에사 magicTarget으로 향하는 광선을 1회 발사한다.
        //List로 생성한 magicStartPosition에 저장된 각 위치에서
        var tempList = new List<Transform>(magicStartPosition);

        foreach (Transform startPos in tempList)
        {
            int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
            Transform endPos = magicTargetPosition[randomTargetIndex];

            //광선 발사 : 발사된 광선은 빠른속도로 magicTarget끼리 랜덤으로 목표를 정하여 무한히 튕긴다.
            StartCoroutine(ShootRayFromTo(startPos, endPos, 15f));
        }

        //10초간 반복된다.
        yield return new WaitForSeconds(10f);

        //10초뒤엔 공격이 끝나므로 magicPos,magicTarget,magicCircle을 삭제시킨다.

        foreach (Transform startPos in tempList)
        {
            Destroy(startPos.gameObject);
        }

        magicStartPosition.Clear();

        foreach (Transform targetPos in magicTargetPosition)
        {
            Destroy(targetPos.gameObject);
        }

        magicTargetPosition.Clear();    //리스트 지우기

        Destroy(createdMegidoCircle);
    }

    private IEnumerator ShootRayFromTo(Transform from, Transform to, float speed)
    {
        Vector3 startPosition = from.position;
        Vector3 endPosition = to.position;

        //광선을 1회 발사했다면 megidoPos 해당 위치가 List에 저장된 magicPosition를 제거시켜, 다시 pos로 돌아가지않게합니다.
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
                if (hitInfo.transform == to)    //광선이 목표에 도달했다면
                {
                    Debug.Log("광선이 목표에 도달");
                    //새로운 랜덤 타겟 생성
                    int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
                    to = magicTargetPosition[randomTargetIndex];
                    endPosition = to.position;
                }
            }

            Debug.DrawLine(startPosition, endPosition, Color.red, 0.2f);

            Debug.Log("새로운 목표를 또 찾는가?");
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

        int createdCount = 0;   //생성된 magicPos의 수 추적

        while (createdCount < 15)
        {
            int randomIndex = UnityEngine.Random.Range(0, possiblePosition.Count);  //랜덤한 위치의 인덱스를 생성
            Vector3 randomPos = possiblePosition[randomIndex];
            possiblePosition.RemoveAt(randomIndex); //이미 선택한 위치는 다시 선택하지않음

            Transform magicPos = Instantiate(megidoPosPrefab, randomPos, Quaternion.identity);
            magicStartPosition.Add(magicPos);

            createdCount++;
            
            yield return new WaitForSeconds(0.05f);
        }

    }
}