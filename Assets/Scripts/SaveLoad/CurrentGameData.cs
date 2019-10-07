using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CurrentGameData : MonoBehaviour
{
    public static CurrentGameData Instance;

    [SerializeField] SaveLoadSystem.KnightGameData currentData = null;

    public SaveLoadSystem.KnightGameData CurrentData => currentData;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AssignCurrentData(SaveLoadSystem.KnightGameData newData)
    {
        if (currentData != null && currentData.playerData != null && !string.IsNullOrEmpty(currentData.playerData.playerName))
        {
            Debug.LogError("Cannot write a new file into the Current Game Data, because there is one loaded already!");
            return;
        }

        currentData = newData;
    }

    public void AssignNewCharacterName(string charName)
    {
        if (currentData != null && !SaveLoadSystem.CheckIfSaveFileExists(charName))
        {
            if (currentData.playerData == null)
            {
                currentData.playerData = new SaveLoadSystem.PlayerSaveData();
            }
            currentData.playerData.playerName = charName;
            SaveLoadSystem.SaveData(currentData);
        }
        else
        {
            Debug.LogError("Current data is empty, or found a duplicate name in save folder");
        }
    }

    public void ReplaceCharWithOffspring(string offspringName)
    {
        if (currentData != null && !SaveLoadSystem.CheckIfSaveFileExists(name))
        {
            if (currentData.playerData == null)
            {
                currentData.playerData = new SaveLoadSystem.PlayerSaveData();
            }
            SaveLoadSystem.KnightGameData oldData = currentData;
            SaveLoadSystem.KnightGameData newData = new SaveLoadSystem.KnightGameData();
            newData.pastOffspring.Add(oldData.playerData);
            newData.playerData = new SaveLoadSystem.PlayerSaveData();
            newData.playerData.playerName = offspringName;
            newData.timeSaved = DateTime.Now.ToString("O");
            currentData = newData;
            SaveLoadSystem.SaveOffspringData(newData);
        }
        else
        {
            Debug.LogError("Current data is empty, or found a duplicate name in save folder");
        }
    }

    public void ClearCurrentData()
    {
        currentData = null;
    }

}
