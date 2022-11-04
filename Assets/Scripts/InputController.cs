using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아두이노의 블루투스로 받은 string 데이터를 게임의 입력 데이터로 변환하여 사용한다
/// 한 줄의 string을 공백으로 split을 하여 각각의 필요한 데이터를 player 스크립트가 여기서 가져와서 사용하게 된다.
/// PosX:190 PosY:250 Trigger:1 ... 이런식  " "으로 나눈걸 ":"으로 한번 더 나눠서 [0]을 key, [1]을 value로 사용
/// </summary>
public class InputController : MonoBehaviour
{
    private EInputType inputType => GameManager.instance.inputType;

    // 원시데이터를 가공한 데이터
    public Vector3 gunRotAngle = Vector3.zero;   // 총의 회전각도 (mpu6050_1.getGyroAngle)
    public Vector3 bodyRotAngle = Vector3.zero;  // 몸의 회전각도 (mpu6050_2.getGyroAngle)
    public bool isFire;           // 총 격발 여부
    public Vector3 moveDir;       // 이동 방향

    // 아두이노로부터 받을 원시 데이터
    private string _dataString = null;
    public string dataString
    {
        get { return _dataString; }
        set
        {
            _dataString = value;

            if(_dataString.Contains("Fire"))
            {
                ExtractData();
                ProcessingInfo();
            }
        }
    }

    private Vector3 offsetGunRotAngle;
    private bool isStartGunRotation = false;
    private Vector3 offsetBodyRotAngle;
    private bool isStartBodyRotation = false;

    // ----------------------------------- dictionary 의 키값 ----------------------------------------
    private const string Horizontal = "Horizontal";                     // 조이스틱 x축
    private const string Vertical = "Vertical";                         // 조이스틱 y축
    private const string Fire = "Fire";                                 // 조이스틱 z축

    private const string ControlerAngleX = "ControllerAngleX";           // 컨트롤러 자이로 회전값 x축
    private const string ControlerAngleY = "ControllerAngleY";           // 컨트롤러 자이로 회전값 y축
    private const string ControlerAngleZ = "ControllerAngleZ";           // 컨트롤러 자이로 회전값 z축

    private const string BodyAngleX = "BodyAngleX";                     // 상체 자이로 회전값 x축
    // ----------------------------------- dictionary 의 키값 ----------------------------------------


    private Dictionary<string, float> InputData = new Dictionary<string, float>();  // dictionary


    private void Start()
    {
        InputData[Horizontal] = 0f;
        InputData[Vertical] = 0f;
        InputData[Fire] = 0f;

        InputData[ControlerAngleX] = 0f;
        InputData[ControlerAngleY] = 0f;
        InputData[ControlerAngleZ] = 0f;

        InputData[BodyAngleX] = 0f;
    }

    /// <summary>
    /// 필요한 데이터 검출
    /// 1. 총 컨트롤러 mpu6050 가속도 값  3개
    /// 2. 총 컨트롤러 mpu6050 각도 값  3개
    /// 3. 조이스틱값  3개 (일단은 x축 y축 격발)
    /// 4. 몸 컨트롤러 mpu6050 각도 값 3개
    /// </summary>
    private void ExtractData()
    {
        string[] data = dataString.Split(' ');

        InputData[Horizontal] = float.Parse(data[0].Split(':')[1]);
        InputData[Vertical] = float.Parse(data[1].Split(':')[1]);
        InputData[Fire] = float.Parse(data[2].Split(':')[1]);

        InputData[ControlerAngleX] = float.Parse(data[3].Split(':')[1]);
        InputData[ControlerAngleY] = float.Parse(data[4].Split(':')[1]);
        InputData[ControlerAngleZ] = float.Parse(data[5].Split(':')[1]);

        InputData[BodyAngleX] = float.Parse(data[6].Split(':')[1]);

        
/*        foreach(string iter in data)
        {
            string[] d = iter.Split(':');
            InputData[d[0]] = float.Parse(d[1]);
        }*/
    }


    /// <summary>
    /// 검출된 정보를 사용할 수 있게 처리
    /// </summary>
    private void ProcessingInfo()
    {
        if (!InputData.ContainsKey(Fire))
            return;

        isFire = InputData[Fire] == 0 ? true : false;

        float xMove = (InputData[Horizontal] - 518) / 518;
        float zMove = (InputData[Vertical] - 518) / 518;

        xMove = Mathf.Abs(xMove - 0) < 0.1f ? 0f : xMove;
        zMove = Mathf.Abs(zMove - 0) < 0.1f ? 0f : zMove;

        moveDir = new Vector3(xMove, 0, zMove).normalized;


        if (!isStartGunRotation)
        {
            offsetGunRotAngle = new Vector3(-InputData[ControlerAngleY], -InputData[ControlerAngleX], -InputData[ControlerAngleZ]);
            gunRotAngle = Vector3.zero;
            isStartGunRotation = true;
        }
        else
            gunRotAngle = new Vector3(-InputData[ControlerAngleY], -InputData[ControlerAngleX], -InputData[ControlerAngleZ])
                - offsetGunRotAngle;


        if (!isStartBodyRotation)
        {
            offsetBodyRotAngle = new Vector3(0f, -InputData[BodyAngleX], 0f);
            bodyRotAngle = Vector3.zero;
            isStartBodyRotation = true;
        }
        else
            bodyRotAngle = new Vector3(0f, -InputData[BodyAngleX], 0f)
                - offsetBodyRotAngle;
    }


    public void InitRotation()
    {
        offsetGunRotAngle = new Vector3(-InputData[ControlerAngleY], -InputData[ControlerAngleX], -InputData[ControlerAngleZ]);
        offsetBodyRotAngle = new Vector3(0f, -InputData[BodyAngleX], 0f);
    }


    private void ExtractDefaultInput()   // 키보드, 마우스 인풋 인식
    {  
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (Input.GetMouseButtonDown(0))
            isFire = true;
        else if (Input.GetMouseButtonUp(0))
            isFire = false;

        gunRotAngle += new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0f);
    }


    private void Update()
    {
        if(inputType == EInputType.DefaultInput)
        {
            ExtractDefaultInput();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            Cursor.visible = Cursor.visible == false ? true : false;
    }
}
