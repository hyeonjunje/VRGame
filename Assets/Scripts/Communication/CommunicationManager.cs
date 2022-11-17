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

    [Header("�Է� ���")]
    [SerializeField] private EInputType _inputType;
    public EInputType inputType { get { return _inputType; } }

    [Header("��� ������Ʈ")]
    [SerializeField] private GameObject serialCom;
    [SerializeField] private GameObject bluetoothCom;

    public void StartCom()
    {
        switch (inputType)
        {
            case EInputType.Serial:
                serialCom.GetComponent<SerialCom>().StartSerialCom();  // ��Ž���
                isConnected = true;
                break;
            case EInputType.Bluetooth:
                bluetoothCom.GetComponent<BluetoothCom>().StartBluetoothCom();  // ��� ���� ��ư Ȱ��ȭ
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
                bluetoothCom.GetComponent<Manager>().WriteCom();  // ��� ���� ��ư Ȱ��ȭ
                break;
            default:
                break;
        }
    }
}
