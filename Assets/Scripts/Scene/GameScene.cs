using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField] private Light mainLight;
    [SerializeField] private ZombieSpawner zombieSpawner;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RectTransform canvas;
    
    public override void Clear()
    {
        canvas.parent = null;
    }

    public override void StartGame()
    {
        // 밝기 설정
        mainLight.intensity = 0;

        // 좀비 생성
        zombieSpawner.SpawnZombie();

        // 플레이어 설정 초기화
        SceneManagerEx.instance.player.init();

        // 플레이어 위치 설정Warp
        SceneManagerEx.instance.player.Warp(transform.position, Quaternion.identity);

        // 인게임 시작 설정
        gameManager.StartGame();

        canvas.SetParent(SceneManagerEx.instance.player.pv);
        canvas.localPosition = new Vector3(0, 0, 1f);
        canvas.localScale = Vector3.one * 0.00106916f;
    }

    protected override void Init()
    {
        sceneType = EScene.Game;

        StartGame();
    }
}
