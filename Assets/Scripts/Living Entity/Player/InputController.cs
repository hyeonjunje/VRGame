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
    private EInputType inputType => CommunicationManager.instance.inputType;

    // ���õ����͸� ������ ������
    public Vector3 gunRotAngle = Vector3.zero;   // ���� ȸ������ (mpu6050_1.getGyroAngle)
    public Vector3 bodyRotAngle = Vector3.zero;  // ���� ȸ������ (mpu6050_2.getGyroAngle)
    public bool isFire;           // �� �ݹ� ����
    public Vector3 moveDir;       // �̵� ����
    public bool trySetting;       // ���� ����(�ʱ�ȭ)

    public Vector3 headRotAngle = Vector3.zero;  // �Ӹ� ����(���̷� ������ ������� ���� ���� �� ���� ���)

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
                try
                {
                    ExtractData();
                    ProcessingInfo();
                }
                catch(System.Exception e)
                {
                    Debug.Log(e);
                }
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
    private const string Setting = "Setting";                           // ���̽�ƽ z��

    private const string ControlerAngleX = "ControllerAngleX";           // ��Ʈ�ѷ� ���̷� ȸ���� x��
    private const string ControlerAngleY = "ControllerAngleY";           // ��Ʈ�ѷ� ���̷� ȸ���� y��
    private const string ControlerAngleZ = "ControllerAngleZ";           // ��Ʈ�ѷ� ���̷� ȸ���� z��

    private const string BodyAngleX = "BodyAngleX";                     // ��ü ���̷� ȸ���� x��

    private const string Fire = "Fire";                                 // ��ư Ŭ��
    // ----------------------------------- dictionary �� Ű�� ----------------------------------------


    private Dictionary<string, float> InputData = new Dictionary<string, float>();  // dictionary


    private void Start()
    {
        InputData[Horizontal] = 0f;
        InputData[Vertical] = 0f;
        InputData[Setting] = 0f;

        InputData[ControlerAngleX] = 0f;
        InputData[ControlerAngleY] = 0f;
        InputData[ControlerAngleZ] = 0f;

        InputData[BodyAngleX] = 0f;

        InputData[Fire] = 0f;
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
        try
        {
            string[] data = dataString.Split(' ');

            foreach(string iter in data)
            {
                string[] d = iter.Split(':');
                InputData[d[0]] = float.Parse(d[1]);
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }


    /// <summary>
    /// ����� ������ ����� �� �ְ� ó��
    /// </summary>
    private void ProcessingInfo()
    {
        if (!InputData.ContainsKey(Fire))
            return;

        trySetting = InputData[Setting] == 0 ? true : false;

        float xMove = (InputData[Horizontal] - 518) / 518;
        float zMove = (InputData[Vertical] - 518) / 518;

        xMove = Mathf.Abs(xMove - 0) < 0.1f ? 0f : xMove;
        zMove = Mathf.Abs(zMove - 0) < 0.1f ? 0f : zMove;

        moveDir = new Vector3(xMove, 0, zMove).normalized;

        isFire = InputData[Fire] == 1 ? true : false;

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

        if (Input.GetKey(KeyCode.E))
            bodyRotAngle += new Vector3(0f, 1f, 0f);
        else if(Input.GetKey(KeyCode.Q))
            bodyRotAngle += new Vector3(0f, -1f, 0f);
    }


    private void NonArduinoVer()   // ����� ����� �Ƶ��̳밡 ��� �÷��̵Ǵ� ���
    {
        moveDir = new Vector3(NonArduinoInput.instance.moveJoystick.Direction.x, 0, NonArduinoInput.instance.moveJoystick.Direction.y);



        if (NonArduinoInput.isFire)
            isFire = true;
        else
            isFire = false;


        bodyRotAngle += new Vector3(0f, NonArduinoInput.instance.screenSlider.dir.x, 0f);
        headRotAngle += new Vector3(-NonArduinoInput.instance.screenSlider.dir.y, NonArduinoInput.instance.screenSlider.dir.x, 0f);

        gunRotAngle += new Vector3(-NonArduinoInput.instance.screenSlider.dir.y, NonArduinoInput.instance.screenSlider.dir.x, 0f);
    }



    private void Update()
    {
        if(inputType == EInputType.DefaultInput)
        {
            ExtractDefaultInput();
        }
        else if(inputType == EInputType.NonArudino)
        {
            NonArduinoVer();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            Cursor.visible = Cursor.visible == false ? true : false;
    }
}
