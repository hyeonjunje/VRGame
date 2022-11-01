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

    public Vector3 gunRotAngle = Vector3.zero;   // ���� ȸ������ (mpu6050.getGyroAngle)
    public bool isFire;           // �� �ݹ� ����
    public Vector3 moveDir;       // �̵� ����

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
    /// 0. Horizontal : ���̽�ƽ x��
    /// 1. Vertical : ���̽�ƽ y��
    /// 2. Fire : ���̽�ƽ z��
    /// 3. AngleX : ���̷� ȸ���� x��
    /// 4. AngleY : ���̷� ȸ���� y��
    /// 5. AngleZ : ���̷� ȸ���� z��
    /// </summary>
    private Dictionary<string, float> InputData = new Dictionary<string, float>();


    /// <summary>
    /// �ʿ��� ������ ����
    /// 1. �� ��Ʈ�ѷ� mpu6050 ���ӵ� ��  3��
    /// 2. �� ��Ʈ�ѷ� mpu6050 ���ӵ� ��  3��
    /// 3. ���̽�ƽ��  3�� (�ϴ��� x�� y�� �ݹ�)
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
    /// ����� ������ ����� �� �ְ� ó��
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
