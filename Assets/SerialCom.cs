using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;


public class SerialCom : MonoBehaviour
{
    [SerializeField] private InputController ic;

    SerialPort serialPort = new SerialPort("COM4", 9600);

    private bool startSerial = false;

    private Thread thread;

    public void StartSerialCom()
    {
        // 이미 켜져있다면 return
        if (serialPort.IsOpen)
            return;
        Debug.Log(serialPort);

        serialPort.Open();

        thread = new Thread(DataReveive);
        thread.Start();
    }


    void DataReveive()
    {
        int a = 0;
        while(true)
        {
            ic.dataString = serialPort.ReadLine();
            Debug.Log(ic.dataString);
        }
    }


    private void OnApplicationQuit()
    {
        thread.Abort();
        serialPort.Close();
    }
}
