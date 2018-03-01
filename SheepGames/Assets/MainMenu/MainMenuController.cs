using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public void SetPlayerName(string playerName)
    {
        DataCollector.currentPlayer = playerName;
    }

    public void ShowDict()
    {
        if(DataCollector.CheckForSaveFile() == true)
        {
            SaveLoadManager.Load();
        }
        else
        {
            SaveLoadManager.Save();
            SaveLoadManager.Load();
        }

        foreach (KeyValuePair<string, Dictionary<string, int>> kvp in DataCollector.tempPlayerDict)
        {
            string player = kvp.Key;
            ICollection coll = kvp.Value;
            foreach (KeyValuePair<string, int>item in coll)
            {
                Debug.Log("Player: "+player+"; Level: "+item.Key+": "+item.Value);
            }
        }
    }
}
