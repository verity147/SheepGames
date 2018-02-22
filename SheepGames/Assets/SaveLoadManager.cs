using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager {
    
    //first backup old savefile in case something goes wrong, then save, then delete old backup
    //also build backup restore function

    public static void Save(JH_ScoreCalculator dataToSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Path.Combine(Application.persistentDataPath, "/player.save"), FileMode.Create);

        SaveData data = new SaveData(dataToSave);
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static int Load()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "/player.save")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Path.Combine(Application.persistentDataPath, "/player.save"), FileMode.Open);

            SaveData data = bf.Deserialize(stream) as SaveData;
            stream.Close();
            return data.latestScore;
        }
        else
        {
            Debug.LogError("No Savefile found!");
            return new int(); 
        }
    }

}

[Serializable]
public class SaveData{

    //sorts all player with their highscore List
    public Dictionary<string, int[]> playerDict;
    //generate a new array and playerDict entry if the playername cannot be found in playerDict
    //everytime a player ends a level, compare his score to the one saved and if it's larger, overwrite it
    //sort the entries with their playername for each level into a new dictionary and sort by value
    //save
    //display as highscore with some ui manager function
    public int latestScore;

    public SaveData(JH_ScoreCalculator jH_Score)
    {
        latestScore = jH_Score.score;
    }
}
