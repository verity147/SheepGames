using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public AudioMixer audioMixer;

    private void Awake()
    {
        //audioMixer = FindObjectOfType<AudioMixer>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
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
}
