using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private Animator animator;

    [Header("�÷��̾� ��ų")]
    private BuffSkill buffSkill;
    private DashSwordSkill dashSwordSkill;
    private PredationSkill predationSkill;
    private BlessingSkill blessingSkill;
    private SpecialSkill specialSkill;

    [Header("���� ��ų")]
    private BossThunderSkill bossSkill01;
    private void Awake()
    {
        animator = FindObjectOfType<PlayerController>().GetComponentInChildren<Animator>();

        buffSkill = GetComponent<BuffSkill>();
        dashSwordSkill = GetComponent<DashSwordSkill>();
        predationSkill = GetComponent<PredationSkill>();
        blessingSkill = GetComponent<BlessingSkill>();
        specialSkill = GetComponent<SpecialSkill>();

        bossSkill01 = GetComponent<BossThunderSkill>();
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


    public void ActiveThunderSkill() => bossSkill01.CastThunderSkill(); //���� �ߵ�
}


