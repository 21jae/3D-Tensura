using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [HideInInspector] public CharacterStatManager playerStatManager;
    [HideInInspector] public CharacterController characterController;
    public SkillManager skillManager;

    private Animator animator;
    private Joystick controller;
    private MoveObject moveObject;

    [field: SerializeField] public PlayerSO Data { get; private set; }
    private List<Renderer> characterRenderers = new List<Renderer>();

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
        if (!UIInventory.INVENTORY_ACTIVATED)
        {
            ApplyGravityScale();

            MovementUpdate();

            AttackUpdate();

            CheckHitWall();
        }
    }
    #endregion

    private void MovementUpdate()
    {
        float moveSpeed = controller.Direction.magnitude;
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    private void ApplyGravityScale()
    {
        if (isGrounded() && Data.AirData.GravityData.VerticalVelocity < 0f)
        {
            Data.AirData.GravityData.VerticalVelocity = 0f;
        }
        else
        {
            Data.AirData.GravityData.VerticalVelocity += Data.AirData.GravityData.gravity * Time.deltaTime;
        }

        characterController.Move(new Vector3(0f, Data.AirData.GravityData.VerticalVelocity * Time.deltaTime, 0f));
    }

    #region 콤보 어택
    public bool IsAnimatorStateNameAndNormalized(string attackState, float threshold = 0.7f)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(attackState) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > threshold;
    }

    private void AttackUpdate()
    {
        if (Data.GroundData.AttackData.isButtonPressed && Time.time - Data.GroundData.AttackData.lastButtonPressTime <= 0.8f)
        {
            Data.GroundData.AttackData.lastButtonPressTime = Time.time;

            if (Data.GroundData.AttackData.comboStack == 0)
            {
                //사운드 재생
                animator.SetBool("Attack01", true);
            }

            else if (Data.GroundData.AttackData.comboStack >= 1 && IsAnimatorStateNameAndNormalized("Attack01"))
            {
                //사운드 재생

                animator.SetBool("Attack01", false);
                animator.SetBool("Attack02", true);
            }
            else if (Data.GroundData.AttackData.comboStack >= 2 && IsAnimatorStateNameAndNormalized("Attack02"))
            {
                //사운드 재생

                animator.SetBool("Attack02", false);
                animator.SetBool("Attack03", true);
            }
            else if (Data.GroundData.AttackData.comboStack >= 3 && IsAnimatorStateNameAndNormalized("Attack03"))
            {
                //사운드 재생

                animator.SetBool("Attack03", false);
                animator.SetBool("Attack04", true);
            }

            else if (IsAnimatorStateNameAndNormalized("Attack04"))
            {
                ResetAttackAnimation();
            }

            Data.GroundData.AttackData.isButtonPressed = false;
        }
        else if (Time.time - Data.GroundData.AttackData.lastButtonPressTime > 1f)
        {
            ResetAttackAnimation();
        }
    }

    private void ResetAttackAnimation()
    {
        animator.SetBool("Attack01", false);
        animator.SetBool("Attack02", false);
        animator.SetBool("Attack03", false);
        animator.SetBool("Attack04", false);
        Data.GroundData.AttackData.comboStack = 0;

    }

    public void StartButtonPress()
    {
        Data.GroundData.AttackData.isButtonPressed = true;
        Data.GroundData.AttackData.lastButtonPressTime = Time.time;

        if (Data.GroundData.AttackData.comboStack < 3)
            Data.GroundData.AttackData.comboStack++;

        else
            Data.GroundData.AttackData.comboStack = 0;
    }

    public void EndButtonPress()
    {
        Data.GroundData.AttackData.isButtonPressed = false;
    }

    #endregion

    #region 벽체크
    private void CheckHitWall()
    {
        RaycastHit hitInfo;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(transform.position, rayDirection, out hitInfo, Data.GroundData.WallData.rayDistance , Data.GroundData.WallData.wallLayerMask))
        {
            characterController.Move(Vector3.zero);
            Debug.Log("충돌중");
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * Data.GroundData.WallData.rayDistance, Color.red);
    }
    #endregion

    public void TakeDamage(float amount, bool isPredation = false)
    {
        if (Data.GroundData.HitData.isInvincible)
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

        else if (damagePercentage > 0.01f)
        {
            animator.SetBool("Damage", true);
            StartCoroutine(BecomeInvincible());
            StartCoroutine(Knockback());
        }

        Debug.Log(playerStatManager.currentHP);

        if (playerStatManager.currentHP <= 0f)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 2.5f);

            //UI창 출력 및 죽음사운드
        }
    }

    private IEnumerator Knockback()
    {
        float endTime = Time.time + Data.GroundData.HitData.knockbackDuration;
        Vector3 knockbackDirection = -transform.forward;

        while (Time.time < endTime)
        {
            characterController.Move(knockbackDirection * Data.GroundData.HitData.knockbackStrength * Time.deltaTime);
            yield return null;
        }

        animator.SetBool("Damage", false);
    }

    private IEnumerator PlayBigDamageAnimation()
    {
        Data.GroundData.HitData.SetIsRecoveringForBigDamage(true);

        animator.SetBool("BigDamage", true);
        yield return new WaitForSeconds(Data.GroundData.HitData.damageInterval);

        animator.SetBool("BigDamage", false);
        animator.SetBool("StandUp", true);
        yield return new WaitForSeconds(Data.GroundData.HitData.damageInterval);

        animator.SetBool("StandUp", false);
        yield return new WaitForSeconds(Data.GroundData.HitData.damageInterval);

        Data.GroundData.HitData.SetIsRecoveringForBigDamage(false);
    }

    private IEnumerator BecomeInvincible()
    {
        Data.GroundData.HitData.SetIsInvincible(true);

        float invincibilityEndTime = Time.time + Data.GroundData.HitData.invincibilityDuration;

        while (Time.time < invincibilityEndTime)
        {
            foreach (var renderer in characterRenderers)
            {
                renderer.enabled = !renderer.enabled;
            }
            yield return new WaitForSeconds(Data.GroundData.HitData.blinkDuration);
        }

        foreach (var renderer in characterRenderers)
        {
            renderer.enabled = true;
        }

        Data.GroundData.HitData.SetIsInvincible(false);
    }

    public void StopPlayer()
    {
        characterController.Move(Vector3.zero);
        animator.SetFloat("MoveSpeed", 0f);
    }

    public bool isMoving()
    {
        return characterController.velocity.magnitude > 0.1f;
    }

    public bool isGrounded() => characterController.isGrounded;
}