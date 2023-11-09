using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private Animator animator;

    [Header("�÷��̾� ��ų")]
    private SlimePoisonSkill poisonSkill;

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



//private UIChange uiChange;

//uiChange = FindObjectOfType<UIChange>();

//[field: Header("��ų �ع�")]
//[field: SerializeField] public PlayerSkillActiveData skillActiveData { get; private set; }
//[field: SerializeField] public SkillButton[] skillButtons { get; private set; }

//private void Update()
//{
//    CheckAndActivateSkills();
//}

//private void CheckAndActivateSkills()
//{
//    if (skillActiveData.buffSkill == true)
//    {
//        ActivateSpeicalSkill("BuffSkill");
//        skillActiveData.buffSkill = false;
//    }

//    if (skillActiveData.dashSkill == true)
//    {
//        ActivateSpeicalSkill("DashSkill");
//        skillActiveData.dashSkill = false;
//    }

//    if (skillActiveData.predationSkill == true)
//    {
//        ActivateSpeicalSkill("PredationSkill");
//        skillActiveData.predationSkill = false;
//    }

//    if (skillActiveData.blessingSkill == true)
//    {
//        ActivateSpeicalSkill("BlessingSkill");
//        skillActiveData.blessingSkill = false;
//    }

//    if (skillActiveData.megidoSkill == true)
//    {
//        ActivateSpeicalSkill("MegidoSkill");
//        skillActiveData.megidoSkill = false;
//    }
//    //
//    if (skillActiveData.characterChange == true)
//    {
//        ToggleCharacterChangeUI();
//        skillActiveData.characterChange = false;
//    }
//}


//private void ToggleCharacterChangeUI()
//{
//    uiChange.gameObject.SetActive(true);
//}

//public void ActivateSpeicalSkill(string skillName)
//{
//    foreach (var skillButton in skillButtons)
//    {
//        if (skillButton.skill.skillName == skillName)
//        {
//            skillButton.SetSkillActive(true);
//            break;
//        }
//    }
//}