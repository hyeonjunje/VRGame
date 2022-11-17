using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> inGameSound;
    [SerializeField] private AudioSource lobbySound;
    
    private void Start()
    {

    }

/*    private void EndGame()
    {
        foreach (AudioSource audioSource in inGameSound)
            audioSource.enabled = false;
    }*/
}
