using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public EScene sceneType { get; protected set; } = EScene.Unknown;  // ����Ʈ�� unknown �ʱ�ȭ

    private void Awake()
    {
        Init();
    }


    protected abstract void Init();


    public abstract void StartGame();

    public abstract void Clear();

}
