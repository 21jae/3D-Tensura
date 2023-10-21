using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    public CharacterStats enemyStats;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject predationHit;

    protected MonsterAI monsterAI;
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;
    protected Transform target;

    //Attack
    protected float attackDelay = 2f;
    protected float reattackTime = 1f;
    private float checkInterval = 0.2f;
    protected float distanceToPlayer;

    public bool isReadyToAttack = true;
    private bool hasPredationHitSpawned;
    public float attackRange = 2f;

    protected float stopTime = 1.5f;
    protected bool canMove = true;

    #region Animator
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int isAttacking = Animator.StringToHash("isAttacking");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Damage = Animator.StringToHash("Damage");
    #endregion

    private void Awake()
    {
        monsterAI = GetComponent<MonsterAI>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        StartCoroutine(CheckPlayerDistance());
        StartStat();
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, target.position);
        MonsterMovement();
    }

    private void StartStat()
    {
        float enemyHealth = enemyStats.maxHealth;
        float enemyAttackPower = enemyStats.attackPower;
        float defense = enemyStats.defense;
    }

    #region ����
    protected virtual IEnumerator CheckPlayerDistance()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (distanceToPlayer < attackRange && isReadyToAttack)
            {
                isReadyToAttack = false;
                StartCoroutine("AttackPlayer");
            }
        }
    }

    protected virtual IEnumerator AttackPlayer()
    {
        canMove = false;
        yield return new WaitForSeconds(attackDelay);   //���� �����ð�

        animator.SetTrigger(isAttacking);   //�Ϲ� ����

        yield return new WaitForSeconds(reattackTime);  //���� ���� ���ð�
        isReadyToAttack = true;

        yield return new WaitForSeconds(stopTime);  //������ ���ð�
        canMove = true;

    }

    #endregion

    #region ������
    protected virtual void MonsterMovement()
    {
        if (!canMove || distanceToPlayer <= attackRange && !isReadyToAttack)
        {
            navMeshAgent.velocity = Vector3.zero;
            animator.SetFloat(Speed, 0f);
        }
        else
        {
            float speed = navMeshAgent.velocity.magnitude;
            animator.SetFloat(Speed, speed);
        }
    }
    #endregion

    #region ������ �ޱ�
    public void TakeDamage(float amount, bool isPredation = false)
    {
        float damageToTake = amount - enemyStats.defense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //���ݷ��� ���º��� ���ٸ� ������ 0

        enemyStats.currentHealth -= damageToTake;

        Debug.Log(enemyStats.currentHealth);
        animator.SetTrigger(Damage);


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
            Death();
        }
    }

    protected virtual void Death()
    {
        animator.SetTrigger(Die);
        navMeshAgent.isStopped = true;
        Destroy(gameObject, 1.6f);
    }

    #endregion
}
