using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class Test : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("COM4", 9600);

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
