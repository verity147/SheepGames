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


    public static bool CheckForSaveFile()
    {
        if (SaveLoadManager.CheckForExistingFile() == true)
        {
            Debug.Log("found it!");
            return true;
        }else{
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
            return false;
        }
    }

    public static bool CheckForPlayer()
    {
        foreach(KeyValuePair<string, Dictionary<string, int>> entry in tempPlayerDict)
        {
            if(entry.Key == currentPlayer)
            {
                return true;
            }
        }        
        return false;
    }




}
