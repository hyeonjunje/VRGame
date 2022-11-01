using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private InputController ic;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform gunLeftHandPos;
    [SerializeField] private Transform gunRightHandPos;

    [Header("ĳ���� ����")]
    [SerializeField] private float playerSpeed;        // ĳ���� �ӵ�
    [SerializeField] private float gunRotSensitivity;  // �� ȸ�� �ΰ���

    private Gun gun => gunPivot.GetComponent<Gun>();
    private bool isShoot = false;

    private Animator animator;
    private Rigidbody myRigid;


    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        TryShoot();                  // �� ��� �õ�
    }


    private void FixedUpdate()
    {
        AimGun();                    // ��Ʈ�ѷ��� ���̷� ���� (���ӵ� ��)�� �̿��� �� �����̰� �ϱ�
        GunRotation();               // ��Ʈ�ѷ��� ���̷� ���� (���ӵ� ��)�� �̿��� �� ȸ���ϰ� �ϱ�
        PlayerRotation();            // �Ӹ��� ���̷� ����(���ӵ� ��)�� �̿��� �÷��̾� �Ӹ� ȸ���ϰ� �ϱ�
        MovePlayer();                // ���̽�ƽ�� �̿��Ͽ� �÷��̾� �̵��ϰ� �ϱ�
    }


    private void MovePlayer()
    {
        myRigid.velocity = ic.moveDir * playerSpeed;
    }


    private void PlayerRotation()
    {
    }


    private void GunRotation()
    {
        gunPivot.rotation = Quaternion.Euler(ic.gunRotAngle);
        Debug.Log(gunPivot.rotation);
    }



    private void AimGun()
    {
    }


    private void TryShoot()
    {
        if (ic.isFire)
        {
            if (!isShoot)
            {
                gun.Shoot();
                isShoot = true;
            }
        }
        else
            isShoot = false;
    }


    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, gunLeftHandPos.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, gunLeftHandPos.rotation);


        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, gunRightHandPos.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, gunRightHandPos.rotation);
    }
}
