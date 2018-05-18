using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsDisplayHelper : MonoBehaviour {

    public Slider sfxSlider;
    public Slider musicSlider;

    private void OnEnable()
    {
        sfxSlider.value = PlayerPrefsManager.GetSfxVolume();
        musicSlider.value = PlayerPrefsManager.GetMusicVolume();        
    }
}
