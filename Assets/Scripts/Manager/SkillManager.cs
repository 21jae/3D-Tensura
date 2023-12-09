using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager _instance;
    public static SkillManager Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<SkillManager>();

            if (_instance == null)
            {
                GameObject skillManager = new GameObject("SkillManager");
                _instance = skillManager.AddComponent<SkillManager>();
            }
            return _instance;
        }
    }
    private Animator animator;

    [Header("플레이어 스킬")]
    private BuffSkill buffSkill;
    private DashSwordSkill dashSwordSkill;
    private PredationSkill predationSkill;
    private BlessingSkill blessingSkill;
    private SpecialSkill specialSkill;

    [field: Header("스킬 데이터")]
    [field: SerializeField] public PlayerSkillData skillData { get; private set; }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        animator = FindObjectOfType<PlayerController>().GetComponentInChildren<Animator>();
        buffSkill = GetComponent<BuffSkill>();
        dashSwordSkill = GetComponent<DashSwordSkill>();
        predationSkill = GetComponent<PredationSkill>();
        blessingSkill = GetComponent<BlessingSkill>();
        specialSkill = GetComponent<SpecialSkill>();
    }
    public void ReadSkill(SOSkill skill)
    {
        animator.Play(skill.animationName); //스킬 읽기
    }

    public void ActivatebuffSkill() => buffSkill.ActivateSkill();    //버프 스킬 발동
    public void ActivateDashSkill() => dashSwordSkill.ActivateSkill();   //대쉬 스킬 발동
    public void ActivatePredationSkill() => predationSkill.ActivateSkill();  //포식 발동
    public void ActivateBlessSkill() => blessingSkill.ActivateSkill();  //가호 발동
    public void ActivateMegidoSkill() => specialSkill.ActivateSkill();  //오의 발동
}

