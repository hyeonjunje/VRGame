using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : LivingEntity
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackableRange;
    [SerializeField] private float attackTime;
    [SerializeField] private float hitTime;

    [SerializeField] private BoxCollider rightBoxCol, leftBoxCol;
    
    private Rigidbody myRigid;
    private Animator animator;

    private Transform target;

    private bool readyToAttack = false;   // 공격할 준비 (쿨타임마다 공격할 수 있게)
    private float attackTimer = 0f;
    private Coroutine coAttack;

    private bool isHit = false;
    private Coroutine coHit;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();

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
        if (!isDead && !readyToAttack && !isHit)
        {
            myRigid.velocity = (target.position - transform.position).normalized * moveSpeed;
            animator.SetBool("isMove", true);
        }
        else
        {
            myRigid.velocity = Vector3.zero;
            animator.SetBool("isMove", false);
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
        animator.SetTrigger("isAttack");
        yield return new WaitForSeconds(1f);
        rightBoxCol.enabled = true;
        leftBoxCol.enabled = true;
        yield return new WaitForSeconds(0.1f);
        rightBoxCol.enabled = false;
        leftBoxCol.enabled = false;
    }


    public override void Hit()
    {
        currentHp--;
        Debug.Log(currentHp);

        if (isDead)
            return;

        if (coHit != null)
            StopCoroutine(coHit);
        coHit = StartCoroutine(CoHit("isHit"));
    }


    public void HitHead()
    {
        currentHp -= 3;
        Debug.Log(currentHp);
        if (isDead)
            return;

        if (coHit != null)
            StopCoroutine(coHit);
        coHit = StartCoroutine(CoHit("isHitHead"));
    }


    public override void Dead()
    {
        animator.SetTrigger("isDeath");
    }


    IEnumerator CoHit(string hitType)
    {
        isHit = true;
        animator.SetTrigger(hitType);

        yield return new WaitForSeconds(hitTime);

        isHit = false;
    }

}
