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

    [Header("입력 방식")]
    [SerializeField] private EInputType _inputType;
    public EInputType inputType { get { return _inputType; } }

    [Header("통신 오브젝트")]
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
                serialCom.GetComponent<SerialCom>().StartSerialCom();  // 통신시작
                break;
            case EInputType.Bluetooth:
                bluetoothCom.SetActive(true);
                bluetoothCom.GetComponent<Manager>().StartBluetoothCom();  // 통신 시작 버튼 활성화
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
                bluetoothCom.GetComponent<Manager>().WriteCom();  // 통신 시작 버튼 활성화
                break;
            default:
                break;
        }
    }
}
