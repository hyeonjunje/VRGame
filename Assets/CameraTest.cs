using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    [SerializeField] private Transform cam;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(cam.localEulerAngles.x,
            -cam.localEulerAngles.y, cam.localEulerAngles.z));
    }
}
