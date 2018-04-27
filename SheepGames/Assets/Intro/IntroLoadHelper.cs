using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class IntroLoadHelper : MonoBehaviour {

    public SceneHandler sceneHandler;

    public void NotifySceneHandler()
    {
        LocalizationManager.localizationManager.enabled = true;
        if(String.IsNullOrEmpty(PlayerPrefsManager.GetLangage()))
        {
            sceneHandler.LoadLocalizationMenu();
        }
        else
        {
            LocalizationManager.localizationManager.LoadLocalization(PlayerPrefsManager.GetLangage());
        }
    }
}
