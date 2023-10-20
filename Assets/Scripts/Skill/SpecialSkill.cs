using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class SpecialSkill : MonoBehaviour, ISkill
{
    [Header("스킬 데이터")]
    [SerializeField] private SOSkill specialSkillData;
    [SerializeField] private GameObject megidoCircle;
    [SerializeField] private Transform megidoPosPrefab;

    [Header("도약")]
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private GameObject wingMesh;
    [SerializeField] private GameObject maskMesh;

    private List<Transform> magicPosition = new List<Transform>();
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

        // 추가해야할 것 : megidoCircle Prefab을 플레이어가 바라보는 방향에서 8 만큼 떨어진 지점에 생성시킨다.
        Vector3 megidoCirclePos = playerController.transform.position + playerController.transform.forward * 8;
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

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_2") || state.normalizedTime >= 1f; });

        animator.Play("Player_Skill05_3");


        for (int i = 0; i < 15; i++)
        {
            //추가 해야 할 것: for문으로 생성할 magicPos는 내 위치가 아니라, megidoCircle을 기준으로 랜덤 생성시킨다.
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-15f, 15f), 0f, UnityEngine.Random.Range(-15f, 15f));
            Transform magicPos = Instantiate(megidoPosPrefab, createdMegidoCircle.transform.position + randomOffset, Quaternion.identity);
            magicPosition.Add(magicPos);
        }
    }
}
