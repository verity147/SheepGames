using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager {
    
    //first backup old savefile in case something goes wrong, then save, then delete old backup
    //also build backup restore function



    public static void Save()
    {
        Debug.Log("called save()");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Path.Combine(Application.persistentDataPath, "SheepGames.save"), FileMode.Create);
        SaveData data = new SaveData
        {
            playerDict = DataCollector.tempPlayerDict
        };
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static void Load()
    {
        if (CheckForExistingFile() == true)
        {
            ///open file on disk
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Path.Combine(Application.persistentDataPath, "SheepGames.save"), FileMode.Open);
            ///turn data into readable Savedata object
            SaveData data = bf.Deserialize(stream) as SaveData;
            stream.Close();
            ///store savedata in DataCollector.tempPlayerDict
            DataCollector.tempPlayerDict = new Dictionary<string, Dictionary<string, int>>();
            DataCollector.tempPlayerDict = data.playerDict;
            return;
        }
        else
        {
            Debug.LogError("No Savefile found!");
            return; 
        }
    }

    public static bool CheckForExistingFile()
    {
        if(File.Exists(Path.Combine(Application.persistentDataPath, "SheepGames.save")))
        {
            return true;
        }else
        {
            return false;
        }
    }


}

[Serializable]
public class SaveData{

    public Dictionary<string, Dictionary<string, int>> playerDict;

}


