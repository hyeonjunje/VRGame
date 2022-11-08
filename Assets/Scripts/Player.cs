using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    [Header("����")]
    [SerializeField] private InputController ic;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform gunLeftHandPos;
    [SerializeField] private Transform gunRightHandPos;
    [SerializeField] private Transform cam;

    [Header("ĳ���� ����")]
    [SerializeField] private float playerSpeed;        // ĳ���� �ӵ�
    [SerializeField] private float gunRotSensitivity;  // �� ȸ�� �ΰ���

    [Header("UI")]
    [SerializeField] private Button innerGyro;
    [SerializeField] private Text innerGyroText;

    private Gun gun => gunPivot.GetComponent<Gun>();
    private bool isShoot = false;

    private Animator animator;
    private CharacterController character;

    private bool isInnerGyro = false;

    private readonly int hashIsWaking = Animator.StringToHash("isWalking");

    private void Start()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        Input.gyro.enabled = true;  // �޴��� ���� ���̷� ���� enabled

        innerGyro.onClick.AddListener(() => SetGyro());

        currentHp = hp;
    }

    private void SetGyro()
    {
        // ���������� ������ ����
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
        TryShoot();                  // �� ��� �õ�
    }


    private void FixedUpdate()
    {
        AimGun();                    // ��Ʈ�ѷ��� ���̷� ���� (���ӵ� ��)�� �̿��� �� �����̰� �ϱ�
        GunRotation();               // ��Ʈ�ѷ��� ���̷� ���� (���ӵ� ��)�� �̿��� �� ȸ���ϰ� �ϱ�
        PlayerHeadRotation();        // �޴��� ���� ���̷� ������ �̿��� �÷��̾� �Ӹ� ȸ���ϰ� �ϱ�
        PlayerRotation();            // ��ü�� ���̷� ������ �̿��� �÷��̾� ȸ��
        MovePlayer();                // ���̽�ƽ�� �̿��Ͽ� �÷��̾� �̵��ϰ� �ϱ�
    }


    private void MovePlayer()
    {
        Vector3 moveDir = new Vector3(ic.moveDir.x * playerSpeed, -9.8f, ic.moveDir.z * playerSpeed);
        character.Move(transform.rotation * moveDir * Time.deltaTime);

        animator.SetBool(hashIsWaking, ic.moveDir != Vector3.zero);
    }


    private void PlayerHeadRotation()
    {
        if(isInnerGyro)
            cam.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, -Input.gyro.rotationRateUnbiased.z);
    }


    private void PlayerRotation()
    {
        transform.rotation = Quaternion.Euler(ic.bodyRotAngle);
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
        Debug.Log("�ƾ�");
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
