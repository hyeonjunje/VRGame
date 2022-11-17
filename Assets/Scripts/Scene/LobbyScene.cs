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
        titleText.text = "종합 설계 과제!!";

        potal1.SetActive(true);
    }

    protected override void Init()
    {
        sceneType = EScene.Lobby;

        // 플레이어 설정 초기화
        SceneManagerEx.instance.player.init();

        // 플레이어 위치 설정Warp
        SceneManagerEx.instance.player.Warp(transform.position, Quaternion.identity);

        if(CommunicationManager.isConnected)
            StartGame();
    }


    public void LoadScene(string sceneName)
    {
        SceneManagerEx.instance.LoadScene(sceneName);
    }
}
