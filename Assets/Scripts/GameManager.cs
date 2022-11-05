using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    private void Start()
    {
        serialCom.SetActive(false);
        bluetoothCom.SetActive(false);
        switch (inputType)
        {
            case EInputType.Serial:
                serialCom.SetActive(true);
                serialCom.GetComponent<SerialCom>().StartSerialCom();  // ��Ž���
                break;
            case EInputType.Bluetooth:
                bluetoothCom.SetActive(true);
                bluetoothCom.GetComponent<Manager>().StartBluetoothCom();  // ��� ���� ��ư Ȱ��ȭ
                break;
            default:
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
