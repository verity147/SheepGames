using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///this gets loaded in the very first scene the game starts with to apply the resolution settings the  player chose last he played
public class SettingsApplicator : MonoBehaviour {

    public SceneHandler sceneHandler;

    private void Awake()
    {
        if(PlayerPrefsManager.GetResolutionWidth() != 0)
        {
            Screen.SetResolution(PlayerPrefsManager.GetResolutionWidth(), 
                                PlayerPrefsManager.GetResolutionHeight(), 
                                PlayerPrefsManager.GetFullscreen());
        }
        StartCoroutine(ShowLogos());
    }

    private IEnumerator ShowLogos()
    {
        yield return new WaitForSecondsRealtime(3);
        sceneHandler.LoadLevel("02_Intro");
    }
}
