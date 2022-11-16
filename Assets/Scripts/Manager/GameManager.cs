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

    //[SerializeField] private Light mainLight;

    /*[Header("UI")]
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private Text GameInfoText;
    [SerializeField] private Text clearTimeText;
    [SerializeField] private Text killCountText;
    [SerializeField] private GameObject reStartText;

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject startText;*/

    public delegate void GameStartEvent();
    public GameStartEvent gameStartEvent = null;

    public delegate void GameEndEvent();
    public GameEndEvent gameEndEvent = null;

    private bool readyToReStart = false;
    private bool readyToStart = false;

    public int killCount = 0;

    public bool isInGame = false;

    private System.DateTime startTime;


    private void Start()
    {
        //lobbyPanel.SetActive(true);
        //StartCoroutine(CoReadyToStart());
    }


/*    public void StartGame()
    {
        if(!isInGame && readyToStart)
        {
            readyToStart = false;

            lobbyPanel.SetActive(false);

            startTime = System.DateTime.Now;
            isInGame = true;

            mainLight.intensity = 0f;

            if (gameStartEvent != null)
                gameStartEvent();
        }
    }


    public void ClearGame()
    {
        isInGame = false;

        if (gameEndEvent != null)
            gameEndEvent.Invoke();

        gameEndPanel.SetActive(true);
        GameInfoText.text = "Game Clear";
        GameInfoText.color = Color.yellow;

        clearTimeText.text = "클리어 시간 : " + (System.DateTime.Now - startTime).TotalSeconds;
        killCountText.text = "좀비를 죽인 횟수 : " + killCount;

        StartCoroutine(CoReadyToReStart());
    }


    public void GameOver()
    {
        isInGame = false;

        if (gameEndEvent != null)
            gameEndEvent.Invoke();

        gameEndPanel.SetActive(true);
        GameInfoText.text = "Game Over";
        GameInfoText.color = Color.red;

        clearTimeText.text = "클리어 시간 : " + (System.DateTime.Now - startTime).TotalSeconds;
        killCountText.text = "좀비를 죽인 횟수 : " + killCount;

        StartCoroutine(CoReadyToReStart());
    }


    public void ReStart()
    {
        if(readyToReStart)
            SceneManager.LoadScene(0);
    }


    IEnumerator CoReadyToReStart()
    {
        yield return new WaitForSeconds(3f);
        readyToReStart = true;

        reStartText.SetActive(true);
    }


    IEnumerator CoReadyToStart()
    {
        yield return new WaitForSeconds(3f);
        readyToStart = true;

        startText.SetActive(true);
    }*/
}
