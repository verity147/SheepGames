using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour {

    public string key;

	void Start () {
        TMP_Text text = GetComponent<TMP_Text>();
        text.text = LocalizationManager.instance.GetLocalizedText(key);
	}
}
