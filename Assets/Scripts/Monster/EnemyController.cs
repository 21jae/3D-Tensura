using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    public CharacterStats enemyStats;
    [SerializeField] private GameObject hitPrefab;
    protected MonsterAI monsterAI;
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;
    protected Transform target;

    [SerializeField] private float originalSpeed;

    //Attack
    protected float attackDelay = 2f;
    protected float reattackTime = 1f;
    private float checkInterval = 0.2f;
    protected float distanceToPlayer;

    public bool isReadyToAttack = true;
    public float attackRange = 2f;

    protected float stopTime = 1.5f;
    protected bool canMove = true;

    private float originalAttackDelay;
    private float originalReattackTime;
    private float originalStopTime;

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

        originalSpeed = navMeshAgent.speed;

        //슬로우
        originalAttackDelay = attackDelay;
        originalReattackTime = reattackTime;
        originalStopTime = stopTime;
    }

    protected virtual void Start()
    {
        StartCoroutine(CheckPlayerDistance());

        float enemyHealth = enemyStats.maxHealth;
        float enemyAttackPower = enemyStats.attackPower;
        float defense = enemyStats.defense;
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, target.position);

        MonsterMovement();
    }

    #region 공격
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
        yield return new WaitForSeconds(attackDelay);   //공격 지연시간

        animator.SetTrigger(isAttacking);   //일반 공격

        yield return new WaitForSeconds(reattackTime);  //공격 재사용 대기시간
        isReadyToAttack = true;

        yield return new WaitForSeconds(stopTime);  //공격후 대기시간
        canMove = true;

    }

    #endregion

    #region 움직임
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

    public void ApplySlowEffect(float slowFactor)
    {
        navMeshAgent.speed = originalSpeed * slowFactor;
        animator.speed = slowFactor;

        attackDelay = originalAttackDelay / slowFactor;
        reattackTime = originalReattackTime / slowFactor;
        stopTime = originalStopTime / slowFactor;
    }

    public void RemoveSlowEffect()
    {
        navMeshAgent.speed = originalSpeed;
        animator.speed = 1;

        attackDelay = originalAttackDelay;
        reattackTime = originalReattackTime;
        stopTime = originalStopTime;
    }
    #endregion

    #region 데미지 받기
    public void TakeDamage(float amount)
    {
        float damageToTake = amount - enemyStats.defense;

        if (damageToTake < 0f)
            damageToTake = 0f;  //공격력이 방어력보다 낮다면 데미지 0

        enemyStats.currentHealth -= damageToTake;

        Debug.Log(enemyStats.currentHealth);
        animator.SetTrigger(Damage);
        Vector3 effectPosition = transform.position + new Vector3(0f, 1f, 0f);
        Instantiate(hitPrefab, effectPosition, Quaternion.identity);

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
