using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    [Header("연결")]
    [SerializeField] private InputController ic;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform gunLeftHandPos;
    [SerializeField] private Transform gunRightHandPos;
    [SerializeField] private Transform cam;

    [Header("캐릭터 정보")]
    [SerializeField] private float playerSpeed;        // 캐릭터 속도
    [SerializeField] private float gunRotSensitivity;  // 총 회전 민감도

    [Header("UI")]
    [SerializeField] private Button innerGyro;
    [SerializeField] private Text innerGyroText;

    private Gun gun => gunPivot.GetComponent<Gun>();
    private bool isShoot = false;

    private Animator animator;
    private Rigidbody myRigid;

    private bool isInnerGyro = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();

        Input.gyro.enabled = true;  // 휴대폰 내장 자이로 센서 enabled

        innerGyro.onClick.AddListener(() => SetGyro());

        currentHp = hp;
    }

    private void SetGyro()
    {
        // 켜져있을때 누르면 꺼짐
        if(isInnerGyro)  
        {
            innerGyroText.text = "Off";
            isInnerGyro = false;
        }
        else
        {
            innerGyroText.text = "On";
            isInnerGyro = true;
        }
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
        if(isInnerGyro)
            cam.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, -Input.gyro.rotationRateUnbiased.z);
    }


    private void GunRotation()
    {
        gunPivot.rotation = Quaternion.Euler(ic.gunRotAngle);
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

    public override void Hit()
    {
        Debug.Log("아야");
    }


    public override void Dead()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hands")
        {
            Hit();
        }
    }
}
