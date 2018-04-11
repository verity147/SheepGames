using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Group : MonoBehaviour {

    private ToggleGroup tg;
    private Toggle[] toggles;


    void Start () {
        tg = GetComponent<ToggleGroup>();
		
        toggles = GetComponentsInChildren<Toggle>();
	}

    public void OnPressToggle(bool b)
    {
        foreach(Toggle t in toggles)
        {
            print(t.GetComponentInChildren<Text>().text + t.isOn);
        }
    }
	

}
