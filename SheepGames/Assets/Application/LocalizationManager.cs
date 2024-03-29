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

    public static LocalizationManager localizationManager;
    public SceneHandler sceneHandler;

    private Dictionary<string, string> localizedText;

    private bool isLoaded = false;
    private readonly string missingText = "Localization not found!";

    private void Awake()
    {
        print("another one");
        ///Localization Manager is needed by every element that holds localized text, 
        ///so it uses a singleton pattern to ensure it is always available and exists only once
        if(localizationManager == null)
        {
            print("+1");
            localizationManager = this;
        }else if (localizationManager !=this)
        {
            print("destroy");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLocalization(string fileName)
    {
        print("trying to load " + fileName);
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
        else
        {
            Debug.LogWarning("Could not find localization data for key " + key);
        }
        return result;
    }

    private IEnumerator LocalizationReady()
    {
        while (!isLoaded)
        {
            yield return null;
        }
        isLoaded = false;
        sceneHandler.LoadLevel("04_MainMenu");
    }

}
