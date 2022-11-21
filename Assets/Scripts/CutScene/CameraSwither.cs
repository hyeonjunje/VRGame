using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class CameraSwither
{
    private static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera activeCamera = null;


    public static void SwitchCamera(CinemachineVirtualCamera camera)
    {
        Debug.Log(camera + " ·Î ¹Ù²ñ");
        camera.Priority = 10;
        activeCamera = camera;

        foreach(CinemachineVirtualCamera c in cameras)
        {
            if (c != camera && c.Priority != 0)
                c.Priority = 0;
        }
    }


    public static void Register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }


    public static void UnRegister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
    }
}
