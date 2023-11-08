using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [field: Header("Data")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Animation")]
    [field: SerializeField] public AnimationData AnimationData { get; private set; }

    public CharacterStatManager playerStatManager  { get; private set; }
    public CharacterController characterController { get; private set; }
    public SkillManager skillManager   { get; private set; }
    public Animator animator { get; private set; }
    public Joystick controller  { get; private set; }
    public MoveObject moveObject { get; private set; }


    private List<Renderer> characterRenderers = new List<Renderer>();

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

        AnimationData.Initialize();

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
        animator.SetFloat(AnimationData.MoveParmeterName, moveSpeed);
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

    #region �޺� ����
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
                //���� ���
                animator.SetBool(AnimationData.Attack01ParmeterName, true);
            }

            else if (Data.GroundData.AttackData.comboStack >= 1 && IsAnimatorStateNameAndNormalized("Attack01"))
            {
                //���� ���

                animator.SetBool(AnimationData.Attack01ParmeterName, false);
                animator.SetBool(AnimationData.Attack02ParmeterName, true);
            }
            else if (Data.GroundData.AttackData.comboStack >= 2 && IsAnimatorStateNameAndNormalized("Attack02"))
            {
                //���� ���

                animator.SetBool(AnimationData.Attack02ParmeterName, false);
                animator.SetBool(AnimationData.Attack03ParmeterName, true);
            }
            else if (Data.GroundData.AttackData.comboStack >= 3 && IsAnimatorStateNameAndNormalized("Attack03"))
            {
                //���� ���

                animator.SetBool(AnimationData.Attack03ParmeterName, false);
                animator.SetBool(AnimationData.Attack04ParmeterName, true);
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
        animator.SetBool(AnimationData.Attack01ParmeterName, false);
        animator.SetBool(AnimationData.Attack02ParmeterName, false);
        animator.SetBool(AnimationData.Attack03ParmeterName, false);
        animator.SetBool(AnimationData.Attack04ParmeterName, false);
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

    #region ��üũ
    private void CheckHitWall()
    {
        RaycastHit hitInfo;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(transform.position, rayDirection, out hitInfo, Data.GroundData.WallData.rayDistance, Data.GroundData.WallData.wallLayerMask))
        {
            characterController.Move(Vector3.zero);
            Debug.Log("�浹��");
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

        float damageToTake = amount - playerStatManager.currentData.currentDefense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //���ݷ��� ���º��� ���ٸ� ������ 0

        playerStatManager.currentData.currentHP -= damageToTake;

        //ū �������� ������ �������� enemy �ִϸ��̼�
        float damagePercentage = damageToTake / playerStatManager.currentData.currentHP;

        if (damagePercentage > 0.25f)
        {
            StartCoroutine(PlayBigDamageAnimation());
            StartCoroutine(BecomeInvincible());

        }

        else if (damagePercentage > 0.01f)
        {
            animator.SetBool(AnimationData.DamageParmeterName, true);
            StartCoroutine(BecomeInvincible());
            StartCoroutine(Knockback());
        }

        Debug.Log(playerStatManager.currentData.currentHP);

        if (playerStatManager.currentData.currentHP <= 0f)
        {
            animator.SetTrigger(AnimationData.DeathParmeterName);
            Destroy(gameObject, 2.5f);

            //UIâ ��� �� ��������
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

        animator.SetBool(AnimationData.DamageParmeterName, false);
    }

    private IEnumerator PlayBigDamageAnimation()
    {
        Data.GroundData.HitData.SetIsRecoveringForBigDamage(true);

        animator.SetBool(AnimationData.BigDamageParmeterName, true);
        yield return new WaitForSeconds(Data.GroundData.HitData.damageInterval);

        animator.SetBool(AnimationData.BigDamageParmeterName, false);
        animator.SetBool(AnimationData.StandUpParmeterName, true);
        yield return new WaitForSeconds(Data.GroundData.HitData.damageInterval);

        animator.SetBool(AnimationData.StandUpParmeterName, false);
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
        animator.SetFloat(AnimationData.MoveParmeterName, 0f);
    }

    public bool isMoving()
    {
        return characterController.velocity.magnitude > 0.1f;
    }

    public bool isGrounded() => characterController.isGrounded;
}