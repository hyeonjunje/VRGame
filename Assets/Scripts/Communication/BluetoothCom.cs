using System;
using System.Collections;
using System.Collections.Generic;
using ArduinoBluetoothAPI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class BluetoothCom : MonoBehaviour
{

	// Use this for initialization
	BluetoothHelper bluetoothHelper;
	string deviceName;
	[SerializeField]
	Toggle Toggle_isDevicePaired;
	[SerializeField]
	Toggle Toggle_isConnected;
	[SerializeField]
	GameObject DebugHolder;
	[SerializeField]
	Button Btn_Connect;
	[SerializeField]
	Button Btn_Disconnect;

	[SerializeField]
	Button ActiveCanvas;

	[SerializeField]
	private InputController ic;

	public string received_message;

	private void Start()
	{
		ActiveCanvas.onClick.AddListener(() =>
		{
			if (DebugHolder.activeSelf)
				DebugHolder.SetActive(false);
			else
				DebugHolder.SetActive(true);
		});
	}

	public void StartBluetoothCom()
	{
		Btn_Connect.onClick.AddListener(() => {
			deviceName = "VRBT";
			bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
			Debug.Log(bluetoothHelper);
			bluetoothHelper.OnConnected += OnConnected;
			bluetoothHelper.OnConnectionFailed += OnConnectionFailed;

			//bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data
			DataReceive().Forget();


			bluetoothHelper.setTerminatorBasedStream("\n");


			if (bluetoothHelper.isDevicePaired())
				Toggle_isDevicePaired.isOn = true;
			else
				Toggle_isDevicePaired.isOn = false;

			if (bluetoothHelper.ScanNearbyDevices())
			{
				Debug.Log("근처 블루투스 있음");

			}
			else
				Debug.Log("없음");

			if (bluetoothHelper.isDevicePaired())
			{
				Debug.Log("try to connect");
				bluetoothHelper.Connect(); // tries to connect
			}
			else
			{
				Debug.Log("not DevicePaired");
			}
		});
		Btn_Disconnect.onClick.AddListener(() => {
			bluetoothHelper.Disconnect();
			Debug.Log("try to Disconnect");
		});
	}

	private async UniTaskVoid DataReceive()
	{
		await UniTask.RunOnThreadPool(() =>
		{
			while (true)
			{
				ic.dataString = bluetoothHelper.Read();
				Debug.Log(ic.dataString);
			}
		});
	}


	/*    private void DataReveive()
		{
			while (true)
			{
				ic.dataString = bluetoothHelper.Read();
				Debug.Log(ic.dataString);
			}
		}*/


	public void WriteCom()
	{
		bluetoothHelper.SendData("Calibration");
	}


	/*	void OnMessageReceived () {
			received_message = bluetoothHelper.Read ();
			ic.dataString = received_message;
		}*/


	void OnConnected()
	{
		Toggle_isConnected.isOn = true;
		try
		{
			bluetoothHelper.StartListening();
			Debug.Log("Connected");
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}


	void OnConnectionFailed()
	{
		Toggle_isConnected.isOn = false;
		Debug.Log("Connection Failed");
	}


	void OnDestroy()
	{
		if (bluetoothHelper != null)
			bluetoothHelper.Disconnect();
	}


	void OnApplicationQuit()
	{
		if (bluetoothHelper != null)
			bluetoothHelper.Disconnect();
	}
}