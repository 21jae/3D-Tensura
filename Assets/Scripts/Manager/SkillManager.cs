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

    public void ReadSkill(SOSkill skill) => animator.Play(skill.animationName); //스킬 읽기

    public void ActivatebuffSkill() => buffSkill.ActivateSkill();    //버프 스킬 발동
    public void ActivateDashSkill() => dashSwordSkill.ActivateSkill();   //대쉬 스킬 발동
    public void ActivatePredationSkill() => predationSkill.ActivateSkill();  //오의 발동
    public void ActivateBlessSkill() => blessingSkill.ActivateSkill();  //가호 발동

}


