using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class DataCollector
{
    ///click + to enhance
    #region Pre-populated savegame data
    private static string[] exampleNames = { "Carl", "Patrick", "Hans" };
    private static string[] levelNames = { "JH_GameLV_01", "JH_GameLV_02", "JH_GameLV_03" };
    private static int[] pointValues = { 100, 300, 600 };
    #endregion

    public static Dictionary<string, Dictionary<string, int>> tempPlayerDict;
    public static string currentPlayer = "NONE";
    public static int currentScore = 0;
    public static string currentLevel = "UNDETERMINED";


    public static void CheckForSaveFile()
    {
        if (SaveLoadManager.CheckForExistingFile() == true)
        {
            Debug.Log("File found");
            SaveLoadManager.Load();
            return;
        }else{
            Debug.Log("new file");
            tempPlayerDict = new Dictionary<string, Dictionary<string, int>>();

            for (int i = 0; i < exampleNames.Length; i++)
            {
                Dictionary<string, int> levelDict = new Dictionary<string, int>();
                for (int j = 0; j < levelNames.Length; j++)
                {
                    levelDict.Add(levelNames[j], pointValues[j]);
                }
                tempPlayerDict.Add(exampleNames[i], levelDict);
            }
            SaveLoadManager.Save();
        }
    }

    public static void CheckForPlayer()
    {
        foreach(KeyValuePair<string, Dictionary<string, int>> entry in tempPlayerDict)
        {
            ///if player already exists, just load the savegame and end function
            if(entry.Key == currentPlayer)
            {
                SaveLoadManager.Load();
                return;
            }
        }
        ///else write the new player into the tempDict with placeholder scores of -100000 so he doesn't show up in the highscore list
        Dictionary<string, int> levelDict = new Dictionary<string, int>();
        for (int j = 0; j < levelNames.Length; j++)
        {
            Debug.Log("adding name");
            levelDict.Add(levelNames[j], -100000);
        }
        tempPlayerDict.Add(currentPlayer, levelDict);
        SaveLoadManager.Save();        
    }

    public static void UpdateScore(int newScore)
    {
        Debug.Log("i got here");
        //currentLevel = "JH_GameLV_01";
        currentLevel = SceneHandler.FindActiveSceneName();

        int oldScore = tempPlayerDict[currentPlayer][currentLevel];
        if (newScore > oldScore)
        {
            tempPlayerDict[currentPlayer][currentLevel] = newScore;
            SaveLoadManager.Save();
        }
        else
        {
            Debug.Log("Score wasn't high enough!");
        }
        foreach (KeyValuePair<string, Dictionary<string, int>> kvp in tempPlayerDict)
        {
            string player = kvp.Key;
            //print(player);
            ICollection coll = kvp.Value;
            foreach (KeyValuePair<string, int> item in coll)
            {
                Debug.Log("Player: " + player + "; Level: " + item.Key + ": " + item.Value);
            }
        }
    }

    public static void UpdateScoreboard()
    {
        List<int> scoreList;

        //fill, sort, return first ten

        //scoreList = new List<int>(tempPlayerDict.)
    }
}
