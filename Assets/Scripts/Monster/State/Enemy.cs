using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    /// <summary>
    /// ����
    /// </summary>
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
    public CharacterStats enemyStats;

    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject predationHit;

    private Animator animator;
    private Transform playerTransform;

    //Patrol
    [SerializeField] private float patrolRadius = 5f;   //���� �ݰ�
    [SerializeField] private float patrolDuration = 5f; //�� ��ġ�� �������� �ð�
    [SerializeField] private float detectionRadius = 8f;  //�÷��̾� ���� �Ÿ�
    private Vector3 nextPatrolPoint;
    private float stopDistance = 2.5f;  //�÷��̾�� ���� �Ÿ� ����

    //Attack
    [SerializeField] private float attackInterval = 1.5f;    //���� ����
    private Coroutine attackRoutine;    //���� ������ �ڷ�ƾ�� �����Ѵ�.
    private bool isAttacking = false;

    //Guard
    [SerializeField] private float guardInterval = 1.5f;  //���� ���ӽð�
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
            case State.GUARD:
                ExitGuard();
                break;
            case State.DEATH:
                ExitDeath();
                break;
        }
    }

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
            case State.GUARD:
                ExcuteGuard();
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
    }

    private void EnterAttack()
    {
    }

    private void EnterGuard()
    {
        animator.SetFloat("Speed", 0f);

        if (guardRoutine != null)
        {
            StopCoroutine(guardRoutine);
        }

        guardRoutine = StartCoroutine(GuardAnimationRoutine());
    }

    private void EnterDeath()
    {
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

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        float distanceToPatrolPoint = Vector3.Distance(transform.position, nextPatrolPoint);

        if (distanceToPlayer <= detectionRadius)
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
        animator.SetFloat("Speed", 1.5f);

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, enemyStats.speed * Time.deltaTime);

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);

        // �����Ÿ� �̻��϶��� �����Ѵ�. (��ħ ����)
        if (distanceToPlayer > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.transform.position, enemyStats.speed * Time.deltaTime);
        }

        if (distanceToPlayer <= 2.5f)
        {
            animator.SetFloat("Speed", 0f);

            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance <= 0.25f) 
            {
                ChangeState(State.GUARD);
            }
            else
            {
                ChangeState(State.ATTACK);
            }

            return;
        }

        //�����Ÿ� �̻� ����� �������� ����
        if (distanceToPlayer > detectionRadius)
        {
            ChangeState(State.PATROL);
            return;
        }
    }

    private void ExcuteAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);

        if (distanceToPlayer <= 2.5f && !isAttacking)
        {
            animator.SetFloat("Speed", 0f);
            attackRoutine = StartCoroutine(AttackPlayer());
            isAttacking = true;
        }
        else if (distanceToPlayer > 2.5f && isAttacking)
        {
            StopCoroutine(attackRoutine);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
            Debug.Log("�������� ������ϴ�.");

            ChangeState(State.CHASE);
        }
    }


    private void ExcuteGuard()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);

        if (distanceToPlayer <= 2.5f)
        {
            animator.SetFloat("Speed", 0f);
        }
        else if (distanceToPlayer > 2.5f)
        {
            ChangeState(State.CHASE);
        }
    }

    private void ExcuteDeath()
    {
        //animator.SetTrigger("Die");
        Destroy(gameObject, 1.6f);
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

    private void ExitGuard()
    {

    }

    private void ExitDeath()
    {

    }

    #endregion


    public void TakeDamage(float amount, bool isPredation = false)
    {
        //������ ������ ����� ����.
        float damageToTake = amount - enemyStats.defense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //���ݷ��� ���º��� ���ٸ� ������ 0

        enemyStats.currentHealth -= damageToTake;

        //���� Guard �ִϸ��̼��� �������̶�� �޴� ������ ����

        Debug.Log(enemyStats.currentHealth);

        //animator hit �ִϸ��̼� ����


        if (isPredation && !hasPredationHitSpawned)    //isPredation(�����ų)�� true�϶� �� hit �������� �����Ѵ�.
        {
            GameObject hitInstance = Instantiate(predationHit, transform.position, Quaternion.identity);
            hitInstance.transform.SetParent(transform);
            hasPredationHitSpawned = true;
            //���߿� ������ 0���� �����
        }
        else if (!isPredation)
        {
            Vector3 effectPosition = transform.position + new Vector3(0f, 1f, 0f);
            Instantiate(hitPrefab, effectPosition, Quaternion.identity);
        }

        if (enemyStats.currentHealth <= 0f)
        {
            ChangeState(State.DEATH);
        }
    }


    #region Patrol Methods
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


    #region Chase Methods

    #endregion

    #region Attack Methods

    private IEnumerator AttackPlayer()
    {
        while (true)
        {
            animator.SetBool("isAttacking", true);

            yield return new WaitForSeconds(attackInterval);

            animator.SetBool("isAttacking", false);

            //�ִϸ��̼� ���� (���� ����)�� ����Ͽ� ��¦ ���
            yield return new WaitForSeconds(attackInterval - 1f);

            //������ ���� �߰�
        }
    }

    #endregion

    #region Guard Methods

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