using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UISaveLoadScreen : MonoBehaviour
{
    [SerializeField] Button[] saveLoadButtons;
    [SerializeField] UINameGenerator nameGenScreen;
    [SerializeField] SceneSwitcher sceneSwitcher;
    [SerializeField] GameObject backButton;
    CanvasGroup cg;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        sceneSwitcher = FindObjectOfType<SceneSwitcher>();
    }

    public void DisableBackButton()
    {
        backButton.gameObject.SetActive(false);
    }

    #region Animate In / Out

    public void AnimateIn()
    {
        if (!cg)
            return;

        cg.interactable = true;
        cg.blocksRaycasts = true;

        LeanTween.alphaCanvas(cg, 1, 0.23f);
        GenerateSaveLoadButtons();
    }

    public void AnimateOut()
    {
        if (!cg)
            return;

        cg.interactable = false;
        cg.blocksRaycasts = false;

        LeanTween.alphaCanvas(cg, 0, 0.18f);
    }

    #endregion

    #region Data

    void GenerateSaveLoadButtons()
    {
        SaveLoadSystem.KnightGameData[] retrievedData = SaveLoadSystem.RetrieveSaveFiles();
        
        for (int i = 0; i < retrievedData.Length; i++)
        {
            if (retrievedData[i] != null)
            {
                SaveLoadSystem.KnightGameData data = SaveLoadSystem.LoadData(retrievedData[i].playerData.playerName);
                saveLoadButtons[i].GetComponent<SaveLoadButton>().PopulateAsLoad(retrievedData[i]);
                saveLoadButtons[i].onClick.RemoveAllListeners();
                saveLoadButtons[i].onClick.AddListener(() => 
                {                    
                    if (!CurrentGameData.Instance)
                    {
                        Debug.LogError("Didn't find an instance of the CurrentGameData in the scene, so can't asign to it!");
                        return;
                    }
                    else
                    {
                        CurrentGameData.Instance.AssignCurrentData(data);
                        sceneSwitcher.SwitchToScene(1);
                    }
                });
            }
            else
            {
                saveLoadButtons[i].GetComponent<SaveLoadButton>().PopulateAsSave(i + 1);
                saveLoadButtons[i].onClick.RemoveAllListeners();
                saveLoadButtons[i].onClick.AddListener(() => 
                {
                    SaveLoadSystem.KnightGameData newData = new SaveLoadSystem.KnightGameData();
                    newData.timeSaved = DateTime.Now.ToString("O");
                    CurrentGameData.Instance.AssignCurrentData(newData);
                    nameGenScreen?.AnimateIn();
                });
            }
        }
    }

    #endregion
}
