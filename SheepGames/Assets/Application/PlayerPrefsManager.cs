using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager  {

    const string MUSIC_VOLUME_KEY = "musicVolume";
    const string SFX_VOLUME_KEY = "sfxVolume";
    const string FULLSCREEN_KEY = "fullscreen";
    const string RESOLUTION_WIDTH_KEY = "resolutionWidth";
    const string RESOLUTION_HEIGHT_KEY = "resolutionHeight";
    const string LANGUAGE_KEY = "language";

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

    public static void SetFullscreen(int fullscreen)
    {
        PlayerPrefs.SetInt(FULLSCREEN_KEY, fullscreen);
    }

    public static bool GetFullscreen()
    {
        if (PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1)
        {
            return true;
        }else if (PlayerPrefs.GetInt(FULLSCREEN_KEY) == 0)
        {
            return false;
        }
        else
        {
            Debug.LogWarning("Could not detect value for fullscreen");
            return true;
        }
    }

    public static void SetResolution(string width, string height)
    {
        PlayerPrefs.SetString(RESOLUTION_WIDTH_KEY, width);
        PlayerPrefs.SetString(RESOLUTION_HEIGHT_KEY, height);
    }

    public static int GetResolutionWidth()
    {
        return PlayerPrefs.GetInt(RESOLUTION_WIDTH_KEY);
    }

    public static int GetResolutionHeight()
    {
        return PlayerPrefs.GetInt(RESOLUTION_HEIGHT_KEY);
    }

    public static void SetLanguage(string languageFile)
    {
        PlayerPrefs.SetString(LANGUAGE_KEY, languageFile);
    }

    public static string GetLangage()
    {
        return PlayerPrefs.GetString(LANGUAGE_KEY);
    }

    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
