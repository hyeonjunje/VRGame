using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Zombie Sound Collection", menuName = "SoundData/Create New Zombie Sound Collection")]
public class ZombieSoundCollection : ScriptableObject
{
    public List<AudioClip> footstepSounds = new List<AudioClip>();
    public List<AudioClip> attackSounds = new List<AudioClip>();
    public List<AudioClip> hurtSounds = new List<AudioClip>();
    public List<AudioClip> idleSound = new List<AudioClip>();
}
