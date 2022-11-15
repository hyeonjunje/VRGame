using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSoundPlayer : MonoBehaviour
{
    [SerializeField] private FootStepCollection[] footstepCollections;
    [SerializeField] private LayerMask surfaceLayer;

    private FootStepCollection footstepCollection = null;
    private FootStepCollection prevFootstepCollection = null;

    private AudioSource audioSource;
    private RaycastHit hit;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayFootStepSound()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out hit,  5f, surfaceLayer))
        {
            prevFootstepCollection = footstepCollection;

            if (hit.transform.name == "Terrain")
                footstepCollection = footstepCollections[0];
            else
                footstepCollection = footstepCollections[1];

            if (footstepCollection != prevFootstepCollection)
                audioSource.Stop();

        }

        if(!audioSource.isPlaying)
        {
            if (audioSource.clip == footstepCollection.footstepSounds[0])
                audioSource.clip = footstepCollection.footstepSounds[1];
            else
                audioSource.clip = footstepCollection.footstepSounds[0];

            audioSource.Play();
        }
    }


    public void StopFootStepSound()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
