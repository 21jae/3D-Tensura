using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public enum State
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        GUARD,
        SKILL,
        DEATH
    }
    public State currentState;

    [HideInInspector] public CharacterStatManager characterStatManager;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject predationHit;
    [SerializeField] private GameObject attackParticlePrefab;
    [SerializeField] private GameObject deathPrefab;
    public GameObject swordPrefab;
    public GameObject shieldPrefab;

    [HideInInspector] public Animator animator;
    private Transform playerTransform;
    private MonsterWeapon monsterWeapon;
    private Monster monster;

    //Patrol
    [SerializeField] private float patrolRadius = 5f;   //���� �ݰ�
    [SerializeField] private float patrolDuration = 5f; //�� ��ġ�� �������� �ð�
    [SerializeField] private float detectionRadius = 8f;  //�÷��̾� ���� �Ÿ�
    private Vector3 nextPatrolPoint;
    [SerializeField] private float stopDistance = 2.5f;  //�÷��̾�� ���� �Ÿ� ����

    //Attack
    [SerializeField] private float attackInterval = 1.5f;    //���� ����
    private Coroutine attackRoutine;    //���� ������ �ڷ�ƾ�� �����Ѵ�.
    private bool isAttacking = false;

    //Guard
    [SerializeField] private float guardInterval = 1.5f;  //���� ���ӽð�
    private Coroutine guardRoutine;
    private bool hasPredationHitSpawned = false;
    private bool isGuarding = false;

    //Damage
    private float DamageInterval = 1.5f;
    private bool isRecoveringFormBigDamage = false;
    private bool isBeingDestroy = false;


    protected virtual void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        ChangeState(State.PATROL);

        animator = GetComponentInChildren<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        monsterWeapon = GetComponentInChildren<MonsterWeapon>();
        characterStatManager = GetComponent<CharacterStatManager>();
        monster = GetComponent<Monster>();

        ChooseNextPatrolPoint();
    }

    private void Update()
    {
        ExecuteState();
    }

    protected virtual void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            ExitState(currentState);
            currentState = newState;
            EnterState(newState);
        }
    }


    #region Switch State
    private void EnterState(State state)
    {
        Debug.Log(" ���� ���� : " + currentState);

        switch (currentState)
        {
            case State.IDLE:
                EnterIdle();
                break;
            case State.PATROL:
                EnterPatrol();
                break;
            case State.CHASE:
                EnterChase();
                break;
            case State.ATTACK:
                EnterAttack();
                break;
            case State.GUARD:
                EnterGuard();
                break;
            case State.SKILL:
                EnterSkill();
                break;
            case State.DEATH:
                EnterDeath();
                break;
        }
    }

    private void ExecuteState()
    {
        StopIfNearPlayer();

        switch (currentState)
        {
            case State.IDLE:
                ExcuteIdle();
                break;
            case State.PATROL:
                ExcutePatrol();
                break;
            case State.CHASE:
                ExcuteChase();
                break;
            case State.ATTACK:
                ExcuteAttack();
                break;
            case State.GUARD:
                ExcuteGuard();
                break;
            case State.SKILL:
                ExcuteSkill();
                break;
            case State.DEATH:
                ExcuteDeath();
                break;
        }
    }

    private void ExitState(State state)
    {
        switch (currentState)
        {
            case State.IDLE:
                ExitIdle();
                break;
            case State.PATROL:
                ExitPatrol();
                break;
            case State.CHASE:
                ExitChase();
                break;
            case State.ATTACK:
                ExitAttack();
                break;
            case State.GUARD:
                ExitGuard();
                break;
            case State.SKILL:
                ExitSkill();
                break;
            case State.DEATH:
                ExitDeath();
                break;
        }
    }



    #endregion


    #region Enter State
    private void EnterIdle() { }
    private void EnterPatrol() { }
    private void EnterChase() { }
    private void EnterAttack() { }
    private void EnterSkill() 
    {
        Debug.Log("��ų�ߵ�");

        Boss boss = GetComponent<Boss>();
        List<Action> possbileSkills = new List<Action>();

        if (!boss.skillActive)
        {
            possbileSkills.Add(() =>
            {
                animator.SetBool("Boss_Skill01", true);
                animator.SetFloat("Speed", 0f);
                boss.BossSkill01();
            });
        }

        if (!boss.skillActive2)
        {
            possbileSkills.Add(() =>
            {
                boss.BossSkill02();
            });
        }

        if (!boss.skillActive3)
        {
            possbileSkills.Add(() =>
            {
                boss.BossSkill03();
            });
        }

        if (possbileSkills.Count > 0)
        {
            int randomSkillIndex = UnityEngine.Random.Range(0, possbileSkills.Count);
            possbileSkills[randomSkillIndex]();
        }
        else
        {
            Debug.Log("��ų �� ã��");
        }
    }

    private void EnterGuard()
    {
        if (guardRoutine != null)
        {
            StopCoroutine(guardRoutine);
        }

        guardRoutine = StartCoroutine(GuardAnimationRoutine());
    }

    public void EnterDeath()
    {
        isBeingDestroy = true;
        animator.SetTrigger("Death");
        Destroy(gameObject);
        Instantiate(deathPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
    #endregion

    #region ExcuteState
    private void ExcuteIdle()
    {
    }

    private void ExcutePatrol()
    {
        animator.SetFloat("Speed", 1f);
        DistanceToPlayer();

        float distanceToPatrolPoint = Vector3.Distance(transform.position, nextPatrolPoint);

        //���� �ȿ� ���Դٸ� �������·� ��ȯ
        if (DistanceToPlayer() <= detectionRadius)
        {
            ChangeState(State.CHASE);
            return;
        }

        //��ǥ������ ���� �����ߴٸ�
        if (distanceToPatrolPoint <= 0.1f)
        {
            StartCoroutine(StayForAWhile());
            return;
        }

        //���� ������ �ٶ󺸸� �̵��ϱ�
        else
        {
            LookTowardsPoint(nextPatrolPoint);
            MoveTowardsPoint(nextPatrolPoint);
        }
    }

    private void ExcuteChase()
    {
        if (isRecoveringFormBigDamage)
        {
            return;
        }

        animator.SetFloat("Speed", 1.5f);

        DistanceToPlayer();
        LookTowardsPoint(playerTransform.position);

        // �����Ÿ� �̻��϶��� �����Ѵ�
        if (DistanceToPlayer() > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, characterStatManager.currentSpeed * Time.deltaTime);
        }


        if (monster.IsLizard)
        {
            //���� Ÿ���� ���ڵ�� ���� ����
            if (DistanceToPlayer() <= stopDistance)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);

                if (chance <= 0.99f) // 80% Ȯ���� ���� ���� ��ȯ
                {
                    ChangeState(State.ATTACK);
                }
                else   // 10% Ȯ���� ���� ���� ��ȯ
                {
                    ChangeState(State.GUARD);
                }

                return;
            }
        }

        else if (monster.IsOrc)
        {
            //��� Ÿ���� ��ũ�� ���� ����
            if (DistanceToPlayer() <= stopDistance)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);

                if (chance <= 0.99f) //80 % Ȯ���� ��� ���� ��ȯ
                {
                    ChangeState(State.GUARD);
                }
                else
                {
                    ChangeState(State.ATTACK);
                }

                return;
            }
        }

        else if (monster.IsBoss)
        {
            // Boss�� Ư����ų ���
            if (DistanceToPlayer() <= stopDistance)
            {
                float chance = UnityEngine.Random.Range(0f, 1f);

                if (chance <= 0.55f)// 55%Ȯ���� ����)
                {
                    ChangeState(State.ATTACK);
                }
                else if (chance <= 0.95f)  //40%Ȯ���� ��ų
                {
                    ChangeState(State.SKILL);
                }
                else    //5%Ȯ���� ����
                {
                    ChangeState(State.GUARD);
                }

                return;
            }
        }


        //�����Ÿ� �̻� ����� ���� ����
        if (monster.IsLizard && DistanceToPlayer() > detectionRadius)
        {
            ChangeState(State.PATROL);
            return;
        }
    }

    private void ExcuteAttack()
    {
        //���� ���� ������ ���� ����
        if (DistanceToPlayer() <= stopDistance && !isAttacking)
        {
            attackRoutine = StartCoroutine(AttackPlayer());
        }
        //���� ������ ������ ���� ����
        else if (DistanceToPlayer() > stopDistance)
        {
            animator.SetBool("isAttacking", false);
            ChangeState(State.CHASE);
        }
    }

    private void ExcuteGuard()
    {
    }
    private void ExcuteSkill()
    {

    }

    private void ExcuteDeath()
    {
        animator.SetTrigger("Death");
        Destroy(gameObject, 1.6f);
    }

    #endregion

    #region Exit State

    private void ExitIdle() { }

    private void ExitPatrol() { }

    private void ExitChase() { }

    private void ExitAttack() 
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
        }

        animator.SetBool("isAttacking", false);
    }

    private void ExitGuard() { }
    private void ExitSkill() { }
    private void ExitDeath() { }

    #endregion

    #region ������ �޴� ����
    public void TakeDamage(float amount, bool isPredation = false)
    {
        //������ ������ ����� ����.
        float damageToTake = amount - characterStatManager.currentDefense;

        if (damageToTake < 0f)
        {
            damageToTake = 0f;  //���ݷ��� ���º��� ���ٸ� ������ 0
        }

        if (isGuarding)
        {
            damageToTake *= 0.2f;   //�������̶�� �޴� ������ 80%����
        }

        characterStatManager.currentHP -= damageToTake;

        Debug.Log(characterStatManager.currentHP);

        Vector3 monsterWorldPosition = this.transform.position; // ������ ���� ��ġ
        UIMonsterHP.Instance.CreateDamagePopup(damageToTake, monsterWorldPosition);

        if (isPredation && !hasPredationHitSpawned)    //isPredation(�����ų)�� true�϶� �� hit �������� �����Ѵ�.
        {
            GameObject hitInstance = Instantiate(predationHit, transform.position, Quaternion.identity);
            hitInstance.transform.SetParent(transform);
            hasPredationHitSpawned = true;
        }
        else if (!isPredation)
        {
            Vector3 effectPosition = transform.position + new Vector3(0f, 1f, 0f);
            
            if (monster.IsLizard)
            {
                Instantiate(hitPrefab, effectPosition, Quaternion.identity);
            }
            else if (monster.IsOrc)
            {
                Instantiate(hitPrefab, shieldPrefab.transform.position, Quaternion.identity);
            }
        }

        if (characterStatManager.currentHP <= 0f)
        {
            ChangeState(State.DEATH);
        }

        //ū �������� ������ �������� enemy �ִϸ��̼�
        float damagePercentage = damageToTake / characterStatManager.currentMaxHP;
        if (monster.IsLizard && damagePercentage > 0.5f)
        {
            StartCoroutine(PlayBigDamageAnimation());
        }

        if (monster.IsOrc && damagePercentage > 0.25f)
        {
            StartCoroutine(PlayOrcDamageAnimation());
        }
    }

    private IEnumerator PlayOrcDamageAnimation()
    {
        StopIfNearPlayer();
        animator.SetBool("Damage", true);
        yield return new WaitForSeconds(DamageInterval);

        animator.SetBool("BigDamage", false);
        animator.SetBool("StandUp", true);
        yield return new WaitForSeconds(DamageInterval);

        animator.SetBool("StandUp", false);
        yield return new WaitForSeconds(DamageInterval);
    }
    #endregion

    #region ���� ������ �޼���

    private float DistanceToPlayer()    //enemy�� player���� �Ÿ� üũ �޼���
    {
        return Vector3.Distance(transform.position, playerTransform.position);
    }
    private void MoveTowardsPoint(Vector3 targetPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, characterStatManager.currentSpeed * Time.deltaTime);
    }

    public void LookTowardsPoint(Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, characterStatManager.currentSpeed * Time.deltaTime);
    }

    private void StopIfNearPlayer()
    {
        if (monster.IsLizard && monster.IsOrc && DistanceToPlayer() <= stopDistance)
        {
            animator.SetFloat("Speed", 0f);
        }

        //if (monster.IsBoss && DistanceToPlayer() <= 3f)
        //{
        //    animator.SetFloat("Speed", 0f);
        //}
    }
    private IEnumerator PlayBigDamageAnimation()
    {
        StopIfNearPlayer();

        animator.SetBool("BigDamage", true);
        yield return new WaitForSeconds(DamageInterval - 0.3f);
        animator.SetBool("BigDamage", false);

        animator.SetBool("StandUp", true);
        yield return new WaitForSeconds(DamageInterval);

        animator.SetBool("StandUp", false);
        yield return new WaitForSeconds(DamageInterval);
    }
    #endregion

    #region ���� �޼���
    private void ChooseNextPatrolPoint()
    {
        float randomX = UnityEngine.Random.Range(-patrolRadius, patrolRadius);
        float randomZ = UnityEngine.Random.Range(-patrolRadius, patrolRadius);

        nextPatrolPoint = transform.position + new Vector3(randomX, 0f, randomZ);
    }

    private IEnumerator StayForAWhile()
    {
        ChangeState(State.IDLE);
        yield return new WaitForSeconds(patrolDuration);
        ChooseNextPatrolPoint();
        ChangeState(State.PATROL);
    }
    #endregion

    #region ���� �޼���

    private IEnumerator AttackPlayer()
    {
        while (true)
        {
            if (!isAttacking)
            {
                animator.SetFloat("Speed", 0f);
                isAttacking = true;

                yield return new WaitForSeconds(attackInterval);
                animator.SetBool("isAttacking", true);

                yield return new WaitForSeconds(attackInterval -1f);
                SpawnAttackParticle();

                animator.SetBool("isAttacking", false);

                yield return new WaitForSeconds(attackInterval);
                isAttacking = false;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SpawnAttackParticle()
    {
        Vector3 particleDirection = transform.forward;
        Vector3 particlePosition = transform.position + particleDirection;
        particlePosition.y += 1.5f;
        Quaternion particleRotation = Quaternion.LookRotation(particleDirection);

        GameObject particleInstance = Instantiate(attackParticlePrefab, particlePosition, particleRotation);
        ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            monsterWeapon.DealDamageToPlayersInRadius();
        }
    }

    #endregion

    #region ���� �޼���
    private IEnumerator GuardAnimationRoutine()
    {
        isGuarding = true;

        if (monster.IsLizard && monster.IsBoss)
        {
            animator.SetBool("isGuarding", true);

            yield return new WaitForSeconds(guardInterval);

            animator.SetBool("isGuarding", false);
            yield return new WaitForSeconds(guardInterval - 1f);
            ChangeState(State.CHASE);
        }

        if (monster.IsOrc)
        {
            animator.SetBool("isGuarding", true);

            yield return new WaitForSeconds(guardInterval + 1.7f);

            animator.SetBool("isGuarding", false);
            yield return new WaitForSeconds(guardInterval - 1f);
            ChangeState(State.CHASE);
        }

        isGuarding = false;
    }

    #endregion
}