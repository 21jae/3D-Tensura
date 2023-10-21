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
        EVADE,
        DEATH
    }
    public State currentState;

    /// <summary>
    /// ������Ʈ
    /// </summary>

    public CharacterStats enemyStats;
    //[SerializeField] private GameObject hitPrefab;
    //[SerializeField] private GameObject predationHit;
    private Animator animator;
    private Transform playerTransform;


    /// <summary>
    /// Idle �� Patrol
    /// </summary>

    [SerializeField] private float patrolRadius = 5f;   //���� �ݰ�
    [SerializeField] private float patrolDuration = 5f; //�� ��ġ�� �������� �ð�
    [SerializeField] private float detectionRadius = 8f;  //�÷��̾� ���� �Ÿ�
    private Vector3 nextPatrolPoint;

    /// <summary>
    /// Attack
    /// </summary>

    [SerializeField] private float attackInterval = 1.5f;    //���� ����
    private Coroutine attackRoutine;    //���� ������ �ڷ�ƾ�� �����Ѵ�.
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
        //���� ���¿��� ����� ����\
        

    }

    private void EnterEvade()
    {
        //���� ���¿��� ����� ����
        

    }

    private void EnterDeath()
    {
        //���� ���¿��� ����� ����
        

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

        //�÷��̾ ã�� �Ÿ��� ���� ������ ���� ã��
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        float distanceToPatrolPoint = Vector3.Distance(transform.position, nextPatrolPoint);

        //�÷��̾ �����ߴٸ�
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
        // �÷��̾ �߰��ߴٸ� �Ĵٺ����մϴ�.
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, enemyStats.speed * Time.deltaTime);

        // �÷��̾ �߰�
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.transform.position, enemyStats.speed * Time.deltaTime);

        // ���� �Ÿ� �������� AttackState ����
        if (distanceToPlayer <= 2f)
        {
            ChangeState(State.ATTACK);
            return;
        }

        //�׷��� �����Ÿ� �̻� ����� �ٽ� chase���·� ��ȯ
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
            Debug.Log("�������� ������ϴ�.");

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
        //������ ������ ����� ����.
    }


    #region Patrol Methods
    private void ChooseNextPatrolPoint()
    {
        Debug.Log("���� ���� Ž��");

        float randomX = UnityEngine.Random.Range(-patrolRadius, patrolRadius);
        float randomZ = UnityEngine.Random.Range(-patrolRadius, patrolRadius);

        nextPatrolPoint = transform.position + new Vector3(randomX, 0f, randomZ);
    }

    private IEnumerator StayForAWhile()
    {
        Debug.Log("���߰ڽ��ϴ�.");

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
            Debug.Log("������ �ٽ� �ϴ°�?");

            animator.SetBool("isAttacking", true);

            yield return new WaitForSeconds(attackInterval);

            animator.SetBool("isAttacking", false);

            //�ִϸ��̼� ���� (���� ����)�� ����Ͽ� ��¦ ���
            yield return new WaitForSeconds(attackInterval - 1f);

            //���߿� ������ ���� �߰�
        }
    }

    #endregion



}