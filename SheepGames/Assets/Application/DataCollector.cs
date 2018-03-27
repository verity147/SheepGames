using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class DataCollector
{
    ///click + to enhance
    #region Pre-populated savegame data
    private static string[] exampleNames = { "Carl", "Patrick", "Hans", "Otto" };
    private static string[] levelNames = { "JH_GameLV_01", "JH_GameLV_02", "JH_GameLV_03" };
    private static int[] pointValues = { 100, 300, 600, 725 };
    #endregion

    public static Dictionary<string, Dictionary<string, int>> tempPlayerDict;
    public static string currentPlayer = "NONE";
    public static int currentScore = -1000000;
    public static string currentLevel = "UNDETERMINED";

    private static int oldScore;

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

            for (int i = 0; i < levelNames.Length; i++)
            {
                Dictionary<string, int> scoreDict = new Dictionary<string, int>();
                for (int j = 0; j < exampleNames.Length; j++)
                {
                    scoreDict.Add(exampleNames[j], pointValues[j]);
                }
                tempPlayerDict.Add(levelNames[i], scoreDict);
            }
            SaveLoadManager.Save();
        }
    }

    public static void UpdateScore(int newScore)
    {
        currentLevel = SceneHandler.FindActiveSceneName();
        currentScore = newScore;
        ///if the player has player this level before and therefore has a saved score...
        if (tempPlayerDict[currentLevel].ContainsKey(currentPlayer))
        {
            oldScore = tempPlayerDict[currentLevel][currentPlayer];
            ///...compare to the new score and if it is not the same, save it
            if (newScore > oldScore)
            {
                tempPlayerDict[currentLevel][currentPlayer] = newScore;
            }
            else
            {
                Debug.Log("Score was too low!");
            }
        }
        else
        {
            tempPlayerDict[currentLevel].Add(currentPlayer, newScore);
        }
        SaveLoadManager.Save();

        #region DEBUG ONLY
        foreach (KeyValuePair<string, Dictionary<string, int>> kvp in tempPlayerDict)
        {
            string player = kvp.Key;
            //print(player);
            ICollection coll = kvp.Value;
            foreach (KeyValuePair<string, int> item in coll)
            {
                Debug.Log("Level: " + player + "; Player: " + item.Key + ": " + item.Value);
            }
        }
        List<KeyValuePair<string, int>> test = SortScoreboard(currentLevel);
        foreach(var item in test)
        {
            Debug.Log(item);
        }
        #endregion
    }

    public static List<KeyValuePair<string, int>> SortScoreboard(string level)
    {
        //fill, sort, return first ten, trim list
        
        List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(tempPlayerDict[level]);

        list.Sort(Compare);
        list.RemoveAt(10);
        return list;
    }

    private static int Compare(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
    {
        return b.Value.CompareTo(a.Value);
    }
}
