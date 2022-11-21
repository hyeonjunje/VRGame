using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Footstep Collection", menuName = "SoundData/Create New Footstep Collection")]
public class FootStepCollection : ScriptableObject
{
    public List<AudioClip> footstepSounds = new List<AudioClip>();
}
