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
    private string _dataString = null;
    public string dataString
    {
        get { return _dataString; }
        set
        {
            _dataString = value;
            if(_dataString != null)
                ExtractData();
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
    public Dictionary<string, float> InputData = new Dictionary<string, float>();


    /// <summary>
    /// �ʿ��� ������
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
}
