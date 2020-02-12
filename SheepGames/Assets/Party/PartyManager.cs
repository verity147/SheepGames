using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public SceneHandler sceneHandler;

    private void OnEnable()
    {
        sceneHandler.LoadLevel("04_MainMenu");
    }
}
