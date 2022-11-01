﻿using System;
using System.Collections;
using System.Collections.Generic;
using ArduinoBluetoothAPI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

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
	private InputController ic;

	public Text Txt_Door;

	public string received_message;
	// 외부에서 접근하여 사용할 수 있습니다.
	// 외부 다른 클래스에서 Manager 함수로 접근하여 Start 펑션에서 다음과 같이 사용하시면 됩니다.
	//===============================================
	// Manager.onDoorOpen.AddListener(OnDoorOpen);
	// Manager.onDoorClose.AddListener(OnDoorClose);
	//===============================================
	public UnityEvent onDoorOpen, onDoorClose;


	public void StartBluetoothCom()
    {
		Btn_Connect.onClick.AddListener(() => {
			deviceName = "VRBT";
			bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
			Debug.Log(bluetoothHelper);
			bluetoothHelper.OnConnected += OnConnected;
			bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
			bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data

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


	void Update () {

		if (Input.GetKeyUp (KeyCode.Alpha0)) {
			if (!isDebugOn) {
				isDebugOn = true;
				DebugHolder.SetActive (true);
			} else {
				isDebugOn = false;
				DebugHolder.SetActive (false);
				myLog = "reset";
			}
		}
	}


	//Asynchronous method to receive messages
	void OnMessageReceived () {
		received_message = bluetoothHelper.Read ();
		Debug.Log(received_message);
		ic.dataString = received_message;


		// ==================================  이런식으로 하면 될듯
		/*if (received_message.Contains ("on")) {
			Txt_Door.text = "Door is close";
			onDoorClose.Invoke();
		}

		if (received_message.Contains ("off")) {
			Txt_Door.text = "Door is open";
			onDoorOpen.Invoke();
		}*/
		// ==================================  ==================================
	}


	void OnConnected () {
		Toggle_isConnected.isOn = true;
		try {
			bluetoothHelper.StartListening ();
			Debug.Log ("Connected");
		} catch (Exception ex) {
			Debug.Log (ex.Message);
		}
	}


	void OnConnectionFailed () {
		Toggle_isConnected.isOn = false;
		Debug.Log ("Connection Failed");
	}


	//Call this function to emulate message receiving from bluetooth while debugging on your PC.
	void OnGUI () {
		if (isDebugOn) {
			if (bluetoothHelper != null)
				bluetoothHelper.DrawGUI ();
			else
				return;

			if (!bluetoothHelper.isConnected ()) {
				Btn_Connect.interactable = true;
				Btn_Disconnect.interactable = false;
				Toggle_isConnected.isOn = false;
			}

			if (bluetoothHelper.isConnected ()) {
				Btn_Connect.interactable = false;
				Btn_Disconnect.interactable = true;
				Toggle_isConnected.isOn = true;
			}

			// Screen Debug
			GUIStyle myStyle = new GUIStyle ();
			myStyle.fontSize = 16;
			myStyle.normal.textColor = Color.blue;
			GUI.Label (new Rect (10, 100, 1080, 1920), myLog, myStyle);
		}
	}

	void OnDestroy () {
		if (bluetoothHelper != null)
			bluetoothHelper.Disconnect ();
	}

	void OnApplicationQuit () {
		if (bluetoothHelper != null)
			bluetoothHelper.Disconnect ();
	}

	// Screen Debug
	bool isDebugOn = true;
	string myLog;
	Queue myLogQueue = new Queue ();
	void OnEnable () {
		Application.logMessageReceived += HandleLog;
	}
	void OnDisable () {
		Application.logMessageReceived -= HandleLog;
	}
	void HandleLog (string logString, string stackTrace, LogType type) {
		myLog = logString;
		string newString = "[" + type + "] : " + myLog + "\n";
		myLogQueue.Enqueue (newString);
		if (type == LogType.Exception) {
			newString = "\n" + stackTrace;
			myLogQueue.Enqueue (newString);
		}
		myLog = string.Empty;
		foreach (string mylog in myLogQueue) {
			myLog += mylog;
		}
	}
}