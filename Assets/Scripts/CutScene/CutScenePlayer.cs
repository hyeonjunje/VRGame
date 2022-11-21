using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public static class CutScenePlayer
{
    private static PlayableDirector playableDirector = null;
    private static TimelineAsset timeline = null;

    public static void SetPlayableDirector(PlayableDirector _playableDirector)
    {
        playableDirector = _playableDirector;
    }


    public static void SetTimeline(TimelineAsset _timeline)
    {
        timeline = _timeline;

        if (playableDirector != null)
            playableDirector.playableAsset = timeline;
    }


    public static void PlayCutScene()
    {
        playableDirector.Play();
    }


    public static void StopCutScene()
    {
        playableDirector.Stop();
    }
}
