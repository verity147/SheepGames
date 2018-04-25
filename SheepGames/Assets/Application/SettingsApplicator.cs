using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsApplicator : MonoBehaviour {

    public SceneHandler sceneHandler;

    private void Awake()
    {
        Screen.SetResolution(PlayerPrefsManager.GetResolutionWidth(), 
                            PlayerPrefsManager.GetResolutionHeight(), 
                            PlayerPrefsManager.GetFullscreen());
        sceneHandler.LoadLevel("02_Intro");
    }
}
