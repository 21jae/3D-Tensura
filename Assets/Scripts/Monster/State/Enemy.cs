using System;
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
        DEATH
    }
    public State currentState;

    [HideInInspector] public CharacterStatManager characterStatManager;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject predationHit;
    [SerializeField] private GameObject attackParticlePrefab;
    [SerializeField] private GameObject deathPrefab;

    private Animator animator;
    private Transform playerTransform;
    private MonsterWeapon monsterWeapon;

    //Patrol
    [SerializeField] private float patrolRadius = 5f;   //순찰 반경
    [SerializeField] private float patrolDuration = 5f; //한 위치에 멈춰있을 시간
    [SerializeField] private float detectionRadius = 8f;  //플레이어 감지 거리
    private Vector3 nextPatrolPoint;
    private float stopDistance = 2.5f;  //플레이어와 일정 거리 유지

    //Attack
    [SerializeField] private float attackInterval = 1.5f;    //공격 간격
    private Coroutine attackRoutine;    //현재 공격을 코루틴에 저장한다.
    private bool isAttacking = false;

    //Guard
    [SerializeField] private float guardInterval = 1.5f;  //가드 지속시간
    private Coroutine guardRoutine;
    private bool hasPredationHitSpawned = false;


    private void Awake()
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


        ChooseNextPatrolPoint();
    }

    private void Update()
    {
        ExecuteState();
    }

    private void ChangeState(State newState)
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
    private void EnterGuard()
    {
        if (guardRoutine != null)
        {
            StopCoroutine(guardRoutine);
        }

        guardRoutine = StartCoroutine(GuardAnimationRoutine());
    }

    private void EnterDeath()
    {
        animator.SetTrigger("Death");
        Destroy(gameObject, 1.5f);
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

        //범위 안에 들어왔다면 추적상태로 전환
        if (DistanceToPlayer() <= detectionRadius)
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
        animator.SetFloat("Speed", 1.5f);

        DistanceToPlayer();
        LookTowardsPoint(playerTransform.position);

        // 일정거리 이상일때만 추적한다
        if (DistanceToPlayer() > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, characterStatManager.currentSpeed * Time.deltaTime);
        }

        // 범위 안에 들어왔다면
        if (DistanceToPlayer() <= stopDistance)
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance <= 0.75f) // 75% 확률로 공격 상태 전환
            {
                ChangeState(State.ATTACK);
            }
            else   // 25% 확률로 가드 상태 전환
            {
                ChangeState(State.GUARD);
            }

            return;
        }

        //일정거리 이상 벗어나면 추적 종료
        if (DistanceToPlayer() > detectionRadius)
        {
            ChangeState(State.PATROL);
            return;
        }
    }

    private void ExcuteAttack()
    {
        //범위 내로 들어오면 공격 실행
        if (DistanceToPlayer() <= stopDistance && !isAttacking)
        {
            attackRoutine = StartCoroutine(AttackPlayer());
        }
        //범위 밖으로 나가면 공격 종료
        else if (DistanceToPlayer() > stopDistance)
        {
            animator.SetBool("isAttacking", false);
            ChangeState(State.CHASE);
        }
    }

    private void ExcuteGuard()
    {
    }

    private void ExcuteDeath()
    {
        Destroy(gameObject, 1.6f);
    }

    #endregion

    #region Exit State

    private void ExitIdle() { }

    private void ExitPatrol() { }

    private void ExitChase() { }

    private void ExitAttack() { }

    private void ExitGuard() { }

    private void ExitDeath() { }

    #endregion

    #region 데미지 받는 로직
    public void TakeDamage(float amount, bool isPredation = false)
    {
        //데미지 받을때 수행될 로직.
        float damageToTake = amount - characterStatManager.currentDefense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //공격력이 방어력보다 낮다면 데미지 0

        characterStatManager.currentHP -= damageToTake;

        //만약 Guard 애니메이션이 실행중이라면 받는 데미지 절반

        Debug.Log(characterStatManager.currentHP);

        //animator hit 애니메이션 실행

        if (isPredation && !hasPredationHitSpawned)    //isPredation(흡수스킬)이 true일땐 이 hit 프리팹을 생성한다.
        {
            GameObject hitInstance = Instantiate(predationHit, transform.position, Quaternion.identity);
            hitInstance.transform.SetParent(transform);
            hasPredationHitSpawned = true;
            //나중에 움직임 0으로 만들기
        }
        else if (!isPredation)
        {
            Vector3 effectPosition = transform.position + new Vector3(0f, 1f, 0f);
            Instantiate(hitPrefab, effectPosition, Quaternion.identity);
        }

        if (characterStatManager.currentHP <= 0f)
        {
            ChangeState(State.DEATH);
        }
    }
    #endregion`

    #region 재사용 가능한 메서드

    private float DistanceToPlayer()    //enemy와 player간의 거리 체크 메서드
    {
        return Vector3.Distance(transform.position, playerTransform.position);
    }
    private void MoveTowardsPoint(Vector3 targetPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, characterStatManager.currentSpeed * Time.deltaTime);
    }

    private void LookTowardsPoint(Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, characterStatManager.currentSpeed * Time.deltaTime);
    }

    private void StopIfNearPlayer()
    {
        if (DistanceToPlayer() <= stopDistance)
        {
            animator.SetFloat("Speed", 0f);
        }
    }
    #endregion

    #region 순찰 메서드
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

    #region 공격 메서드

    private IEnumerator AttackPlayer()
    {
        while (true)
        {
            if (!isAttacking)
            {
                animator.SetFloat("Speed", 0f);
                isAttacking = true;

                yield return new WaitForSeconds(attackInterval -1f);
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

        if (ps.CompareTag("Player"))
        {
            Debug.Log("데미지 전달");
            monsterWeapon.DealDamageToPlayersInRadius();
        }
    }

    #endregion

    #region 가드 메서드
    private IEnumerator GuardAnimationRoutine()
    {
        animator.SetBool("isGuarding", true);

        yield return new WaitForSeconds(guardInterval);

        animator.SetBool("isGuarding", false);
        yield return new WaitForSeconds(guardInterval - 1f);
        ChangeState(State.CHASE);
    }

    #endregion
}