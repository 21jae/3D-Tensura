using System;
using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    private BossThunderSkill thunderSKill;
    private BossDashSkill bossdashSkill;

    protected override void Awake()
    {
        base.Awake();

        thunderSKill = GetComponent<BossThunderSkill>();
        bossdashSkill = GetComponent<BossDashSkill>();
    }

    private void Start()
    {
        StartCoroutine(ThunderSkillRoutine());
        StartCoroutine(bossSkillRoutine());
    }

    private IEnumerator ThunderSkillRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            thunderSKill.CastThunderSkill();
        }
    }
    private IEnumerator bossSkillRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(50f);
            bossdashSkill.BossDash();
        }
    }

}
