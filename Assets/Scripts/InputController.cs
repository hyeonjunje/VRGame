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
    /// 0. Horizontal : 조이스틱 x축
    /// 1. Vertical : 조이스틱 y축
    /// 2. Fire : 조이스틱 z축
    /// 3. AngleX : 자이로 회전값 x축
    /// 4. AngleY : 자이로 회전값 y축
    /// 5. AngleZ : 자이로 회전값 z축
    /// </summary>
    public Dictionary<string, float> InputData = new Dictionary<string, float>();


    /// <summary>
    /// 필요한 데이터
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
}
