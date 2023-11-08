using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour, ISkill
{
    private Vector3 originalPosition;

    private List<Transform> magicStartPosition = new List<Transform>();
    private List<Transform> magicTargetPosition = new List<Transform>();
    private List<GameObject> createdMegidoRays = new List<GameObject>();

    private PlayerController playerController;
    private SkillManager skillManager;
    private Enemy enemy;
    private Animator animator;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerController = FindObjectOfType<PlayerController>();
        animator = playerController.GetComponentInChildren<Animator>();
        enemy = FindObjectOfType<Enemy>();
        skillManager = GetComponent<SkillManager>();

        if (playerController == null)
        {
            Debug.Log("컨트롤러 없음");
        }
    }

    private void ActivateMeshes()
    {
        if (skillManager.skillData.MegidoData.wingMesh != null && 
            skillManager.skillData.MegidoData.maskMesh != null &&
            skillManager.skillData.MegidoData.swordMesh != null)
        {
            skillManager.skillData.MegidoData.wingMesh.gameObject.SetActive(true);
            skillManager.skillData.MegidoData.maskMesh.gameObject.SetActive(true);
            skillManager.skillData.MegidoData.swordMesh.gameObject.SetActive(false);
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

        Vector3 megidoCirclePos = playerController.transform.position + playerController.transform.forward * 30f;
        GameObject createdMegidoCircle = Instantiate(skillManager.skillData.MegidoData.megidoCircle, megidoCirclePos, Quaternion.identity);

        //애니메이션 스테이트 체크 타이밍 겹쳐서 0.1초간 텀을 줬다.
        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_1") || state.normalizedTime >= 1f; });

        //도약중
        animator.Play("Player_Skill05_2");
        float targetZ = playerController.transform.position.z + 3f;
        float targetY = playerController.transform.position.y + skillManager.skillData.MegidoData.jumpHeight;

        while (playerController.transform.position.y < targetY)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, new Vector3(playerController.transform.position.x,
                targetY, targetZ), skillManager.skillData.MegidoData.jumpSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(CreateMagicPosWithInterval(15, 2f));

        yield return new WaitForSeconds(skillManager.skillData.MegidoData.skillDelay);

        for (int i = 0; i < 88; i++)
        {
            Vector3 randomTargetOffset = new Vector3(UnityEngine.Random.Range(-18f, 20f), UnityEngine.Random.Range(0f, 12f), UnityEngine.Random.Range(-18f, 20f));
            Transform magicTarget = Instantiate(skillManager.skillData.MegidoData.megidoTargetPrefab, createdMegidoCircle.transform.position + randomTargetOffset, Quaternion.identity);

            magicTargetPosition.Add(magicTarget);
            yield return new WaitForSeconds(0.05f);
        }

        animator.Play("Player_Skill05_4");

        //발사하기 전 잠깐 대기
        yield return new WaitForSeconds(1f);

        Vector3 targetPosition = new Vector3(playerController.transform.position.x, playerController.transform.position.y - 6f, playerController.transform.position.z + 6f);
        float moveSpeed = 4f;

        while (Vector3.Distance(playerController.transform.position, targetPosition) > 0.1f)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }


        //StartPosition을 안전히 저장할 tempList
        var tempList = new List<Transform>(magicStartPosition);

        foreach (Transform startPos in tempList)
        {
            int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
            Transform endPos = magicTargetPosition[randomTargetIndex];

            StartCoroutine(ShootRayFromTo(startPos, endPos, 180f));
        }

        //메기도 지속시간
        yield return new WaitForSeconds(5f);

        foreach (GameObject ray in createdMegidoRays)
        {
            Destroy(ray);
        }
        createdMegidoRays.Clear();

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
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, originalPosition, skillManager.skillData.MegidoData.jumpSpeed * Time.deltaTime);

            yield return null;
        }

        GameObject explosion = Instantiate(skillManager.skillData.MegidoData.megidoExplosion, megidoCirclePos, Quaternion.identity);

        Collider[] hitEnemies = Physics.OverlapSphere(explosion.transform.position, 50f, playerController.Data.GroundData.LayerData.GroundLayer);
        float damageToDeal = skillManager.skillData.MegidoData.specialSkillData.CalculateSkillDamage(playerController.playerStatManager.currentData.currentAttackPower);

        foreach (Collider enemy in hitEnemies)
        {
            IDamageable damageableEnemy = enemy.GetComponent<IDamageable>();
            if (damageableEnemy != null)
            {
                damageableEnemy.TakeDamage(damageToDeal);
            }
        }

        skillManager.skillData.MegidoData.wingMesh.gameObject.SetActive(false);
        skillManager.skillData.MegidoData.maskMesh.gameObject.SetActive(false);
        skillManager.skillData.MegidoData.swordMesh.gameObject.SetActive(true);

        animator.SetFloat("MoveSpeed", 0f);
    }


    private IEnumerator ShootRayFromTo(Transform from, Transform to, float raySpeed)
    {
        Vector3 startPosition = from.position;
        Vector3 endPosition = to.position;

        Vector3 middlePosition = (startPosition + endPosition) / 2;
        middlePosition.y += UnityEngine.Random.Range(1f, 3f);

        //광선을 1회 발사했다면 megidoPos 해당 위치가 List에 저장된 magicPosition를 제거시켜, 다시 pos로 돌아가지않게합니다.
        if (magicStartPosition.Contains(from))
        {
            magicStartPosition.Remove(from);
        }

        GameObject megidoRayInstance = Instantiate(skillManager.skillData.MegidoData.megidoRayPrefab, startPosition, Quaternion.identity);
        createdMegidoRays.Add(megidoRayInstance);
        LineRenderer lineRenderer = megidoRayInstance.GetComponent<LineRenderer>();

        int segments = 20;
        lineRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            Vector3 point = BezierCurve(t, startPosition, middlePosition, endPosition);
            lineRenderer.SetPosition(i, point);
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        lineRenderer.startWidth = 0.08f;
        lineRenderer.endWidth = 0.08f;

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
                Instantiate(skillManager.skillData.MegidoData.megidoHitPrefab, megidoRayInstance.transform.position, Quaternion.identity);
                int randomTargetIndex = UnityEngine.Random.Range(0, magicTargetPosition.Count);
                to = magicTargetPosition[randomTargetIndex];
                endPosition = to.position;
                lineRenderer.SetPosition(0, megidoRayInstance.transform.position);
                lineRenderer.SetPosition(1, megidoRayInstance.transform.position);
            }
            yield return null;
        }

    }

    private IEnumerator CreateMagicPosWithInterval(int count, float interval)
    {
        Vector3 centerPos = playerController.transform.position + new Vector3(0f, 3.5f, -1f);

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
        int createdCount = 0;   //생성된 magicPos의 수 추적

        while (createdCount < 15)
        {
            int randomIndex = UnityEngine.Random.Range(0, possiblePosition.Count);  //랜덤한 위치의 인덱스를 생성
            Vector3 randomPos = possiblePosition[randomIndex];
            possiblePosition.RemoveAt(randomIndex); //이미 선택한 위치는 다시 선택하지않음

            Transform magicPos = Instantiate(skillManager.skillData.MegidoData.megidoPosPrefab, randomPos, Quaternion.identity);
            magicStartPosition.Add(magicPos);

            createdCount++;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}