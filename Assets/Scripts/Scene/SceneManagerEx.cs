using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;


public enum EScene
{
    Unknown,  // 디폴트
    Lobby,    // 로비 씬
    Game,     // 게임 씬
}

public class SceneManagerEx : MonoBehaviour
{
    public static SceneManagerEx instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;


        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public Player player;

    [SerializeField] private Text loadText;
    private string[] loadTexts = { "로딩중.", "로딩중..", "로딩중..." };
    private Coroutine coLoding;

    private bool isLoad = false;


    public void LoadScene(string sceneName)
    {
        if(!isLoad)
        {
            isLoad = true;
            CurrentScene.Clear();
            LoadSceneAsync(sceneName).Forget();
        }
    }

    private async UniTaskVoid LoadSceneAsync(string sceneName)
    {
        Debug.Log("안녕 전 " + System.DateTime.Now);

        coLoding = StartCoroutine(CoLoding());

        await SceneManager.LoadSceneAsync(sceneName);
        Debug.Log("안녕 gn " + System.DateTime.Now);

        StopCoroutine(coLoding);
        loadText.gameObject.SetActive(false);

        isLoad = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            player.transform.position = Vector3.zero;
        }
    }


    IEnumerator CoLoding()
    {
        loadText.gameObject.SetActive(true);
        int index = 0;
        float timer = 0f;
        while(true)
        {
            timer += Time.deltaTime;
            if(timer >= 0.5)
            {
                timer = 0f;
                if (index >= loadTexts.Length)
                    index = 1;
                loadText.text = loadTexts[index++];
            }
            yield return null;
        }
    }

    private void Start()
    {
        CommunicationManager.instance.StartCom();
    }
}
