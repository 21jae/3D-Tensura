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
    [SerializeField] private GameObject megidoRayPrefab;
    private float skillDelay = 1f;


    [Header("도약")]
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
            Debug.Log("컨트롤러 없음");
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

        //대현자 사운드 및 텍스트 호출하기
        //Debug.Log("대현자 : 메기도 준비가 완료되었습니다.");
    }

    private IEnumerator ExeCuteJump()
    {
        ActivateMeshes();

        originalPosition = playerController.transform.position;

        Vector3 megidoCirclePos = playerController.transform.position + playerController.transform.forward * 10f;
        GameObject createdMegidoCircle = Instantiate(megidoCircle, megidoCirclePos, Quaternion.identity);

        //애니메이션 스테이트 체크 타이밍 겹쳐서 0.1초간 텀을 줬다.
        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_1") || state.normalizedTime >= 1f; });

        //도약중
        animator.Play("Player_Skill05_2");
        float targetZ = playerController.transform.position.z + 3f;
        float targetY = playerController.transform.position.y + jumpHeight;

        while (playerController.transform.position.y < targetY)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, new Vector3(playerController.transform.position.x,
                targetY, targetZ), jumpSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(CreateMagicPosWithInterval(18, 2.5f));

        yield return new WaitForSeconds(skillDelay);

        for (int i = 0; i < 40; i++)
        {
            Vector3 randomTargetOffset = new Vector3(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(0f, 16f), UnityEngine.Random.Range(-15f, 15f));
            Transform magicTarget = Instantiate(megidoTargetPrefab, createdMegidoCircle.transform.position + randomTargetOffset, Quaternion.identity);

            magicTargetPosition.Add(magicTarget);
            yield return new WaitForSeconds(0.05f);
        }

        animator.Play("Player_Skill05_4");

        //발사하기 전 잠깐 대기
        yield return new WaitForSeconds(1f);

        
        //StartPosition을 안전히 저장할 tempList
        var tempList = new List<Transform>(magicStartPosition);

        foreach (Transform startPos in tempList)
        {
            int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
            Transform endPos = magicTargetPosition[randomTargetIndex];

            StartCoroutine(ShootRayFromTo(startPos, endPos, 100f));
        }

        //메기도 지속시간
        yield return new WaitForSeconds(30f);

        //지속시간 종료됐으므로 프리펩 및 List에 저장된 것들 제거
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

        //원래 위치로 복귀
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

        //광선을 1회 발사했다면 megidoPos 해당 위치가 List에 저장된 magicPosition를 제거시켜, 다시 pos로 돌아가지않게합니다.
        if (magicStartPosition.Contains(from))
        {
            magicStartPosition.Remove(from);
        }

        GameObject megidoRayInstance = Instantiate(megidoRayPrefab, startPosition, Quaternion.identity);
        LineRenderer lineRenderer = megidoRayInstance.GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;

        SetMaxAlpha(lineRenderer);

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
                    //새로운 랜덤 타겟 생성
                    lineRenderer.SetPosition(1, megidoRayInstance.transform.position);

                    int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
                    to = magicTargetPosition[randomTargetIndex];
                    endPosition = to.position;
                }
            }
            
            if (Vector3.Distance(megidoRayInstance.transform.position, endPosition) <= 0.01f)
            {
                //새로운 랜덤 타겟 생성
                int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
                to = magicTargetPosition[randomTargetIndex];
                endPosition = to.position;
                lineRenderer.SetPosition(0, megidoRayInstance.transform.position);
                lineRenderer.SetPosition(1, megidoRayInstance.transform.position);

                //StartCoroutine(DestroyAfterDelay(megidoRayInstance, 2f));
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
        for (int i = 0; i < 3; i++)
        {
            possiblePosition.Add(centerPos + new Vector3((i - 1f) * interval, 0, interval));
        }

        int createdCount = 0;   //생성된 magicPos의 수 추적

        while (createdCount < 18)
        {
            int randomIndex = UnityEngine.Random.Range(0, possiblePosition.Count);  //랜덤한 위치의 인덱스를 생성
            Vector3 randomPos = possiblePosition[randomIndex];
            possiblePosition.RemoveAt(randomIndex); //이미 선택한 위치는 다시 선택하지않음

            Transform magicPos = Instantiate(megidoPosPrefab, randomPos, Quaternion.identity);
            magicStartPosition.Add(magicPos);

            createdCount++;
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject megidoRay, float delay)
    {
        yield return new WaitForSeconds(delay);

        LineRenderer lineRenderer = megidoRay.GetComponent<LineRenderer>();
        float fadeDuration = 5.0f; // 잔상이 사라지는 데 걸리는 시간
        float startAlpha = lineRenderer.startColor.a;
        float endAlpha = lineRenderer.endColor.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            Color newStartColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, Mathf.Lerp(startAlpha, 0, normalizedTime));
            Color newEndColor = new Color(lineRenderer.endColor.r, lineRenderer.endColor.g, lineRenderer.endColor.b, Mathf.Lerp(endAlpha, 0, normalizedTime));

            lineRenderer.startColor = newStartColor;
            lineRenderer.endColor = newEndColor;

            yield return null;
        }

        lineRenderer.startColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, 0);
        lineRenderer.endColor = new Color(lineRenderer.endColor.r, lineRenderer.endColor.g, lineRenderer.endColor.b, 0);
    }

    private void SetMaxAlpha(LineRenderer lineRenderer)
    {
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;

        startColor.a = 1f;
        endColor.a = 1f;

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }
}