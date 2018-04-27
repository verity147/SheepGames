﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



public static class DataCollector
{
    ///click + to enhance
    #region Pre-populated savegame data
    private static string[] exampleNames = { "Carl", "Patrick", "Hans", "Otto" };
    private static string[] levelNames = { "JH_GameLV_01", "JH_GameLV_02", "JH_GameLV_03" };
    private static int[] pointValues = { 100, 800, 600, 725 };
    #endregion

    public static Dictionary<string, Dictionary<string, int>> tempPlayerDict;
    public static string currentPlayer = "NONE";
    public static int currentScore = -1000000;
    public static string currentLevel = "UNDETERMINED";
    public static int currentGameTotal;
    public const string TOTAL_SCORE = "ALL_Total";

    internal static int oldScore = -1000000;
    private static int oldGameTotal;
    private static int oldTotal;

    ///THESE HAVE TO BE FILLED ACCORDING TO THE ACTUAL AMOUNT OF LEVELS
    private static string[] jh_Levels = { "JH_GameLV_01", "JH_GameLV_02", "JH_GameLV_03" };
    private static string[] pp_Levels = { "PP_GameLV_01", "PP_GameLV_02", "PP_GameLV_03" };
    private static string[] pw_Levels = { "PW_GameLV_01", "PW_GameLV_02", "PW_GameLV_03" };
    private static string[] hr_Levels = { "HR_GameLV_01", "HR_GameLV_02", "HR_GameLV_03" };

    public static Dictionary<string, string[]> gameLevels = new Dictionary<string, string[]>()
    {
        { "JumpForHeight", jh_Levels },
        { "PuzzlingPatterns", pp_Levels },
        { "PressOWar", pw_Levels },
        { "Hillrace", hr_Levels }
    };

    public static void CheckForSaveFile()
    {
        if (SaveLoadManager.CheckForExistingFile() == true)
        {
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
                Debug.Log("Current Score: " + currentScore);
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
        //List<KeyValuePair<string, int>> test = SortScoreboard(currentLevel);
        //foreach(var item in test)
        //{
        //    Debug.Log(item);
        //}
        //Debug.Log("Name: " + currentPlayer);
        #endregion
    }

    ///can get the total score list for a game when supplied with the respective level names
    public static List<KeyValuePair<string, int>> GetGameTotals(string[] levels)
    {
        ///this list will hold the results
        List<KeyValuePair<string, int>> scoreTotals = new List<KeyValuePair<string, int>>();
        ///gets the results of the first level in form of a kvp list (first level will definitely have every player)
        List<KeyValuePair<string, int>> levelScore = new List<KeyValuePair<string, int>>(tempPlayerDict[levels[0]]);
        ///for the entire list...
        for (int j = 0; j < levelScore.Count; j++)
        {
            ///...get one player...
            string player = levelScore[j].Key;
            int playerScore = 0;
            bool addToTotal = true;
            foreach (string level in levels)
            {
                ///...and for each level of the specified game, add that players score to a total...
                ///...with a check if the player has completed each level of said game and only includes him if he does
                if (tempPlayerDict[level].ContainsKey(player))
                {
                    playerScore += tempPlayerDict[level][player];
                }
                else
                {
                    addToTotal = false;
                    break;
                }
            }
            ///put the total into the list of results
            if (addToTotal)
            {
                scoreTotals.Add(new KeyValuePair<string,int>(player,playerScore));
            }
        }
        ///sort the result list from highest to lowest and return it
        scoreTotals.Sort(Compare);
        return scoreTotals;
    }
    ///calculate the overall total highscore
    public static List<KeyValuePair<string,int>> GetScoreTotal()
    {        
        List<KeyValuePair<string, int>> jhTotalList = GetGameTotals(jh_Levels);

        Dictionary<string, int> jhTotal = jhTotalList.ToDictionary(pair => pair.Key, pair => pair.Value);
        Dictionary<string, int> pwTotal = GetGameTotals(pw_Levels).ToDictionary(pair => pair.Key, pair => pair.Value);
        Dictionary<string, int> ppTotal = GetGameTotals(pp_Levels).ToDictionary(pair => pair.Key, pair => pair.Value);
        Dictionary<string, int> hrTotal = GetGameTotals(hr_Levels).ToDictionary(pair => pair.Key, pair => pair.Value);
        ///list for the future results
        List<KeyValuePair<string, int>> totals = new List<KeyValuePair<string, int>>();

        Dictionary<string, int>[] gameTotals = { jhTotal, pwTotal, ppTotal, hrTotal };

        /// for each entry in the longest dictionary ( = the level with the most different players)
        for (int i = 0; i < jhTotal.Count; i++)
        {
            ///one player is chosen from the highscorelist for one game...
            ///if someone has not played one of the games, he's not qualified for the highscore anyways
            string player = jhTotalList[i].Key;
            int score = jhTotalList[i].Value;
            bool addToTotal = true;
            
            foreach (Dictionary<string, int> list in gameTotals)
            {
                ///...check for his name in every game...
                if (list.ContainsKey(player))
                {
                    score += list[player];
                }
                else ///...if he's not played one of the games, prevent his name from being added to the total highscore list...
                {
                    addToTotal = false;
                    break;
                }
            }
            if (addToTotal)
            {
                ///...add the player to the highscore list
                totals.Add(new KeyValuePair<string, int>(player, score));
            }
        }
        totals.Sort(Compare);
        return totals;   
    }

    public static List<KeyValuePair<string, int>> SortScore(string level)
    {    
        List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(tempPlayerDict[level]);
        int i = 0;
        foreach(KeyValuePair<string,int> entry in list)
        {
            i++;
            Debug.LogFormat("{0}. Player: {1}, Score: {2}", i, entry.Key, entry.Value);
        }
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
}
