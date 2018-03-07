using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public void SetPlayerName(string playerName)
    {
        DataCollector.currentPlayer = playerName;
        DataCollector.CheckForSaveFile();
        DataCollector.CheckForPlayer();       
    }

    ///DEBUG FUNCTION 
    ///add this to a button on the main menu to write temporary savegame to console
    public void ShowDict()
    {
        ///players without any scores won't get displayed by this, re-enable comment if needed
        foreach (KeyValuePair<string, Dictionary<string, int>> kvp in DataCollector.tempPlayerDict)
        {
            string player = kvp.Key;
            //print(player);
            ICollection coll = kvp.Value;
            foreach (KeyValuePair<string, int>item in coll)
            {
                Debug.Log("Player: " + player + "; Level: " + item.Key + ": " + item.Value);
            }
        }
    }
}
