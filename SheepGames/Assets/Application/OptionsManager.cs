using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour {

    public AudioMixer audioMixer;

    private Resolution[] resolutions;
    internal List<string> resolutionsList;
    internal int currentResIndex = 0;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionsList = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionsList.Add(option);

            if(resolutions[i].width==Screen.currentResolution.width&&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        audioMixer.SetFloat("MusicVolume", PlayerPrefsManager.GetMusicVolume());
        audioMixer.SetFloat("SFXVolume", PlayerPrefsManager.GetSfxVolume());
    }

    public void ChangeMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefsManager.SetMusicVolume(volume);
    }

    public void ChangeSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefsManager.SetSfxVolume(volume);
    }

    public void SetResolution(int index)
    {
        ///resolutions needs to be set a second time because the script forgets it.
        resolutions = Screen.resolutions;
        Resolution resolution = resolutions[currentResIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
}
