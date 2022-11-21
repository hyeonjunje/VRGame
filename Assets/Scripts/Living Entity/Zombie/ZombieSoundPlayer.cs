using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundPlayer : MonoBehaviour
{
    [SerializeField] private ZombieSoundCollection zombieSoundCollection;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayIdleSound()
    {
        Debug.Log(gameObject.name + " ¼Ò¸®³¿");
        audioSource.PlayOneShot(zombieSoundCollection.idleSound[Random.Range(0, zombieSoundCollection.idleSound.Count)]);
    }


    public void PlayHurtSound()
    {
        audioSource.PlayOneShot(zombieSoundCollection.hurtSounds[Random.Range(0, zombieSoundCollection.hurtSounds.Count)]);
    }


    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(zombieSoundCollection.footstepSounds[Random.Range(0, zombieSoundCollection.footstepSounds.Count)]);
    }


    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(zombieSoundCollection.attackSounds[Random.Range(0, zombieSoundCollection.attackSounds.Count)]);
    }
}
