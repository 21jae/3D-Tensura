using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private Animator animator;

    [Header("플레이어 스킬")]
    private BuffSkill buffSkill;
    private DashSwordSkill dashSwordSkill;
    private PredationSkill predationSkill;
    private BlessingSkill blessingSkill;
    private SpecialSkill specialSkill;

    [Header("보스 스킬")]
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
        animator.Play(skill.animationName); //스킬 읽기
    }

    public void ActivatebuffSkill() => buffSkill.ActivateSkill();    //버프 스킬 발동
    public void ActivateDashSkill() => dashSwordSkill.ActivateSkill();   //대쉬 스킬 발동
    public void ActivatePredationSkill() => predationSkill.ActivateSkill();  //포식 발동
    public void ActivateBlessSkill() => blessingSkill.ActivateSkill();  //가호 발동
    public void ActivateMegidoSkill() => specialSkill.ActivateSkill();  //오의 발동


    public void ActiveThunderSkill() => bossSkill01.CastThunderSkill(); //번개 발동
}


