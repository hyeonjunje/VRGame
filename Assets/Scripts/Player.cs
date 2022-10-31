using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private InputController ic;
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform gunHandle;

    [Header("캐릭터 정보")]
    [SerializeField] private float playerSpeed;

    private Gun gun => gunPivot.GetComponent<Gun>();
    private bool isShoot = false;

    private bool isStartGunRotation = false;
    private Vector3 offset;

    private Animator animator;
    private Rigidbody myRigid;


    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        TryShoot();
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
        float xMove = (ic.InputData["Horizontal"] - 518) / 518;
        float zMove = (ic.InputData["Vertical"] - 518) / 518;

        xMove = Mathf.Abs(xMove - 0) < 0.1f ? 0f : xMove;
        zMove = Mathf.Abs(zMove - 0) < 0.1f ? 0f : zMove;

        Vector3 dir = new Vector3(xMove, 0, zMove).normalized * playerSpeed;
        myRigid.velocity = dir;
    }


    private void PlayerRotation()
    {
    }


    private void GunRotation()
    {
        if(!isStartGunRotation)
        {
            //offset = new Vector3(ic.InputData["AngleX"], ic.InputData["AngleY"], ic.InputData["AngleZ"]);
            offset = new Vector3(-ic.InputData["AngleY"], -ic.InputData["AngleX"], -ic.InputData["AngleZ"]);
            isStartGunRotation = true;
        }
        else
        {
            Vector3 rotation = new Vector3(-ic.InputData["AngleY"], -ic.InputData["AngleX"], -ic.InputData["AngleZ"]);

            gunPivot.rotation = Quaternion.Euler(rotation - offset);
        }
    }


    public void InitGunRotation()
    {
        Debug.Log("눌림");
        offset = new Vector3(-ic.InputData["AngleY"], -ic.InputData["AngleX"], -ic.InputData["AngleZ"]);
    }


    private void AimGun()
    {
    }


    private void TryShoot()
    {
        if (ic.InputData["Fire"] == 0)
        {
            if (!isShoot)
            {
                gun.Shoot();
                isShoot = true;
            }
        }
        else if(ic.InputData["Fire"] == 1)
            isShoot = false;
    }


    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, gunHandle.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, gunHandle.rotation);


        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, gunHandle.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, gunHandle.rotation);
    }
}
