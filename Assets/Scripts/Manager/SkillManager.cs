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

    [Header("�÷��̾� ��ų")]
    private BuffSkill buffSkill;
    private DashSwordSkill dashSwordSkill;
    private PredationSkill predationSkill;
    private BlessingSkill blessingSkill;
    private SpecialSkill specialSkill;

    [field: Header("��ų ������")]
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
        animator.Play(skill.animationName); //��ų �б�
    }

    public void ActivatebuffSkill() => buffSkill.ActivateSkill();    //���� ��ų �ߵ�
    public void ActivateDashSkill() => dashSwordSkill.ActivateSkill();   //�뽬 ��ų �ߵ�
    public void ActivatePredationSkill() => predationSkill.ActivateSkill();  //���� �ߵ�
    public void ActivateBlessSkill() => blessingSkill.ActivateSkill();  //��ȣ �ߵ�
    public void ActivateMegidoSkill() => specialSkill.ActivateSkill();  //���� �ߵ�
}

