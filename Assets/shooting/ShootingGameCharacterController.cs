using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGameCharacterController : MonoBehaviour
{
    [SerializeField]
    private Transform gunHandle;

    [SerializeField]
    private Transform gunPivot;

    private Animator animator;
    private Rigidbody myRigid;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
    }

    public float horizontal;
    public float vertical;

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    private void Update()
    {
        //gunPivot.Translate(new Vector3(horizontal, vertical, gunPivot.position.z), Space.Self);
        AimGun();

        CharacterRotation();
    }

    [SerializeField] private float lookSensitivity;
    [SerializeField] private float currentGunRotationX;
    [SerializeField] private float gunRotationLimit = 70;
    [SerializeField] private float currentGunRotationY;

    [SerializeField] private float rotationSensitivity;
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxis("Horizontal");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * rotationSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // ÄõÅÍ´Ï¾ð * ÄõÅÍ´Ï¾ð
    }

    private void AimGun()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        float _yRotation = Input.GetAxisRaw("Mouse X");
        float _cameraRotationY = _yRotation * lookSensitivity;

        currentGunRotationX -= _cameraRotationX;
        currentGunRotationX = Mathf.Clamp(currentGunRotationX, -gunRotationLimit, gunRotationLimit);

        currentGunRotationY += _cameraRotationY;
        currentGunRotationY = Mathf.Clamp(currentGunRotationY, -gunRotationLimit, gunRotationLimit);

        gunPivot.localEulerAngles = new Vector3(currentGunRotationX, currentGunRotationY, 0f);
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
