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

    [Header("Enemy 스탯")]
    [HideInInspector] public CharacterStatManager characterStatManager;

    [field: Header("Enemy 데이터")]
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
        Debug.Log(" 현재 상태 : " + currentState);

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

        //범위 안에 들어왔다면 추적상태로 전환
        if (DistanceToPlayer() <= Data.PatrolData.detectionRadius)
        {
            ChangeState(State.CHASE);
            return;
        }

        //목표지점에 거의 도달했다면
        if (distanceToPatrolPoint <= 0.1f)
        {
            StartCoroutine(StayForAWhile());
            return;
        }

        //순찰 지점을 바라보며 이동하기
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

        // 일정거리 이상일때만 추적한다
        if (DistanceToPlayer() > Data.PatrolData.stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, characterStatManager.currentData.currentSpeed * Time.deltaTime);
        }


        if (DistanceToPlayer() <= Data.PatrolData.stopDistance)
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance <= 0.7f) // 70% 확률로 공격 상태 전환
            {
                ChangeState(State.ATTACK);
            }
            else
            {
                ChangeState(State.GUARD);
            }

            return;

        }

        //일정거리 이상 벗어나면 추적 종료
        if (DistanceToPlayer() > Data.PatrolData.detectionRadius)
        {
            ChangeState(State.PATROL);
            return;
        }
    }

    private void ExcuteAttack()
    {
        //범위 내로 들어오면 공격 실행
        if (DistanceToPlayer() <= Data.PatrolData.stopDistance && !Data.AttackData.isAttacking)
        {
            attackRoutine = StartCoroutine(AttackPlayer());
        }
        //범위 밖으로 나가면 공격 종료
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

    #region 데미지 받는 로직
    public void TakeDamage(float amount, bool isPredation = false)
    {
        //데미지 받을때 수행될 로직.
        float damageToTake = amount - CharacterStatManager.instance.currentData.currentDefense;

        if (damageToTake < 0f)
        {
            damageToTake = 0f;  //공격력이 방어력보다 낮다면 데미지 0
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

    #region 재사용 가능한 메서드

    private float DistanceToPlayer()    //enemy와 player간의 거리 체크 메서드
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

    #region 순찰 메서드
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

    #region 공격 메서드

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

    #region 가드 메서드
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