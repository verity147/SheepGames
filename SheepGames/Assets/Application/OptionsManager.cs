using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour {

    public AudioMixer audioMixer;

    private Resolution[] resolutions;
    internal List<string> resolutionsList;
    internal int currentResIndex = 0;

    private void Awake()
    {
        //audioMixer = FindObjectOfType<AudioMixer>();
        DontDestroyOnLoad(gameObject);
    }

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
        Resolution resolution = resolutions[currentResIndex];
        Screen.SetResolution(resolution.width,resolution.height, Screen.fullScreen);
    }
}
