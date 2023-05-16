using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadAfterVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField]
    private SceneHandler sceneHandler;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.loopPointReached += VideoEnded;
    }

    private void VideoEnded(VideoPlayer vp)
    {
        sceneHandler.LoadLevel("02_Intro");
    }

}
