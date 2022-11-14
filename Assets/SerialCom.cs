using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using Cysharp.Threading.Tasks;

public class SerialCom : MonoBehaviour
{
    [SerializeField] private InputController ic;

    SerialPort serialPort = new SerialPort("COM4", 9600);

    private bool startSerial = false;


    public void StartSerialCom()
    {
        // 이미 켜져있다면 return
        if (serialPort.IsOpen)
            return;
        Debug.Log(serialPort);

        serialPort.Open();

        //DataReceive();
        DataReceive().Forget();
        Debug.Log("=====================================================================");
    }


    /*private async void DataReceive()
    {
        await Task.Run(() =>
        {
            Debug.Log((Thread.CurrentThread == mainThread));
            for (int i = 0; i < 10000; i++)
            {
                ic.dataString = serialPort.ReadLine();
                Debug.Log(ic.dataString);
            }
        });
    }*/


    private async UniTaskVoid DataReceive()
    {
        await UniTask.RunOnThreadPool(() =>
        {
            while(true)
            {
                ic.dataString = serialPort.ReadLine();
                //Debug.Log(ic.dataString);
            }
        });
    }


    private void OnApplicationQuit()
    {
        serialPort.Close();
    }


    public void WriteCom()
    {
        serialPort.Write("Calibration");
    }
}
