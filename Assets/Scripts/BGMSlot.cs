using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlot : MonoBehaviour
{
    [SerializeField] private float textSpeed;   // 글자가 움직이는 속도
    [SerializeField] private float initPos;     // 초기화되는 위치
    [SerializeField] private float spawnPos;    // 다시 스폰되는 위치

    [SerializeField] private RectTransform brmRect;
    private Text bgmText;

    [HideInInspector] public AudioClip audioClip = null;


    private void Awake()
    {
        bgmText = brmRect.GetComponent<Text>();
    }

    void Update()
    {
        brmRect.localPosition += Vector3.left * textSpeed * Time.deltaTime;

        if (brmRect.localPosition.x < initPos)
        {
            brmRect.localPosition = new Vector3(spawnPos - 960, 0f, 0f);
        }
    }


    public void SetSlot(AudioClip clip)
    {
        audioClip = clip;
        bgmText.text = clip.name;
        brmRect.sizeDelta = new Vector2(brmRect.GetComponent<Text>().preferredWidth, brmRect.sizeDelta.y);
        initPos = -brmRect.sizeDelta.x - 960;
    }


    public void InteractSlot()
    {
        BGMManager.selectedSlot?.SetColor(Color.white);
        BGMManager.selectedSlot = this;
        SoundManager.instance.PlayBGM(audioClip);
        SetColor(Color.red);
        Debug.Log(this.name + " " + "이거 실행" + BGMManager.selectedSlot);
    }


    public void SetColor(Color color)
    {
        bgmText.color = color;
    }
}
