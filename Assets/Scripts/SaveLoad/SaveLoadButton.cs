using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SaveLoadButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI emptyFileText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dateText;

    public void PopulateAsSave(int buttonNumber)
    {
        emptyFileText.gameObject.SetActive(true);
        nameText.gameObject.SetActive(false);
        dateText.gameObject.SetActive(false);
    }

    public void PopulateAsLoad(SaveLoadSystem.KnightGameData data)
    {
        emptyFileText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(true);
        dateText.gameObject.SetActive(true);

        nameText.text = data.playerData.playerName;
        dateText.text = DateTime.Parse(data.timeSaved).ToUniversalTime().ToString();
    }
}
