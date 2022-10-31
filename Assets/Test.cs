using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class Test : MonoBehaviour
{
    [SerializeField] private InputController ic;

    SerialPort serialPort = new SerialPort("COM4", 9600);

    private bool startSerial = false;

    private void Start()
    {
        Debug.Log(serialPort);
        serialPort.Open();
    }

    private void Update()
    {
        try
        {
            if(serialPort.IsOpen)
            {
                Debug.Log(serialPort.ReadLine());

                ic.dataString = serialPort.ReadLine();
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnApplicationQuit()
    {
        serialPort.Close();
    }
}
