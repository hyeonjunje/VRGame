using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSlot : MonoBehaviour
{
    [SerializeField] private float textSpeed;   // ���ڰ� �����̴� �ӵ�
    [SerializeField] private float initPos;     // �ʱ�ȭ�Ǵ� ��ġ
    [SerializeField] private float spawnPos;    // �ٽ� �����Ǵ� ��ġ

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
        Debug.Log(this.name + " " + "�̰� ����" + BGMManager.selectedSlot);
    }


    public void SetColor(Color color)
    {
        bgmText.color = color;
    }
}
