using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [Header("UI")]
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private Text GameInfoText;
    [SerializeField] private Text clearTimeText;
    [SerializeField] private Text killCountText;
    [SerializeField] private GameObject reStartText;

    private int killCount = 0;
    private System.DateTime startTime;

    private bool readyToRestart = false;

    private void Start()
    {
        EventManager.enemyDieEvent += EnemyDie;
        EventManager.shootEvent += Restart;
    }


    public void StartGame()
    {
        killCount = 0;
        startTime = System.DateTime.Now;
    }


    private void EnemyDie()
    {
        killCount++;
    }


    private void Restart()
    {
        if(readyToRestart)
        {
            readyToRestart = false;
            SceneManagerEx.instance.LoadScene("Lobby");
        }
    }


    public void GameOver()
    {
        gameEndPanel.SetActive(true);
        GameInfoText.text = "Game Over";
        GameInfoText.color = Color.red;
        double totalTime = (System.DateTime.Now - startTime).TotalSeconds;
        clearTimeText.text = "클리어 시간 : " + (int)(totalTime / 60) + "분 " + (int)(totalTime % 60) + "초";
        killCountText.text = "좀비를 죽인 횟수 : " + killCount;

        StartCoroutine(CoReadyToReStart());
    }


    public void GameClear()
    {
        gameEndPanel.SetActive(true);
        GameInfoText.text = "Game Clear";
        GameInfoText.color = Color.yellow;
        clearTimeText.text = "클리어 시간 : " + (System.DateTime.Now - startTime).TotalSeconds;
        killCountText.text = "좀비를 죽인 횟수 : " + killCount;

        StartCoroutine(CoReadyToReStart());
    }


    IEnumerator CoReadyToReStart()
    {
        reStartText.SetActive(false);
        readyToRestart = false;

        yield return new WaitForSeconds(2f);

        readyToRestart = true;
        reStartText.SetActive(true);
    }
}
