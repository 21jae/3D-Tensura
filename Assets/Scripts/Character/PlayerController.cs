using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [HideInInspector] public CharacterStatManager playerStatManager;
    private CharacterController characterController;
    public SkillManager skillManager;

    private Animator animator;
    private Joystick controller;
    private MoveObject moveObject;

    [Header("����")]
    private static int COMBOSTACK = 0;
    private float lastButtonPressTime = 0f;
    private bool isButtonPressed = false;

    [Header("�� üũ")]
    [SerializeField] private float rayDistance = 1.5f;
    private const int WALL_LAYER_MASK = 1 << 6;

    [Header("�ǰ�")]
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float blinkDuration = 0.1f;
    [SerializeField] private float knockbackStrength = 5f;
    [SerializeField] private float knockbackDuration = 0.1f;
    private List<Renderer> characterRenderers = new List<Renderer>();
    private bool isInvincible;


    #region
    [Header("�ִϸ��̼� Hash")]
    private readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int Attack01 = Animator.StringToHash("Attack01");
    private readonly int Attack02 = Animator.StringToHash("Attack02");
    private readonly int Attack03 = Animator.StringToHash("Attack03");
    private readonly int Attack04 = Animator.StringToHash("Attack04");
    private readonly int Death = Animator.StringToHash("Death");
    private readonly int Damage = Animator.StringToHash("Damage");

    #endregion

    #region �ʱ�ȭ �� ������Ʈ ����
    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        playerStatManager = GetComponent<CharacterStatManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        controller = FindObjectOfType<Joystick>();
        moveObject = GetComponent<MoveObject>();
        
        characterRenderers.AddRange(GetComponentsInChildren<Renderer>());
    }

    private void Update()
    {
        AttackUpdate();

        MovementUpdate();

        CheckHitWall();
    }

    #endregion

    private void MovementUpdate()
    {
        float moveSpeed = controller.Direction.magnitude;
        animator.SetFloat(MoveSpeed, moveSpeed);
    }

    #region �޺� ����
    public bool IsAnimatorStateNameAndNormalized(string attackState, float threshold = 0.7f)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(attackState) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > threshold;
    }

    private void AttackUpdate()
    {
        if (isButtonPressed)
        {
            lastButtonPressTime = Time.time;

            if (COMBOSTACK == 0)
            {
                //���� ���
                animator.SetBool(Attack01, true);
            }

            else if (COMBOSTACK >= 1 && IsAnimatorStateNameAndNormalized("Attack01"))
            {
                //���� ���

                animator.SetBool(Attack01, false);
                animator.SetBool(Attack02, true);
            }
            else if (COMBOSTACK >= 2 && IsAnimatorStateNameAndNormalized("Attack02"))
            {
                //���� ���

                animator.SetBool(Attack02, false);
                animator.SetBool(Attack03, true);
            }
            else if (COMBOSTACK >= 3 && IsAnimatorStateNameAndNormalized("Attack03"))
            {
                //���� ���

                animator.SetBool(Attack03, false);
                animator.SetBool(Attack04, true);
            }

            else if (IsAnimatorStateNameAndNormalized("Attack04"))
            {
                ResetAttackAnimation();
            }

            COMBOSTACK = (COMBOSTACK + 1) % 4;
        }
        else if (Time.time - lastButtonPressTime > 1f)
        {
            ResetAttackAnimation();
        }
    }

    private void ResetAttackAnimation()
    {
        animator.SetBool(Attack01, false);
        animator.SetBool(Attack02, false);
        animator.SetBool(Attack03, false);
        animator.SetBool(Attack04, false);
        COMBOSTACK = 0;

    }

    public void StartButtonPress()
    {
        isButtonPressed = true;
    }

    public void EndButtonPress()
    {
        isButtonPressed = false;
        COMBOSTACK = 0;
    }

    #endregion

    #region ��üũ
    private void CheckHitWall()
    {
        RaycastHit hitInfo;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(transform.position, rayDirection, out hitInfo, rayDistance, WALL_LAYER_MASK))
        {
            characterController.Move(Vector3.zero);
            Debug.Log("�浹��");
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * rayDistance, Color.red);
    }
    #endregion

    public void TakeDamage(float amount, bool isPredation = false)
    {
        if (isInvincible)
        {
            return;
        }
        
        float damageToTake = amount - playerStatManager.currentDefense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //���ݷ��� ���º��� ���ٸ� ������ 0

        playerStatManager.currentHP -= damageToTake;
        StartCoroutine(Knockback());
        animator.SetBool(Damage, true);
        StartCoroutine(BecomeInvincible());

        Debug.Log(playerStatManager.currentHP);

        if (playerStatManager.currentHP <= 0f)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 3.5f);
            
            //UIâ ��� �� ��������
        }
    }

    private IEnumerator Knockback()
    {
        float endTime = Time.time + knockbackDuration;
        Vector3 knockbackDirection = -transform.forward;

        while (Time.time < endTime)
        {
            characterController.Move(knockbackDirection * knockbackStrength * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float invincibilityEndTime = Time.time + invincibilityDuration;

        while (Time.time < invincibilityEndTime)
        {
            foreach (var renderer in characterRenderers)
            {
                renderer.enabled = !renderer.enabled;
            }
            yield return new WaitForSeconds(blinkDuration);
        }

        foreach (var renderer in characterRenderers)
        {
            renderer.enabled = true;
        }
        animator.SetBool(Damage, false);
        isInvincible = false;
    }
}