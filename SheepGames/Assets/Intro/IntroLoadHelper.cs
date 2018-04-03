using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLoadHelper : MonoBehaviour {

    public SceneHandler sceneHandler;

    public void NotifySceneHandler()
    {
        sceneHandler.LoadLocalizationMenu();
    }
}
