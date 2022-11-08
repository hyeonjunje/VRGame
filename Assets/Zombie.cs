using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    [SerializeField] private float attackableRange;
    [SerializeField] private float attackTime;
    [SerializeField] private float hitTime;

    [SerializeField] private BoxCollider rightBoxCol, leftBoxCol;
    
    private Animator animator;
    private NavMeshAgent nav;
    private FieldOfView fov;

    private Transform target;

    private bool readyToAttack = false;   // 공격할 준비 (쿨타임마다 공격할 수 있게)
    private float attackTimer = 0f;
    private Coroutine coAttack;

    private bool isHit = false;
    private Coroutine coHit;

    private readonly int hashIsDeath = Animator.StringToHash("isDeath");
    private readonly int hashIsAttack = Animator.StringToHash("isAttack");
    private readonly int hashIsHitHead = Animator.StringToHash("isHitHead");
    private readonly int hashIsHit = Animator.StringToHash("isHit");
    private readonly int hashIsMove = Animator.StringToHash("isMove");

    private void Start()
    {
        animator = GetComponent<Animator>();

        nav = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();

        target = FindObjectOfType<Player>().transform;

        currentHp = hp;
    }


    private void Update()
    {
        Move();
        TryAttack();
    }


    private void Move()
    {
        // 죽거나 공격중이거나 맞을때 이동 불가
        if (fov.canSeePlayer && !isDead && !readyToAttack && !isHit)
        {
            nav.SetDestination(target.position);
            animator.SetBool(hashIsMove, true);
        }
        else
        {
            nav.ResetPath();
            animator.SetBool(hashIsMove, false);
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
        rightBoxCol.enabled = true;
        leftBoxCol.enabled = true;
        yield return new WaitForSeconds(0.1f);
        rightBoxCol.enabled = false;
        leftBoxCol.enabled = false;
    }


    public override void Hit()
    {
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
        animator.SetTrigger(hashIsDeath);
    }


    IEnumerator CoHit(int hitType)
    {
        isHit = true;
        animator.SetTrigger(hitType);

        yield return new WaitForSeconds(hitTime);

        isHit = false;
    }

}
