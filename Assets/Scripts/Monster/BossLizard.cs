using System.Collections;
using UnityEngine;

public class BossLizard : EnemyController
{
    private float specialAttackChance = 0.8f;

    #region ���� ���� ǥ��
    private LineRenderer lineRenderer;
    public float attackRadius = 40f;
    public float startAngle = -45f;
    public float endAngle = 45f;
    public int resolution = 50;
    #endregion

    #region �ִϸ��̼� �ؽ�
    private static readonly int SpecialAttack = Animator.StringToHash("SpecialAttack");
    private static readonly int Skill = Animator.StringToHash("Skill");
    private static readonly int Rush = Animator.StringToHash("Rush");
    #endregion 

    protected override void Start()
    {
        base.Start();
        
        StartCoroutine(SpecialSkill());
    }


    

    protected override IEnumerator AttackPlayer()
    {
        if (Random.value < specialAttackChance)
        {
            yield return new WaitForSeconds(3f);
            animator.SetTrigger(SpecialAttack);
            yield return new WaitForSeconds(attackDelay);
        }
        else 
        {
            yield return base.AttackPlayer();
        } 

        isReadyToAttack = true;
    }

    private IEnumerator SpecialSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(25f);
            UseSpecialSkill();
        }
    }

    private void UseSpecialSkill()
    {
        Debug.Log("��ų ���!");
        animator.SetTrigger(Skill);
    }
}


