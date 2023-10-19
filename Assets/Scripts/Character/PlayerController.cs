using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public PlayerStatManager playerStatManager;
    public SkillManager skillManager;

    private Animator animator;
    private Joystick controller;
    private MoveObject moveObject;

    [Header("공격")]
    [SerializeField] private float cooldownTime = 2f;
    private float nextAttackTime = 0f;
    private static int COMBOSTACK = 0;
    private float lastClickTimed = 0f;
    private float maxComboDelay = 3f;

    [Header("벽 체크")]
    [SerializeField] private float rayDistance = 1.5f;
    private const int WALL_LAYER_MASK = 1 << 6;

    [Header("애니메이션 Hash")]
    private readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private readonly int Attack01 = Animator.StringToHash("Attack01");
    private readonly int Attack02 = Animator.StringToHash("Attack02");
    private readonly int Attack03 = Animator.StringToHash("Attack03");
    private readonly int Attack04 = Animator.StringToHash("Attack04");

    #region 초기화 및 업데이트 로직
    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        controller = FindObjectOfType<Joystick>();
        moveObject = GetComponent<MoveObject>();
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

    #region 콤보 어택
    private bool IsAnimatorStateNameAndNormalized(string attackState, float threshold = 0.7f)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(attackState) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > threshold;
    }

    private void AttackUpdate()
    {
        if (IsAnimatorStateNameAndNormalized("Attack01"))
            animator.SetBool(Attack01, false);

        if (IsAnimatorStateNameAndNormalized("Attack02"))
            animator.SetBool(Attack02, false);

        if (IsAnimatorStateNameAndNormalized("Attack03"))
            animator.SetBool(Attack03, false);

        if (IsAnimatorStateNameAndNormalized("Attack04"))
        {
            animator.SetBool(Attack04, false);
            COMBOSTACK = 0;
        }

        if (Time.time - lastClickTimed > maxComboDelay)
            COMBOSTACK = 0;
    }

    public void OnClick()
    {
        if (Time.time > nextAttackTime)
        {
            lastClickTimed = Time.time;
            COMBOSTACK++;

            if (COMBOSTACK == 1)
            {
                animator.SetBool(Attack01, true);
            }

            COMBOSTACK = Mathf.Clamp(COMBOSTACK, 0, 4);

            if (COMBOSTACK >= 2 && IsAnimatorStateNameAndNormalized("Attack01"))
            {
                animator.SetBool(Attack01, false);
                animator.SetBool(Attack02, true);
            }
            if (COMBOSTACK >= 3 && IsAnimatorStateNameAndNormalized("Attack02"))
            {
                animator.SetBool(Attack02, false);
                animator.SetBool(Attack03, true);
            }
            if (COMBOSTACK >= 4 && IsAnimatorStateNameAndNormalized("Attack03"))
            {
                animator.SetBool(Attack03, false);
                animator.SetBool(Attack04, true);
            }
        }
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
}