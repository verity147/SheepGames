using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public static class DataCollector
{
    ///click + to enhance
    #region Pre-populated savegame data
    private static string[] exampleNames = { "Carl", "Patrick", "Hans", "Otto" };
    private static string[] levelNames = { "JH_GameLV_01", "JH_GameLV_02", "JH_GameLV_03" , "JH_Total", "ALL_Total"};
    private static int[] pointValues = { 100, 300, 600, 725 };
    #endregion

    public static Dictionary<string, Dictionary<string, int>> tempPlayerDict;
    public static string currentPlayer = "NONE";
    public static int currentScore = -1000000;
    public static string currentLevel = "UNDETERMINED";
    public static int currentGameTotal;

    private static int oldScore;
    private static int oldGameTotal;
    private static int oldTotal;


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
        currentLevel = SceneManager.GetActiveScene().name;
        ///if the player has player this level before and therefore has a saved score...
        if (tempPlayerDict[currentLevel].ContainsKey(currentPlayer))
        {
            oldScore = tempPlayerDict[currentLevel][currentPlayer];
            ///...compare to the new score and if it is not the same, save it
            if (newScore > oldScore)
            {
                tempPlayerDict[currentLevel][currentPlayer] = newScore;
                currentScore = newScore;
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
        Debug.Log("Name: " + currentPlayer);
        #endregion
    }

    public static List<KeyValuePair<string, int>> SortScoreboard(string level)
    {    
        List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(tempPlayerDict[level]);

        list.Sort(Compare);
        return list;
    }

    private static int Compare(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
    {
        return b.Value.CompareTo(a.Value);
    }

    private static string FindGame()
    {
        string game;
        if (currentLevel.Contains("JH"))
        {
            game = "JH_Total";
        }else if (currentLevel.Contains("PP"))
        {
            game = "PP_Total";
        }else if (currentLevel.Contains("HR"))
        {
            game = "HR_Total";
        }else if (currentLevel.Contains("PW"))
        {
            game = "PW_Total";
        }
        else
        {
            Debug.LogError("Could not detect game to add up total");
            game = "NONE";
        }

        return game;
    }

    public static void CalculateTotalScore()
    {
        string game = FindGame();
        oldGameTotal = tempPlayerDict[game][currentPlayer];
        oldTotal = tempPlayerDict["ALL_Total"][currentPlayer];

        ///resets the per game total to 0 if it's the first level of any game
        if (currentLevel.Contains("01"))
        {
            currentGameTotal = 0;
        }
        currentGameTotal += currentScore;

        ///checks if the new total beats the old and overwrites it if so per game
        if(currentGameTotal > oldGameTotal)
        {
            tempPlayerDict[game][currentPlayer] = currentGameTotal;
        }

        ///checks if the new total beats the old and overwrites it if so
        int newTotal = tempPlayerDict["JH_Total"][currentPlayer]; //+ the other games' totals
        if (newTotal > oldTotal)
        {
            tempPlayerDict["ALL_Total"][currentPlayer] = newTotal;
        }
        SaveLoadManager.Save();
    }
}
