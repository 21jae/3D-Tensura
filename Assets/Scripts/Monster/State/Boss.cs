using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    private BossThunderSkill thunderSKill;
    private BossDashSkill bossdashSkill;
    private BossBoltextSkill boltextSkill;

    [HideInInspector] public bool skillActive;
    [HideInInspector] public bool skillActive2;
    [HideInInspector] public bool skillActive3;

    private float waitSkillDelay = 1.5f;

    protected override void Awake()
    {
        base.Awake();

        thunderSKill = GetComponent<BossThunderSkill>();
        bossdashSkill = GetComponent<BossDashSkill>();
        boltextSkill = GetComponent<BossBoltextSkill>();
    }

    protected override void ChangeState(State newState)
    {
        base.ChangeState(newState);
    }

    private IEnumerator ThunderSkillRoutine()
    {
        yield return new WaitForSeconds(waitSkillDelay);

        skillActive = true;

        int repeatCount = 10;

        for (int i = 0; i < repeatCount; i++)
        {
            yield return new WaitForSeconds(waitSkillDelay + 1f);
            thunderSKill.CastThunderSkill();
        }

        yield return new WaitForSeconds(waitSkillDelay);
        skillActive = false;

        animator.SetBool("Boss_Skill01", false);
        ChangeState(State.PATROL);

    }

    private IEnumerator bossSkillRoutine()
    {
        yield return new WaitForSeconds(waitSkillDelay);

        skillActive3 = true;

        animator.SetBool("Boss_Skill03", true);
        bossdashSkill.BossDash();

        skillActive3 = false;

        yield return new WaitForSeconds(waitSkillDelay);
        animator.SetBool("Boss_Skill03", false);
        yield return new WaitForSeconds(2.5f);
        ChangeState(State.PATROL);

    }

    private IEnumerator BoltextSkillRoutine()
    {
        skillActive2 = true;
        yield return new WaitForSeconds(waitSkillDelay);
        skillActive2 = false;

        boltextSkill.CastBoltextSkill();
        yield return new WaitForSeconds(waitSkillDelay);
        ChangeState(State.PATROL);

    }

    public void BossSkill01()
    {
        if (!skillActive)
        {
            StartCoroutine(ThunderSkillRoutine());
        }
    }

    public void BossSkill02()
    {
        if (!skillActive2)
        {
            StartCoroutine(BoltextSkillRoutine());
        }
    }

    public void BossSkill03()
    {
        if (!skillActive3)
        {
            StartCoroutine(bossSkillRoutine());
        }
    }
}
