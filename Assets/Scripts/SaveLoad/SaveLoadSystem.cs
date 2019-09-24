using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public static class SaveLoadSystem
{
    [Serializable]
    public class KnightGameData
    {
        public string timeSaved;
        public PlayerSaveData playerData;
    }

    [Serializable]
    public class PlayerSaveData
    {
        public string playerName;
        public float[] playerPosition;
        public float[] playerRotation;        
    }


    #region Pulling Out Existing Files

    public static KnightGameData[] RetrieveSaveFiles()
    {
        KnightGameData[] foundFiles = new KnightGameData[3];
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, $"SaveFiles")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, $"SaveFiles"));
        }

        BinaryFormatter bFormatter = new BinaryFormatter();    

        string[] filePaths = Directory.GetFiles(Path.Combine(Application.persistentDataPath, $"SaveFiles"));
        for (int i = 0; i < filePaths.Length; i++)
        {
            FileStream stream = new FileStream(filePaths[i], FileMode.Open);
            foundFiles[i] = bFormatter.Deserialize(stream) as KnightGameData;
            stream.Close();
        }
        return foundFiles;
    }

    #endregion

    #region Saving/Loading

    public static bool CheckIfSaveFileExists(string charName)
    {
        return File.Exists(Path.Combine(Application.persistentDataPath, $"SaveFiles/{charName}.road"));
    }

    public static void SaveData(KnightGameData dataToWrite)
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        string filePath = Path.Combine(Application.persistentDataPath, $"SaveFiles/{dataToWrite.playerData.playerName}.road");

        FileStream stream = new FileStream(filePath, FileMode.Create);
        bFormatter.Serialize(stream, dataToWrite);
        stream.Close();
    }

    public static KnightGameData LoadData(string charName)
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        string filePath = Path.Combine(Application.persistentDataPath, $"SaveFiles/{charName}.road");
        FileStream stream = new FileStream(filePath, FileMode.Open);

        KnightGameData retrievedData = bFormatter.Deserialize(stream) as KnightGameData;
        stream.Close();
        return retrievedData;
    }

    #endregion

    #region Deleting Data

    public static void DeleteCharacterData(string characterName)
    {
        if (CheckIfSaveFileExists(characterName))
        {
            File.Delete(Path.Combine(Application.persistentDataPath, $"SaveFiles/{characterName}.road"));
        }
    }

    #endregion

}
