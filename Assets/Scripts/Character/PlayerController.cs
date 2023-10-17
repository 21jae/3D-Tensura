using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterStats playerStats;
    private CharacterController characterController;
    private Animator animator;
    private Joystick controller;
    private MoveObject moveObject;

    //Skill
    private PredationSkill predationskill;
    private DashSwordSkill dashSwordskill;
    private BuffSkill buffSkill;
    private BlessingSkill blessingskill;

    //Attack
    [SerializeField] private float cooldownTime = 2f;
    private float nextAttackTime = 0f;
    private static int comboStack = 0;
    private float lastClickTimed = 0f;
    private float maxComboDelay = 1f;

    public float currentAttackPower;


    #region Animator
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int Attack01 = Animator.StringToHash("Attack01");
    private static readonly int Attack02 = Animator.StringToHash("Attack02");
    private static readonly int Attack03 = Animator.StringToHash("Attack03");
    private static readonly int Attack04 = Animator.StringToHash("Attack04");
    #endregion

    //Ray

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        controller = FindObjectOfType<Joystick>();
        moveObject = GetComponent<MoveObject>();

        predationskill = GetComponent<PredationSkill>();
        buffSkill = GetComponent<BuffSkill>();
        dashSwordskill = GetComponent<DashSwordSkill>();
        blessingskill = GetComponent<BlessingSkill>();

        currentAttackPower = playerStats.attackPower;
    }

    private void Start()
    {
        Debug.Log(playerStats.attackPower.ToString());
    }

    private void Update()   
    {
        AttackUpdate();

        float moveSpeed = controller.Direction.magnitude;
        animator.SetFloat(MoveSpeed, moveSpeed);

        CheckHitWall();
    }


    #region Attack
    private void AttackUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
        {
            animator.SetBool(Attack01, false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        {
            animator.SetBool(Attack02, false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            animator.SetBool(Attack03, false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack04"))
        {
            animator.SetBool(Attack04, false);
            comboStack = 0;
        }

        if (Time.time - lastClickTimed > maxComboDelay)
        {
            comboStack = 0;
        }
    }

    public void OnClick()
    {
        if (Time.time > nextAttackTime)
        {
            lastClickTimed = Time.time;
            comboStack++;

            if (comboStack == 1)
            {
                //스매쉬 사운드
                animator.SetBool(Attack01, true);
            }

            comboStack = Mathf.Clamp(comboStack, 0, 4);

            if (comboStack >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
            {
                animator.SetBool(Attack01, false);
                animator.SetBool(Attack02, true);
            }
            if (comboStack >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
            {
                animator.SetBool(Attack02, false);
                animator.SetBool(Attack03, true);
            }
            if (comboStack >= 4 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
            {
                animator.SetBool(Attack03, false);
                animator.SetBool(Attack04, true);
            }
        }
    }

    //ScriptableObject 값 변경 유지


    #endregion



    #region 스킬발동
    public void ActivateSkill(SOSkill skill)
    {
        animator.Play(skill.animationName);
    }
        
    public void ActivatePredationSkill() => predationskill.ActivatePredation();
    public void ActivatebuffSkill() => buffSkill.ActivateTime();
    public void ActivateDashSkill() => dashSwordskill.ActivateDash();
    public void ActivateBlessSkill() => blessingskill.ActivateBless();

    #endregion


    #region 벽체크
    private void CheckHitWall()
    {
        RaycastHit hitInfo;
        Vector3 rayDirection = transform.forward;
        float rayDistance = 1.5f;
        int wallLayerMask = 1 << 6;

        if (Physics.Raycast(transform.position, rayDirection, out hitInfo, rayDistance, wallLayerMask))
        {
            characterController.Move(Vector3.zero);
            Debug.Log("충돌중");
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 rayDirection = transform.forward;
        float rayDistance = 1.5f;
        Color rayColor = Color.red;

        Vector3 rayStartPos = transform.position + Vector3.up;
        Debug.DrawRay(rayStartPos, rayDirection * rayDistance, rayColor);
    }
    #endregion


}
