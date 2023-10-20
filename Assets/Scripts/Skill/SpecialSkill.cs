using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class SpecialSkill : MonoBehaviour, ISkill
{
    [Header("��ų ������")]
    [SerializeField] private SOSkill specialSkillData;
    [SerializeField] private GameObject megidoCircle;
    [SerializeField] private Transform megidoPosPrefab;

    [Header("����")]
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

        // �߰��ؾ��� �� : megidoCircle Prefab�� �÷��̾ �ٶ󺸴� ���⿡�� 8 ��ŭ ������ ������ ������Ų��.
        Vector3 megidoCirclePos = playerController.transform.position + playerController.transform.forward * 8;
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

        yield return new WaitUntil(() => { AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05_2") || state.normalizedTime >= 1f; });

        animator.Play("Player_Skill05_3");


        for (int i = 0; i < 15; i++)
        {
            //�߰� �ؾ� �� ��: for������ ������ magicPos�� �� ��ġ�� �ƴ϶�, megidoCircle�� �������� ���� ������Ų��.
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-15f, 15f), 0f, UnityEngine.Random.Range(-15f, 15f));
            Transform magicPos = Instantiate(megidoPosPrefab, createdMegidoCircle.transform.position + randomOffset, Quaternion.identity);
            magicPosition.Add(magicPos);
        }
    }
}
