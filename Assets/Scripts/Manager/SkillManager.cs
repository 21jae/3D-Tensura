using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private Animator animator;
    //private PlayerController playerController;

    private BuffSkill buffSkill;
    private DashSwordSkill dashSwordSkill;
    private PredationSkill predationSkill;
    private BlessingSkill blessingSkill;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //playerController = GetComponent<PlayerController>();

        buffSkill = GetComponent<BuffSkill>();
        dashSwordSkill = GetComponent<DashSwordSkill>();
        predationSkill = GetComponent<PredationSkill>();
        blessingSkill = GetComponent<BlessingSkill>(); 
    }

    public void ReadSkill(SOSkill skill) => animator.Play(skill.animationName); //��ų �б�

    public void ActivatebuffSkill() => buffSkill.ActivateSkill();    //���� ��ų �ߵ�
    public void ActivateDashSkill() => dashSwordSkill.ActivateSkill();   //�뽬 ��ų �ߵ�
    public void ActivatePredationSkill() => predationSkill.ActivateSkill();  //���� �ߵ�
    public void ActivateBlessSkill() => blessingSkill.ActivateSkill();  //��ȣ �ߵ�

}


