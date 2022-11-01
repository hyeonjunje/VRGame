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

    public Vector3 gunRotAngle = Vector3.zero;   // 총의 회전각도 (mpu6050.getGyroAngle)
    public bool isFire;           // 총 격발 여부
    public Vector3 moveDir;       // 이동 방향

    private Vector3 offsetGunRotAngle;
    private bool isStartGunRotation = false;

    private string _dataString = null;
    public string dataString
    {
        get { return _dataString; }
        set
        {
            _dataString = value;
            if(_dataString != null)
            {
                ExtractData();
                ProcessingInfo();
            }
        }
    }

    /// <summary>
    /// 0. Horizontal : 조이스틱 x축
    /// 1. Vertical : 조이스틱 y축
    /// 2. Fire : 조이스틱 z축
    /// 3. AngleX : 자이로 회전값 x축
    /// 4. AngleY : 자이로 회전값 y축
    /// 5. AngleZ : 자이로 회전값 z축
    /// </summary>
    private Dictionary<string, float> InputData = new Dictionary<string, float>();


    /// <summary>
    /// 필요한 데이터 검출
    /// 1. 총 컨트롤러 mpu6050 가속도 값  3개
    /// 2. 총 컨트롤러 mpu6050 각속도 값  3개
    /// 3. 조이스틱값  3개 (일단은 x축 y축 격발)
    /// </summary>
    private void ExtractData()
    {
        string[] data = dataString.Split(' ');
        foreach(string iter in data)
        {
            string[] d = iter.Split(':');
            InputData[d[0]] = float.Parse(d[1]);
        }
    }


    /// <summary>
    /// 검출된 정보를 사용할 수 있게 처리
    /// </summary>
    private void ProcessingInfo()
    {
        isFire = InputData["Fire"] == 0 ? true : false;

        float xMove = (InputData["Horizontal"] - 518) / 518;
        float zMove = (InputData["Vertical"] - 518) / 518;

        xMove = Mathf.Abs(xMove - 0) < 0.1f ? 0f : xMove;
        zMove = Mathf.Abs(zMove - 0) < 0.1f ? 0f : zMove;

        moveDir = new Vector3(xMove, 0, zMove).normalized;


        if (!isStartGunRotation)
        {
            offsetGunRotAngle = new Vector3(-InputData["AngleY"], -InputData["AngleX"], -InputData["AngleZ"]);
            gunRotAngle = Vector3.zero;
            isStartGunRotation = true;
        }
        else
            gunRotAngle = new Vector3(-InputData["AngleY"], -InputData["AngleX"], -InputData["AngleZ"]) - offsetGunRotAngle;
    }


    public void InitGunRotation()
    {
        offsetGunRotAngle = new Vector3(-InputData["AngleY"], -InputData["AngleX"], -InputData["AngleZ"]);
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
