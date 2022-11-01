using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private InputController ic;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform gunLeftHandPos;
    [SerializeField] private Transform gunRightHandPos;

    [Header("캐릭터 정보")]
    [SerializeField] private float playerSpeed;        // 캐릭터 속도
    [SerializeField] private float gunRotSensitivity;  // 총 회전 민감도

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
        TryShoot();                  // 총 쏘기 시도
    }


    private void FixedUpdate()
    {
        AimGun();                    // 컨트롤러의 자이로 센서 (가속도 값)을 이용해 총 움직이게 하기
        GunRotation();               // 컨트롤러의 자이로 센서 (각속도 값)을 이용해 총 회전하게 하기
        PlayerRotation();            // 머리의 자이로 센서(각속도 값)을 이용해 플레이어 머리 회전하게 하기
        MovePlayer();                // 조이스틱을 이용하여 플레이어 이동하게 하기
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
