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
    [SerializeField] private FootSoundPlayer footSoundPlayer;

    [Header("캐릭터 정보")]
    [SerializeField] private float playerSpeed;        // 캐릭터 속도
    [SerializeField] private float gunRotSensitivity;  // 총 회전 민감도
    [SerializeField] private float timeToDead;         // 죽을 때 쓰러지는 속도

    [Header("UI")]
    [SerializeField] private Button innerGyro;
    [SerializeField] private Text innerGyroText;

    private Gun gun => gunPivot.GetComponent<Gun>();
    private bool isShoot = false;

    private Animator animator;
    private CharacterController character;
    private AudioSource audioSource;

    private bool isInnerGyro = false;

    private bool isSetting = false;

    private readonly int hashIsWaking = Animator.StringToHash("isWalking");


    private void Start()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

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
        if (!GameManager.instance.isInGame)
            return;

        GunRotation();               // 컨트롤러의 자이로 센서 (각속도 값)을 이용해 총 회전하게 하기
        PlayerHeadRotation();        // 휴대폰 내장 자이로 센서를 이용해 플레이어 머리 회전하게 하기
        PlayerRotation();            // 상체의 자이로 센서를 이용해 플레이어 회전
        MovePlayer();                // 조이스틱을 이용하여 플레이어 이동하게 하기

        InitialGyro();               // 자이로센서의 값을 초기화
    }


    private void MovePlayer()
    {
        if (ic.moveDir != Vector3.zero)
            footSoundPlayer.PlayFootStepSound();
        else
            footSoundPlayer.StopFootStepSound();

        Vector3 moveDir = new Vector3(ic.moveDir.x * playerSpeed, -9.8f, ic.moveDir.z * playerSpeed);
        character.Move(transform.rotation * moveDir * Time.deltaTime);

        animator.SetBool(hashIsWaking, ic.moveDir != Vector3.zero);
    }


    private void PlayerHeadRotation()
    {
        if (isInnerGyro)
        {
            cam.Rotate(new Vector3(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, -Input.gyro.rotationRateUnbiased.z));
        }
    }


    private void PlayerRotation()
    {
        transform.rotation = Quaternion.Euler(ic.bodyRotAngle);
    }


    private void GunRotation()
    {
        gunPivot.rotation = Quaternion.Euler(ic.gunRotAngle);
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


    private void InitialGyro()
    {
        if (ic.trySetting && !isSetting)
        {
            ic.InitRotation();
            isSetting = true;
            
            cam.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if (!ic.trySetting)
            isSetting = false;
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
        if (isDead)
            return;

        Debug.Log("아야");
        currentHp--;
    }


    public override void Dead()
    {
        StartCoroutine(CoDead());

        GameManager.instance.GameOver();
    }


    IEnumerator CoDead()
    {
        float currentTime = 0f;
        Quaternion originRot = transform.rotation;
        Vector3 eulerRot = new Vector3(-90f, originRot.eulerAngles.y, originRot.eulerAngles.z);
        while (true)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeToDead)
                currentTime = timeToDead;

            transform.rotation = Quaternion.Lerp(originRot, Quaternion.Euler(eulerRot), currentTime / timeToDead);

            if (currentTime == timeToDead)
                break;

            yield return null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hands")
        {
            Hit();
        }
    }
}
