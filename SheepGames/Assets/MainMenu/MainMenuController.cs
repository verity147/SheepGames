using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void SetPlayerName(string playerName)
    {
        DataCollector.currentPlayer = playerName;
    }

    public void ShowDict()
    {
        DataCollector.CheckForSaveFile();
        print(DataCollector.playerDict);
        SaveLoadManager.Save();
    }
}
