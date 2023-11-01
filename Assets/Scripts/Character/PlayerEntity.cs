using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    public CharacterStatManager characterStatManager;
    private Animator animator;
    private Rigidbody rigidbody;

    private bool isInvincible;



    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float amount, bool isPredation = false)
    {
        if (isInvincible)
        {
            return;
        }

        StopPlayer();

        float damageToTake = amount - characterStatManager.currentDefense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //공격력이 방어력보다 낮다면 데미지 0

        characterStatManager.currentHP -= damageToTake;

        //큰 데미지를 입을시 쓰러지는 enemy 애니메이션
        float damagePercentage = damageToTake / characterStatManager.currentHP;

        if (damagePercentage > 0.25f)
        {
            StartCoroutine(PlayBigDamageAnimation());
            StartCoroutine(BecomeInvincible());

        }

        else if (damagePercentage > 0.01f)
        {
            animator.SetBool(Damage, true);
            StartCoroutine(BecomeInvincible());
            StartCoroutine(Knockback());
        }

        Debug.Log(characterStatManager.currentHP);

        if (characterStatManager.currentHP <= 0f)
        {
            animator.SetTrigger(Death);
            Destroy(gameObject, 2.5f);

            //UI창 출력 및 죽음사운드
        }
    }
}
