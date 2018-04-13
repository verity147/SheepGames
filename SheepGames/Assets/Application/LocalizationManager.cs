﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Load this script from language selection menu.
/// Tutorial taken from: https://www.youtube.com/watch?v=IRQBIPcwjC8
/// 
/// </summary>


public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager instance;
    public SceneHandler sceneHandler;

    private Dictionary<string, string> localizedText;

    private bool isLoaded = false;
    private string missingText = "Localization not found!";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if (instance !=this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLocalization(string fileName)
    {
        StartCoroutine(LocalizationReady());
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string rawText = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(rawText);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
        }
        else
        {
            Debug.LogError("Localization file not found!");
        }
        isLoaded = true;
    }

    public string GetLocalizedText(string key)
    {
        string result = missingText;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        return result;
    }

    private IEnumerator LocalizationReady()
    {
        while (!isLoaded)
        {
            yield return null;
        }
        sceneHandler.LoadLevel("MainMenu");
    }
}