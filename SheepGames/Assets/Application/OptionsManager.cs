using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour {

    public AudioMixer audioMixer;  
    public SceneHandler sceneHandler;
    public float musicFadeIn = 1f;

    private Resolution[] resolutions;
    internal List<string> resolutionsList;
    internal int currentResIndex = 0;
    private int newResIndex = 0;
    private readonly string[] languageFiles = { "SheepGamesLocalizationEN.json",
                                                "SheepGamesLocalizationDE.json" };

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(0.0001f) * 20); ///so the fade-in actually starts from silence
            StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "MusicVolume", musicFadeIn, PlayerPrefsManager.GetMusicVolume()));
        }
        else
        {
            StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "MusicVolume", musicFadeIn, 1f));
        }
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            ChangeSfxVolume(PlayerPrefsManager.GetSfxVolume());
        }
    }

    internal List<string> BuildResolutionsList()
    {
        resolutions = Screen.resolutions;
        resolutionsList = new List<string>();
        float width;
        float height;

        for (int i = 0; i < resolutions.Length; i++)
        {
            width = resolutions[i].width;
            height = resolutions[i].height;
            if(Mathf.Approximately(width/height, 16f / 9f))
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                resolutionsList.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                   resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = i;
                }
            }

        }
        return resolutionsList;
    }

    public void ChangeMusicVolume(float volume)
    {
        PlayerPrefsManager.SetMusicVolume(volume);
        volume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void ChangeSfxVolume(float volume)
    {
        PlayerPrefsManager.SetSfxVolume(volume);
        volume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void ChangeResolutionIndex(int index)
    {
        newResIndex = index;
    }
    public void ChangeLanguageIndex(int index)
    {
        print("changing to index " + index);
        SaveLanguageSetting(languageFiles[index]);
    }

    public void ChangeResolution()
    {
        ///resolutions needs to be set a second time because the script forgets it.
        resolutions = Screen.resolutions;
        Resolution resolution = resolutions[newResIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefsManager.SetResolution(resolution.width.ToString(), resolution.height.ToString());
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (isFullscreen)
        {
            PlayerPrefsManager.SetFullscreen(1);
        }
        else
        {
            PlayerPrefsManager.SetFullscreen(0);
        }
    }

    public void SaveLanguageSetting(string filename)
    {
        PlayerPrefsManager.SetLanguage(filename);
        LocalizationManager.localizationManager.LoadLocalization(filename);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefsManager.ClearPlayerPrefs();
    }

    public void ClearSaveData()
    {
        if (SaveLoadManager.CheckForExistingFile())
        {
            SaveLoadManager.DeleteSaveData();
            DataCollector.CheckForSaveFile();
        }
    }
}
