using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [HideInInspector] public CharacterStatManager playerStatManager;
    private CharacterController characterController;
    public SkillManager skillManager;
    public InventoryObject inventory;

    private Animator animator;
    private Joystick controller;
    private MoveObject moveObject;

    [Header("공격")]
    private static int COMBOSTACK = 0;
    private float lastButtonPressTime = 0f;
    private bool isButtonPressed = false;

    [Header("벽 체크")]
    [SerializeField] private float rayDistance = 1.5f;
    private const int WALL_LAYER_MASK = 1 << 6;

    [Header("피격")]
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float blinkDuration = 0.1f;
    [SerializeField] private float knockbackStrength = 5f;
    [SerializeField] private float knockbackDuration = 0.1f;
    [SerializeField] private float damageInterval = 1.5f;
    private List<Renderer> characterRenderers = new List<Renderer>();
    private bool isInvincible;
    private bool isRecoveringFormBigDamage;
    private bool isSkillDamage;


    #region 애니메이션
    [Header("애니메이션 Hash")]
    private readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int Attack01 = Animator.StringToHash("Attack01");
    private readonly int Attack02 = Animator.StringToHash("Attack02");
    private readonly int Attack03 = Animator.StringToHash("Attack03");
    private readonly int Attack04 = Animator.StringToHash("Attack04");
    private readonly int Death = Animator.StringToHash("Death");
    private readonly int Damage = Animator.StringToHash("Damage");
    private readonly int BigDamage = Animator.StringToHash("BigDamage");
    private readonly int StandUp = Animator.StringToHash("StandUp");


    #endregion

    #region 초기화 및 업데이트 로직
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

        InventoryTest();
    }

    #endregion

    private void MovementUpdate()
    {
        float moveSpeed = controller.Direction.magnitude;
        animator.SetFloat(MoveSpeed, moveSpeed);
    }

    #region 콤보 어택
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
                //사운드 재생
                animator.SetBool(Attack01, true);
            }

            else if (COMBOSTACK >= 1 && IsAnimatorStateNameAndNormalized("Attack01"))
            {
                //사운드 재생

                animator.SetBool(Attack01, false);
                animator.SetBool(Attack02, true);
            }
            else if (COMBOSTACK >= 2 && IsAnimatorStateNameAndNormalized("Attack02"))
            {
                //사운드 재생

                animator.SetBool(Attack02, false);
                animator.SetBool(Attack03, true);
            }
            else if (COMBOSTACK >= 3 && IsAnimatorStateNameAndNormalized("Attack03"))
            {
                //사운드 재생

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

    #region 벽체크
    private void CheckHitWall()
    {
        RaycastHit hitInfo;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(transform.position, rayDirection, out hitInfo, rayDistance, WALL_LAYER_MASK))
        {
            characterController.Move(Vector3.zero);
            Debug.Log("충돌중");
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

        StopPlayer();

        float damageToTake = amount - playerStatManager.currentDefense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //공격력이 방어력보다 낮다면 데미지 0

        playerStatManager.currentHP -= damageToTake;

        //큰 데미지를 입을시 쓰러지는 enemy 애니메이션
        float damagePercentage = damageToTake / playerStatManager.currentHP;
        
        if (damagePercentage > 0.25f)
        {
            StartCoroutine(PlayBigDamageAnimation());
            StartCoroutine(BecomeInvincible());

        }

        else if (damagePercentage > 0.05f)
        {
            animator.SetBool(Damage, true);
            StartCoroutine(BecomeInvincible());
            StartCoroutine(Knockback());
        }

        Debug.Log(playerStatManager.currentHP);

        if (playerStatManager.currentHP <= 0f)
        {
            animator.SetTrigger(Death);
            Destroy(gameObject, 2.5f);
            
            //UI창 출력 및 죽음사운드
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

        animator.SetBool(Damage, false);
    }

    private IEnumerator PlayBigDamageAnimation()
    {
        isRecoveringFormBigDamage = true;

        animator.SetBool(BigDamage, true);
        yield return new WaitForSeconds(damageInterval);

        animator.SetBool(BigDamage, false);
        animator.SetBool(StandUp, true);
        yield return new WaitForSeconds(damageInterval);

        animator.SetBool(StandUp, false);
        yield return new WaitForSeconds(damageInterval);

        isRecoveringFormBigDamage = false;
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
        isInvincible = false;
    }

    private void StopPlayer()
    {
        characterController.Move(Vector3.zero);
        animator.SetFloat(MoveSpeed, 0f);
    }

    #region 인벤토리
    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }

    private void InventoryTest()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            inventory.Load();
        }
    }
    #endregion
}