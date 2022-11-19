using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInputType
{
    Serial,               // 시리얼 통신
    Bluetooth,            // 블루투스 통신
    DefaultInput,         // 기본 입출력 장치(PC)
    NonArudino,           // 아두이노 없이 실행(모바일)
}
