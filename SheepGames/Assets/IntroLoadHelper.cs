using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLoadHelper : MonoBehaviour {

    public void NotifySceneHandler()
    {
        FindObjectOfType<SceneHandler>().LoadLocalizationMenu();
    }
}
