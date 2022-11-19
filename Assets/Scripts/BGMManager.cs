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
    }

    private void SetSlots()
    {
        for(int i = 0; i < bgms.Length; i++)
            bgmSlots[i].SetSlot(bgms[i]);
    }
}
