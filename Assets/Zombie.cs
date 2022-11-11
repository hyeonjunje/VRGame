using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    [SerializeField] private float attackableRange;
    [SerializeField] private float attackTime;
    [SerializeField] private float hitTime;
    [SerializeField] private float intervalIdleSound = 5f;

    [SerializeField] private float searchTime; // 탐색 시간

    [SerializeField] private float IdleSpeed;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private BoxCollider rightBoxCol, leftBoxCol;

    [HideInInspector] public ZombieSpawner zombieSpawner;

    public NavMeshAgent agent;

    private bool isPatrol;
    private bool isPatrolZombie;

    private PatrolRoutine patrolRoutine;
    private Transform patrolDestination;

    private Animator animator;
    private FieldOfView fov;
    private ZombieSoundPlayer zombieSoundPlayer;

    private Transform target;

    private float idleTimer = 0f;
    private float originIntervalIdleSound;

    private bool readyToAttack = false;   // 공격할 준비 (쿨타임마다 공격할 수 있게)
    private float attackTimer = 0f;
    private Coroutine coAttack;

    private bool isHit = false;
    private Coroutine coHit;

    private Coroutine coSearch;

    private readonly int hashIsDeath = Animator.StringToHash("isDeath");
    private readonly int hashIsAttack = Animator.StringToHash("isAttack");
    private readonly int hashIsHitHead = Animator.StringToHash("isHitHead");
    private readonly int hashIsHit = Animator.StringToHash("isHit");
    private readonly int hashIsMove = Animator.StringToHash("isMove");

    private void Start()
    {
        animator = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();
        zombieSoundPlayer = GetComponent<ZombieSoundPlayer>();

        target = FindObjectOfType<Player>().transform;
        currentHp = hp;

        originIntervalIdleSound = intervalIdleSound;

        agent.speed = IdleSpeed;
    }


    public void SetPatrolInfo(bool _isPatrolZombie, PatrolRoutine _patrolRoutine)
    {
        isPatrolZombie = _isPatrolZombie;
        isPatrol = isPatrolZombie;

        patrolRoutine = _patrolRoutine;
    }


    public void MyReset()
    {
        currentHp = hp;
        isPatrol = false;
        isPatrolZombie = false;
    }


    private void Update()
    {
        if (isDead)
            return;

        Move();
        TryAttack();
        Idle();
    }


    private void Idle()
    {
        idleTimer += Time.deltaTime;
        if(idleTimer >= intervalIdleSound)
        {
            intervalIdleSound += Random.Range(-0.2f, 0.2f);
            intervalIdleSound = Mathf.Clamp(intervalIdleSound, originIntervalIdleSound - 1, originIntervalIdleSound + 1);
            idleTimer = 0;
            zombieSoundPlayer.PlayIdleSound();
        }
    }


    private void Move()
    {
        if (agent.enabled == false)
            return;

        if (isPatrolZombie && isPatrol)
            Patrol();

        else
        {
            // 죽거나 공격중이거나 맞을때 이동 불가
            if (fov.canSeePlayer && !isDead && !readyToAttack && !isHit)
            {
                if (agent.speed == IdleSpeed)
                    agent.speed = chaseSpeed;

                agent.SetDestination(target.position);
                animator.SetBool(hashIsMove, true);
            }
            else
            {
                agent.ResetPath();
                animator.SetBool(hashIsMove, false);

                if (isPatrolZombie)
                    isPatrol = true;
            }
        }
    }


    private void Patrol()
    {
        if(fov.canSeePlayer)
        {
            isPatrol = false;
        }
        else
        {
            isPatrol = true;

            if (Vector3.Distance(transform.position, patrolRoutine.startPos.position) < 0.1f)
                patrolDestination = patrolRoutine.endPos;
            else if (Vector3.Distance(transform.position, patrolRoutine.endPos.position) < 0.1f)
                patrolDestination = patrolRoutine.startPos;

            agent.SetDestination(patrolDestination.position);
            animator.SetBool(hashIsMove, true);
        }
    }


    private void TryAttack()
    {
        readyToAttack = Vector3.Distance(target.position, transform.position) < attackableRange;

        if (!isDead && readyToAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackTime)
            {
                attackTimer = 0f;

                if (coAttack != null)
                    StopCoroutine(coAttack);
                coAttack = StartCoroutine(CoAttack());
            }
        }
        else
            attackTimer = 0;
    }


    IEnumerator CoAttack()
    {
        animator.SetTrigger(hashIsAttack);
        yield return new WaitForSeconds(1f);

        zombieSoundPlayer.PlayAttackSound();

        rightBoxCol.enabled = true;
        leftBoxCol.enabled = true;
        yield return new WaitForSeconds(0.1f);
        rightBoxCol.enabled = false;
        leftBoxCol.enabled = false;
    }


    public override void Hit()
    {
        zombieSoundPlayer.PlayHurtSound();

        if (isDead)
            return;

        currentHp--;

        if (isDead)
            return;

        if (coHit != null)
            StopCoroutine(coHit);
        coHit = StartCoroutine(CoHit(hashIsHit));
    }


    public void HitHead()
    {
        zombieSoundPlayer.PlayHurtSound();

        if (isDead)
            return;

        currentHp -= 3;
        if (isDead)
            return;

        if (coHit != null)
            StopCoroutine(coHit);
        coHit = StartCoroutine(CoHit(hashIsHitHead));
    }


    public override void Dead()
    {
        StopAllCoroutines();
        animator.SetTrigger(hashIsDeath);

        agent.enabled = false;
    }


    IEnumerator CoHit(int hitType)
    {
        isHit = true;
        animator.SetTrigger(hitType);

        yield return new WaitForSeconds(hitTime);

        isHit = false;

        Search();
    }


    public void Search()
    {
        if (isDead)
            return;

        if (fov.canSeePlayer)
            return;

        if (coSearch != null)
            StopCoroutine(coSearch);
        coSearch = StartCoroutine(CoSearch());
    }


    public IEnumerator CoSearch()
    {
        float currentTime = 0;

        while(true)
        {
            InfiniteLoopDetector.Run();

            currentTime += Time.deltaTime;

            if (currentTime >= searchTime)
                currentTime = searchTime;

            if (agent.speed != IdleSpeed)
                agent.speed = IdleSpeed;

            agent.SetDestination(target.position);
            animator.SetBool(hashIsMove, true);
            if (fov.canSeePlayer)
                break;

            if (currentTime == searchTime)
            {
                animator.SetBool(hashIsMove, false);
                break;
            }
            yield return null;
        }
    }
}
