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
        animator.SetFloat("Speed", 1.5f);

        DistanceToPlayer();
        LookTowardsPoint(playerTransform.position);

        // �����Ÿ� �̻��϶��� �����Ѵ�
        if (DistanceToPlayer() > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, characterStatManager.currentSpeed * Time.deltaTime);
        }

        // ���� �ȿ� ���Դٸ�
        if (DistanceToPlayer() <= stopDistance)
        {
            float chance = UnityEngine.Random.Range(0f, 1f);

            if (chance <= 0.75f) // 75% Ȯ���� ���� ���� ��ȯ
            {
                ChangeState(State.ATTACK);
            }
            else   // 25% Ȯ���� ���� ���� ��ȯ
            {
                ChangeState(State.GUARD);
            }

            return;
        }

        //�����Ÿ� �̻� ����� ���� ����
        if (DistanceToPlayer() > detectionRadius)
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

    #region ������ �޴� ����
    public void TakeDamage(float amount, bool isPredation = false)
    {
        //������ ������ ����� ����.
        float damageToTake = amount - characterStatManager.currentDefense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //���ݷ��� ���º��� ���ٸ� ������ 0

        characterStatManager.currentHP -= damageToTake;

        //���� Guard �ִϸ��̼��� �������̶�� �޴� ������ ����

        Debug.Log(characterStatManager.currentHP);

        //animator hit �ִϸ��̼� ����

        if (isPredation && !hasPredationHitSpawned)    //isPredation(������ų)�� true�϶� �� hit �������� �����Ѵ�.
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

        if (characterStatManager.currentHP <= 0f)
        {
            ChangeState(State.DEATH);
        }
    }
    #endregion`

    #region ���� ������ �޼���

    private float DistanceToPlayer()    //enemy�� player���� �Ÿ� üũ �޼���
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
            Debug.Log("������ ����");
            monsterWeapon.DealDamageToPlayersInRadius();
        }
    }

    #endregion

    #region ���� �޼���
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