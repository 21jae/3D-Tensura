using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    /// <summary>
    /// 상태
    /// </summary>
    public enum State
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        EVADE,
        DEATH
    }
    public State currentState;

    /// <summary>
    /// 컴포넌트
    /// </summary>

    public CharacterStats enemyStats;
    //[SerializeField] private GameObject hitPrefab;
    //[SerializeField] private GameObject predationHit;
    private Animator animator;
    private Transform playerTransform;


    /// <summary>
    /// Idle 및 Patrol
    /// </summary>

    [SerializeField] private float patrolRadius = 5f;   //순찰 반경
    [SerializeField] private float patrolDuration = 5f; //한 위치에 멈춰있을 시간
    [SerializeField] private float detectionRadius = 8f;  //플레이어 감지 거리
    private Vector3 nextPatrolPoint;

    /// <summary>
    /// Attack
    /// </summary>

    [SerializeField] private float attackInterval = 1.5f;    //공격 간격
    private Coroutine attackRoutine;    //현재 공격을 코루틴에 저장한다.
    private bool isAttacking = false;


    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        ChangeState(State.PATROL);
        animator = GetComponentInChildren<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
            case State.EVADE:
                ExitEvade();
                break;
            case State.DEATH:
                ExitDeath();
                break;
        }
    }

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
            case State.EVADE:
                EnterEvade();
                break;
            case State.DEATH:
                EnterDeath();
                break;
        }
    }

    private void ExecuteState()
    {
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
            case State.EVADE:
                ExcuteEvade();
                break;
            case State.DEATH:
                ExcuteDeath();
                break;
        }
    }
    #endregion


    #region Enter State
    private void EnterIdle()
    {
        
    }

    private void EnterPatrol()
    {
        

    }
    private void EnterChase()
    {
        
        animator.SetFloat("Speed", 1.5f);
    }

    private void EnterAttack()
    {
        //현재 상태에서 수행될 로직\
        

    }

    private void EnterEvade()
    {
        //현재 상태에서 수행될 로직
        

    }

    private void EnterDeath()
    {
        //현재 상태에서 수행될 로직
        

    }
    #endregion

    #region ExcuteState
    private void ExcuteIdle()
    {
        animator.SetFloat("Speed", 0f);
    }

    private void ExcutePatrol()
    {
        animator.SetFloat("Speed", 1f);

        //플레이어를 찾는 거리와 다음 움직일 지점 찾기
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        float distanceToPatrolPoint = Vector3.Distance(transform.position, nextPatrolPoint);

        //플레이어를 감지했다면
        if (distanceToPlayer <= detectionRadius)
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
        else
        {
            Vector3 moveDirection = (nextPatrolPoint - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, enemyStats.speed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, nextPatrolPoint, enemyStats.speed * Time.deltaTime);
        }
    }

    private void ExcuteChase()
    {
        // 플레이어를 발견했다면 쳐다보게합니다.
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, enemyStats.speed * Time.deltaTime);

        // 플레이어를 추격
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.transform.position, enemyStats.speed * Time.deltaTime);

        // 일정 거리 좁혀지면 AttackState 수행
        if (distanceToPlayer <= 2f)
        {
            ChangeState(State.ATTACK);
            return;
        }

        //그러나 일정거리 이상 벗어나면 다시 chase상태로 전환
        if (distanceToPlayer > detectionRadius)
        {
            ChangeState(State.PATROL);
            return;
        }

    }

    private void ExcuteAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);

        if (distanceToPlayer <= 2f && !isAttacking)
        {

            animator.SetFloat("Speed", 0f);
            attackRoutine = StartCoroutine(AttackPlayer());
            isAttacking = true;
        }
        else if(distanceToPlayer > 2f && isAttacking)
        {
            StopCoroutine(attackRoutine);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
            Debug.Log("범위에서 벗어났습니다.");

            ChangeState(State.CHASE);
        }
    }


    private void ExcuteEvade()
    {

    }

    private void ExcuteDeath()
    {

    }
    #endregion

    #region Exit State

    private void ExitIdle()
    {

    }

    private void ExitPatrol()
    {

    }

    private void ExitChase()
    {

    }

    private void ExitAttack()
    {

    }

    private void ExitEvade()
    {

    }

    private void ExitDeath()
    {

    }

    #endregion


    public void TakeDamage(float amount, bool isPredation = false)
    {
        //데미지 받을때 수행될 로직.
    }


    #region Patrol Methods
    private void ChooseNextPatrolPoint()
    {
        Debug.Log("순찰 지점 탐색");

        float randomX = UnityEngine.Random.Range(-patrolRadius, patrolRadius);
        float randomZ = UnityEngine.Random.Range(-patrolRadius, patrolRadius);

        nextPatrolPoint = transform.position + new Vector3(randomX, 0f, randomZ);
    }

    private IEnumerator StayForAWhile()
    {
        Debug.Log("멈추겠습니다.");

        ChangeState(State.IDLE);
        yield return new WaitForSeconds(patrolDuration);
        ChooseNextPatrolPoint();
        ChangeState(State.PATROL);
    }
    #endregion


    #region Chase Methods
    #endregion

    #region Attack Methods

    private IEnumerator AttackPlayer()
    {
        while (true)
        {
            Debug.Log("공격을 다시 하는가?");

            animator.SetBool("isAttacking", true);

            yield return new WaitForSeconds(attackInterval);

            animator.SetBool("isAttacking", false);

            //애니메이션 길이 (공격 간격)을 고려하여 살짝 대기
            yield return new WaitForSeconds(attackInterval - 1f);

            //나중에 데미지 로직 추가
        }
    }

    #endregion



}