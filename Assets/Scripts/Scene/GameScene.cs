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
        // ��� ����
        mainLight.intensity = 0;

        // ���� ����
        zombieSpawner.SpawnZombie();

        // �÷��̾� ���� �ʱ�ȭ
        SceneManagerEx.instance.player.init();

        // �÷��̾� ��ġ ����Warp
        SceneManagerEx.instance.player.Warp(transform.position, Quaternion.identity);

        // �ΰ��� ���� ����
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
