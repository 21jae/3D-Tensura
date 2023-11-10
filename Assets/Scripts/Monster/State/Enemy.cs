using DialogueEditor;
using System.Collections;
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

    [Header("Enemy ����")]
    [HideInInspector] public CharacterStatManager characterStatManager;

    [field: Header("Enemy ������")]
    [field: SerializeField] public EnemyStateData Data { get; private set; }

    [HideInInspector] public Animator animator;
    private Transform playerTransform;
    private MonsterWeapon monsterWeapon;
    private Monster monster;

    private Vector3 nextPatrolPoint;
    private Coroutine attackRoutine;
    public Coroutine guardRoutine;


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
        Data.HitData.SetIsBeingDestroy(true);
        animator.SetTrigger("Death");
        Destroy(gameObject, 2.5f);
        Data.HitData.deathPrefab = ObjectPooling.instance.GetPooledObject("Death");
        Data.HitData.deathPrefab.transform.position = transform.position + Vector3.up;
        Data.HitData.deathPrefab.SetActive(true);
        ConversationManager.Instance.SetBool("WolfClear", true);
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
        if (DistanceToPlayer() <= Data.PatrolData.detectionRadius)
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
        if (Data.HitData.isRecoveringFormBigDamage)
        {
            return;
        }

        animator.SetFloat("Speed", 1.5f);

        DistanceToPlayer();
        LookTowardsPoint(playerTransform.position);

        // �����Ÿ� �̻��϶��� �����Ѵ�
        if (DistanceToPlayer() > Data.PatrolData.stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, characterStatManager.currentData.currentSpeed * Time.deltaTime);
        }


        if (DistanceToPlayer() <= Data.PatrolData.stopDistance)
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance <= 0.7f) // 70% Ȯ���� ���� ���� ��ȯ
            {
                ChangeState(State.ATTACK);
            }
            else
            {
                ChangeState(State.GUARD);
            }

            return;

        }

        //�����Ÿ� �̻� ����� ���� ����
        if (DistanceToPlayer() > Data.PatrolData.detectionRadius)
        {
            ChangeState(State.PATROL);
            return;
        }
    }

    private void ExcuteAttack()
    {
        //���� ���� ������ ���� ����
        if (DistanceToPlayer() <= Data.PatrolData.stopDistance && !Data.AttackData.isAttacking)
        {
            attackRoutine = StartCoroutine(AttackPlayer());
        }
        //���� ������ ������ ���� ����
        else if (DistanceToPlayer() > Data.PatrolData.stopDistance)
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
            Data.AttackData.SetIsAttack(false);
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
        float damageToTake = amount - CharacterStatManager.instance.currentData.currentDefense;

        if (damageToTake < 0f)
        {
            damageToTake = 0f;  //���ݷ��� ���º��� ���ٸ� ������ 0
        }

        CharacterStatManager.instance.currentData.currentHP -= damageToTake;
        Debug.Log(CharacterStatManager.instance.currentData.currentHP);

        //Vector3 effectPosition = transform.position + new Vector3(0f, 1f, 0f);
        //Data.HitData.hitPrefab = ObjectPooling.instance.GetPooledObject("DamageHit");
        //Data.HitData.hitPrefab.transform.position = effectPosition;
        //Data.HitData.hitPrefab.transform.rotation = Quaternion.identity;

        if (CharacterStatManager.instance.currentData.currentHP <= 0f)
        {
            ChangeState(State.DEATH);
        }
    }

    #endregion

    #region ���� ������ �޼���

    private float DistanceToPlayer()    //enemy�� player���� �Ÿ� üũ �޼���
    {
        return Vector3.Distance(transform.position, playerTransform.position);
    }
    private void MoveTowardsPoint(Vector3 targetPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, characterStatManager.currentData.currentSpeed * Time.deltaTime);
    }

    public void LookTowardsPoint(Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, characterStatManager.currentData.currentSpeed * Time.deltaTime);
    }

    private void StopIfNearPlayer()
    {
        if (DistanceToPlayer() <= Data.PatrolData.stopDistance)
        {
            animator.SetFloat("Speed", 0f);
        }
    }
    #endregion

    #region ���� �޼���
    private void ChooseNextPatrolPoint()
    {
        float randomX = UnityEngine.Random.Range(-Data.PatrolData.patrolRadius, Data.PatrolData.patrolRadius);
        float randomZ = UnityEngine.Random.Range(-Data.PatrolData.patrolRadius, Data.PatrolData.patrolRadius);

        nextPatrolPoint = transform.position + new Vector3(randomX, 0f, randomZ);
    }

    private IEnumerator StayForAWhile()
    {
        ChangeState(State.IDLE);
        yield return new WaitForSeconds(Data.PatrolData.patrolDuration);
        ChooseNextPatrolPoint();
        ChangeState(State.PATROL);
    }
    #endregion

    #region ���� �޼���

    private IEnumerator AttackPlayer()
    {
        while (true)
        {
            if (!Data.AttackData.isAttacking)
            {
                animator.SetFloat("Speed", 0f);
                Data.AttackData.SetIsAttack(true);

                yield return new WaitForSeconds(Data.AttackData.attackInterval);
                animator.SetBool("isAttacking", true);

                yield return new WaitForSeconds(Data.AttackData.attackInterval - 1f);
                SpawnAttackParticle();

                animator.SetBool("isAttacking", false);

                yield return new WaitForSeconds(Data.AttackData.attackInterval);
                Data.AttackData.SetIsAttack(false);

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

        GameObject particleInstance = ObjectPooling.instance.GetPooledObject("MonsterSlash");
        particleInstance.transform.position = particlePosition;
        particleInstance.transform.rotation = particleRotation;

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
        Data.GuardData.SetIsGuarding(true);

        animator.SetBool("isGuarding", true);

        yield return new WaitForSeconds(Data.GuardData.guardInterval);

        animator.SetBool("isGuarding", false);
        yield return new WaitForSeconds(Data.GuardData.guardInterval - 1f);
        ChangeState(State.CHASE);

        Data.GuardData.SetIsGuarding(false);

    }

    #endregion
}