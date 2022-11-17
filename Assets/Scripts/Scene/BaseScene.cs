using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public EScene sceneType { get; protected set; } = EScene.Unknown;  // 디폴트로 unknown 초기화

    private void Awake()
    {
        Init();
    }


    protected abstract void Init();


    public abstract void StartGame();

    public abstract void Clear();

}
