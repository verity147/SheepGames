using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class DataCollector
{
    private static string[] exampleNames = { "Carl", "Patrick", "Hans" };
    private static string[] levelNames = { "JH_GameLV_01", "JH_GameLV_02", "JH_GameLV_03" };
    private static int[] pointValues = { 100, 300, 600 };

    public static Dictionary<string, Dictionary<string, int>> playerDict;
    public static string currentPlayer = "NONE";
    public static int currentScore;

    public static void CheckForSaveFile()
    {
        if (SaveLoadManager.CheckForExistingFile())
        {
            Debug.Log("hi");
            return;
        }else
        {
            playerDict = new Dictionary<string, Dictionary<string, int>>();

            for (int i = 0; i < exampleNames.Length; i++)
            {
                Dictionary<string, int> levelDict = new Dictionary<string, int>();
                for (int j = 0; j < levelNames.Length; j++)
                {
                    levelDict.Add(levelNames[j], pointValues[j]);
                }
                playerDict.Add(exampleNames[i], levelDict);
            }
        }
    }

    public static bool CheckForPlayer()
    {
        foreach(KeyValuePair<string, Dictionary<string, int>> entry in playerDict)
        {
            if(entry.Key == currentPlayer)
            {
                return true;
            }
        }        
        return false;
    }




}
