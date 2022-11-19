using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private BGMSlot[] bgmSlots;

    public static BGMSlot selectedSlot;

    private void Start()
    {
        SetSlots();

        foreach(var hihi in FindObjectsOfType<AudioListener>())
        {
            Debug.Log(hihi.name);
        }
    }

    private void SetSlots()
    {
        for(int i = 0; i < bgms.Length; i++)
            bgmSlots[i].SetSlot(bgms[i]);
    }
}
