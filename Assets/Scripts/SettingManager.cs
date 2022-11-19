using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private Camera mainCamR;
    [SerializeField] private Camera mainCamL;   // 이게 본체

    [SerializeField] private GameObject inputCanvas;  // 입력 UI

    // NonArduino 버전 : 카메라 하나, 적절한 입력시스템

    public static bool isArduino = true;

    private void Start()
    {
        ChangeMode();
    }


    public void ChangeMode()
    {
        if (isArduino)
            SetNonArduinoVer();
        else
            SetArduinoVer();
    }


    public void SetNonArduinoVer()
    {
        isArduino = false;

        mainCamR.gameObject.SetActive(false);
        mainCamL.rect = new Rect(0, 0, 1f, 1f);

        mainCamL.transform.localPosition = new Vector3(0f, 0.076f, -0.129f);
        mainCamL.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        inputCanvas.SetActive(true);
    }


    public void SetArduinoVer()
    {
        isArduino = true;

        mainCamR.gameObject.SetActive(true);
        mainCamR.rect = new Rect(0.5f, 0, 0.5f, 1f);
        mainCamL.rect = new Rect(0, 0, 0.5f, 1f);

        mainCamR.transform.localPosition = new Vector3(0.03f, 0.076f, -0.129f);
        mainCamR.transform.localRotation = Quaternion.Euler(new Vector3(0, 8.44f, 0));

        mainCamL.transform.localPosition = new Vector3(-0.03f, 0.076f, -0.129f);
        mainCamL.transform.localRotation = Quaternion.Euler(new Vector3(0, -8.44f, 0));

        inputCanvas.SetActive(false);
    }
}
