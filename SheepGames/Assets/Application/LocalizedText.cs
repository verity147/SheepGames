using UnityEngine;
using System;
using TMPro;

public class LocalizedText : MonoBehaviour {

    public string key;

	void Start () {
        TMP_Text text = GetComponent<TMP_Text>();

        if (!LocalizationManager.localizationManager)
        {
            Debug.LogError("The LocalizationManager is not loaded!");
            return;

        }

        if(string.IsNullOrEmpty(LocalizationManager.localizationManager.GetLocalizedText(key)))
            Debug.LogWarning(gameObject.name.ToString() + " could not find translation file for " + key);
        else
            text.text = LocalizationManager.localizationManager.GetLocalizedText(key);       
	}
}
