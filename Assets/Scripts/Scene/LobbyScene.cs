using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : BaseScene
{
    [SerializeField] private Text titleText;
    [SerializeField] private GameObject potal1;


    public override void Clear()
    {

    }

    public override void StartGame()
    {
        titleText.text = "���� ���� ����!!";

        potal1.SetActive(true);
    }

    protected override void Init()
    {
        sceneType = EScene.Lobby;

        // �÷��̾� ���� �ʱ�ȭ
        SceneManagerEx.instance.player.init();

        // �÷��̾� ��ġ ����Warp
        SceneManagerEx.instance.player.Warp(transform.position, Quaternion.identity);

        if(CommunicationManager.isConnected)
            StartGame();
    }


    public void LoadScene(string sceneName)
    {
        SceneManagerEx.instance.LoadScene(sceneName);
    }
}
