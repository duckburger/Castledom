﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (currentData != null && !string.IsNullOrEmpty(currentData.playerData.playerName))
        {
            Debug.LogError("Cannot write a new file into the Current Game Data, because there is one loaded already!");
            return;
        }

        currentData = newData;
    }

    public void AssignNewCharacterName(string name)
    {
        if (currentData != null && !SaveLoadSystem.CheckIfSaveFileExists(name))
        {
            currentData.playerData.playerName = name;
            SaveLoadSystem.SaveData(currentData);
        }
        else
        {
            Debug.LogError("Current data is empty, or found a duplicate name in save folder");
        }
    }

}