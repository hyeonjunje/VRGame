using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> inGameSound;
    [SerializeField] private AudioSource lobbySound;
    
    private void Start()
    {
        GameManager.instance.gameStartEvent += StartGame;
    }


    private void StartGame()
    {
        foreach (AudioSource audioSource in inGameSound)
            audioSource.enabled = true;

        lobbySound.enabled = false;
    }


/*    private void EndGame()
    {
        foreach (AudioSource audioSource in inGameSound)
            audioSource.enabled = false;
    }*/
}
