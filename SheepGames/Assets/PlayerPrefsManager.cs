using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

    const string MUSIC_VOLUME_KEY = "musicVolume";
    const string SFX_VOLUME_KEY = "sfxVolume";
    //add language

    public static void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }

    public static void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
    }

    public static float GetSfxVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
    }
}
