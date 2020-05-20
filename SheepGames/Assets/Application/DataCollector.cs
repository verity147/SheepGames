using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DataCollector
{
    ///click + to enhance
    #region Pre-populated savegame data
    private static string[] exampleNames = { "John McMullen", "Jack Cross", "Hamish Pride", "Nathan MacAngus", "Ben McBrick",
                                             "Megan MacAngus", "Holly McMillen", "Olivia Darksense", "Cat Bluebell", "Janet Kelden" };
    #endregion

    public static Dictionary<string, Dictionary<string, int>> tempPlayerDict;
    public static string currentPlayer = "NONE";
    public static int currentScore = -1000000;
    public static string currentLevel = "UNDETERMINED";
    public static int currentGameTotal;
    public const string TOTAL_SCORE = "ALL_Total";

    private static readonly int defaultScore = -1000000;
    internal static int oldScore;
    private static int oldGameTotal;
    private static int oldTotal;

    ///THESE HAVE TO BE FILLED ACCORDING TO THE ACTUAL AMOUNT OF LEVELS
    internal static string[] jh_Levels = { "JH_GameLV_01", "JH_GameLV_02", "JH_GameLV_03" };
    private static string[] pp_Levels = { "PP_GameLV_01", "PP_GameLV_02", "PP_GameLV_03" };
    private static string[] pw_Levels = { "PW_GameLV_01", "PW_GameLV_02", "PW_GameLV_03" };
    private static string[] hr_Levels = { "HR_GameLV_01" };

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
        }else
        {
            Debug.Log("A new savefile is being written.");
            tempPlayerDict = new Dictionary<string, Dictionary<string, int>>(); ///level, scoreDict

            GenerateExampleScorePerLevel(2, 24, jh_Levels[0]);
            GenerateExampleScorePerLevel(8, 43, jh_Levels[1]);
            GenerateExampleScorePerLevel(15, 63, jh_Levels[2]);
            GenerateExampleScorePerLevel(10, 25, pp_Levels[0]);
            GenerateExampleScorePerLevel(16, 33, pp_Levels[1]);
            GenerateExampleScorePerLevel(21, 44, pp_Levels[2]);
            GenerateExampleScorePerLevel(14, 31, pw_Levels[0]);
            GenerateExampleScorePerLevel(18, 33, pw_Levels[1]);
            GenerateExampleScorePerLevel(21, 36, pw_Levels[2]);
            GenerateExampleScorePerLevel(55, 98, hr_Levels[0]);

            /////for each game...
            //foreach (KeyValuePair<string, string[]> levelNames in gameLevels)
            //{
            //    ///...take every level of that game...
            //    if (levelNames.Key != "Hillrace")
            //    {
            //        for (int k = 0; k < levelNames.Value.Length; k++)
            //        {
            //            Dictionary<string, int> scoreDict = new Dictionary<string, int>();
            //            ///...and add each template player with a certain score to a new dictionary...
            //            for (int j = 0; j < exampleNames.Length; j++)
            //            {
            //                int newScore = Random.Range(100, 900);  //change arbitrary scores to match reality
            //                scoreDict.Add(exampleNames[j], newScore);
            //            }
            //            ///...then add this new dictionary to the actual savegame data and save
            //            tempPlayerDict.Add(levelNames.Value[k], scoreDict);
            //        }
            //    }
            //}

            SaveLoadManager.Save();
        }
    }

    private static void GenerateExampleScorePerLevel(int min, int max, string level)
    {
        Dictionary<string, int> scoreDict = new Dictionary<string, int>(); ///playername, score

        foreach (string name in exampleNames)
        {
            int newScore = Random.Range(min, max);
            newScore *= 10;
            scoreDict.Add(name, newScore);
        }
        tempPlayerDict.Add(level, scoreDict);
    }

    public static void UpdateScore(int newScore)
    {
        currentLevel = SceneManager.GetActiveScene().name;
        CheckForSaveFile();
        oldScore = defaultScore;
        ///if there is a valid player name...
        if (!string.IsNullOrEmpty(currentPlayer))
        {
            currentScore = newScore;
            ///...check if he's played before...
            if (tempPlayerDict[currentLevel].ContainsKey(currentPlayer))
            {
                oldScore = tempPlayerDict[currentLevel][currentPlayer];
                ///...and test if the new score is a new best
                if (newScore > oldScore)
                {
                    tempPlayerDict[currentLevel][currentPlayer] = newScore;
                }
                else
                {
                    Debug.Log("Score was too low!");
                }
            }///...or add him to the savefile if he's new
            else if (!tempPlayerDict[currentLevel].ContainsKey(currentPlayer))
            {
                tempPlayerDict[currentLevel].Add(currentPlayer, currentScore);
            }
            SaveLoadManager.Save();
        }
        else
        {
            Debug.LogWarning("No Player found!");
        }

        #region DEBUG ONLY
        //foreach (KeyValuePair<string, Dictionary<string, int>> kvp in tempPlayerDict)
        //{
        //    string player = kvp.Key;
        //    ICollection coll = kvp.Value;
        //    foreach (KeyValuePair<string, int> item in coll)
        //    {
        //        Debug.Log("level: " + player + "; player: " + item.Key + ": " + item.Value);
        //    }
        //}
        //List<KeyValuePair<string, int>> test = SortScore(currentLevel);
        //foreach (var item in test)
        //{
        //    Debug.Log(item);
        //}
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

        Dictionary<string, int>[] gameTotals = { jhTotal, pwTotal, ppTotal, hrTotal }; //find shortest game total

        /// for each entry in one of the dictionary
        for (int i = 0; i < jhTotal.Count; i++)
        {
            ///one player is chosen from the highscorelist for one game...
            ///if someone has not played one of the games, he's not qualified for the highscore anyways
            string player = jhTotalList[i].Key;
            int score = 0;
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
        //int i = 0;
        //foreach(KeyValuePair<string,int> entry in list)
        //{
        //    i++;
        //    Debug.LogFormat("{0}. Player: {1}, Score: {2}", i, entry.Key, entry.Value);
        //}
        list.Sort(Compare);
        return list;
    }

    private static int Compare(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
    {
        return b.Value.CompareTo(a.Value);
    }
    /* 
     * /// not necessary function, kept jic
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
    */
}
