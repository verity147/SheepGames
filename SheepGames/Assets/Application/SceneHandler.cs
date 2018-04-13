﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    //just for test builds
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadLocalizationMenu()
    {
        SceneManager.LoadSceneAsync("LocalizationChoice");
    }    

    public void LoadLevel(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
   
}
