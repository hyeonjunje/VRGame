using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CommunicationManager : MonoBehaviour
{
    public static CommunicationManager instance;
    public static bool isConnected = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [Header("입력 방식")]
    [SerializeField] private EInputType _inputType;
    public EInputType inputType { get { return _inputType; } }

    [Header("통신 오브젝트")]
    [SerializeField] private GameObject serialCom;
    [SerializeField] private GameObject bluetoothCom;

    public void StartCom()
    {
        switch (inputType)
        {
            case EInputType.Serial:
                serialCom.GetComponent<SerialCom>().StartSerialCom();  // 통신시작
                isConnected = true;
                break;
            case EInputType.Bluetooth:
                bluetoothCom.GetComponent<BluetoothCom>().StartBluetoothCom();  // 통신 시작 버튼 활성화
                break;
            default:
                SceneManagerEx.instance.CurrentScene.StartGame();
                isConnected = true;
                break;
        }
    }


    public void Calibration()
    {
        switch (inputType)
        {
            case EInputType.Serial:
                serialCom.GetComponent<SerialCom>().WriteCom();
                break;
            case EInputType.Bluetooth:
                bluetoothCom.GetComponent<Manager>().WriteCom();  // 통신 시작 버튼 활성화
                break;
            default:
                break;
        }
    }
}
