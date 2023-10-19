using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour, ISkill
{
    [Header("스킬 데이터")]
    [SerializeField] private SOSkill specialSkillData;
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
    
    private void ActivateWingsAndMask()
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
        ActivateWingsAndMask();

        //도약 시작
        float targetY = playerController.transform.position.y + jumpHeight;

        //도약중
        while (playerController.transform.position.y < targetY)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, new Vector3(playerController.transform.position.x,
                targetY, playerController.transform.position.z), jumpSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitUntil(() => 
        { 
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); return !state.IsName("Player_Skill05") || state.normalizedTime >= 1f; 
        });

        animator.Play("Player_MegidoIdle");

        for (int i = 0; i < 10; i++)
        {
            Transform magicPos = Instantiate(megidoPosPrefab, playerController.transform.position + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, UnityEngine.Random.Range(-5f, 5f)), Quaternion.identity);
            magicPosition.Add(magicPos);
        }
    }
}
