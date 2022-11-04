using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ƶ��̳��� ��������� ���� string �����͸� ������ �Է� �����ͷ� ��ȯ�Ͽ� ����Ѵ�
/// �� ���� string�� �������� split�� �Ͽ� ������ �ʿ��� �����͸� player ��ũ��Ʈ�� ���⼭ �����ͼ� ����ϰ� �ȴ�.
/// PosX:190 PosY:250 Trigger:1 ... �̷���  " "���� ������ ":"���� �ѹ� �� ������ [0]�� key, [1]�� value�� ���
/// </summary>
public class InputController : MonoBehaviour
{
    private EInputType inputType => GameManager.instance.inputType;

    // ���õ����͸� ������ ������
    public Vector3 gunRotAngle = Vector3.zero;   // ���� ȸ������ (mpu6050_1.getGyroAngle)
    public Vector3 bodyRotAngle = Vector3.zero;  // ���� ȸ������ (mpu6050_2.getGyroAngle)
    public bool isFire;           // �� �ݹ� ����
    public Vector3 moveDir;       // �̵� ����

    // �Ƶ��̳�κ��� ���� ���� ������
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

    // ----------------------------------- dictionary �� Ű�� ----------------------------------------
    private const string Horizontal = "Horizontal";                     // ���̽�ƽ x��
    private const string Vertical = "Vertical";                         // ���̽�ƽ y��
    private const string Fire = "Fire";                                 // ���̽�ƽ z��

    private const string ControlerAngleX = "ControllerAngleX";           // ��Ʈ�ѷ� ���̷� ȸ���� x��
    private const string ControlerAngleY = "ControllerAngleY";           // ��Ʈ�ѷ� ���̷� ȸ���� y��
    private const string ControlerAngleZ = "ControllerAngleZ";           // ��Ʈ�ѷ� ���̷� ȸ���� z��

    private const string BodyAngleX = "BodyAngleX";                     // ��ü ���̷� ȸ���� x��
    // ----------------------------------- dictionary �� Ű�� ----------------------------------------


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
    /// �ʿ��� ������ ����
    /// 1. �� ��Ʈ�ѷ� mpu6050 ���ӵ� ��  3��
    /// 2. �� ��Ʈ�ѷ� mpu6050 ���� ��  3��
    /// 3. ���̽�ƽ��  3�� (�ϴ��� x�� y�� �ݹ�)
    /// 4. �� ��Ʈ�ѷ� mpu6050 ���� �� 3��
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
    /// ����� ������ ����� �� �ְ� ó��
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


    private void ExtractDefaultInput()   // Ű����, ���콺 ��ǲ �ν�
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
