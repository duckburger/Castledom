using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] UIAnnouncementBoardController announcementBoard;
    [SerializeField] RectTransform pickupIcon;
    [SerializeField] RectTransform speakIcon;
    [SerializeField] UIObjectivePointer objectivePointer;
    [SerializeField] UIObjectiveTextDisplay objectiveTextDisplay;
    [SerializeField] AudioSource uiAudioSource;
    [SerializeField] UIMoneyText moneyDisplayController;
    [SerializeField] TextMeshProUGUI playerNameText;

    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    #region Announcement Board

    public void DisplayMessageInAnnouncementBoard(AnnouncementBoardData message)
    {
        announcementBoard?.Populate(message);
        announcementBoard?.AnimateIn();
    }

    #endregion


    #region Item Pick Up Icon

    public void PositionPickupIcon(object obj)
    {
        if (!pickupIcon)
            return;

        Transform trans = (Transform)obj;
        if (!pickupIcon.gameObject.activeSelf)
            pickupIcon.gameObject.SetActive(true);
        pickupIcon.position = mainCam.WorldToScreenPoint(trans.position);
    }

    public void TurnOffPickupIcon()
    {
        if (pickupIcon.gameObject.activeSelf)
            pickupIcon.gameObject.SetActive(false);
    }


    #endregion

    #region Speak To Icon

    public void PositionSpeakToIcon(object obj)
    {
        if (!speakIcon)
            return;

        Transform trans = (Transform)obj;
        if (!speakIcon.gameObject.activeSelf)
            speakIcon.gameObject.SetActive(true);
        speakIcon.position = mainCam.WorldToScreenPoint(trans.position) + Vector3.up * 15f;
    }

    public void TurnOnSpeakToIcon()
    {
        if (!speakIcon.gameObject.activeSelf)
            speakIcon.gameObject.SetActive(true);
    }

    public void TurnOffSpeakToIcon()
    {
        if (speakIcon.gameObject.activeSelf)
            speakIcon.gameObject.SetActive(false);
    }

    #endregion

    #region Objective UI

    public void UpdateAllObjectiveUI(object newObjective)
    {
        if (!objectivePointer)
        {
            Debug.LogError("No objective pointer connected to the UI controller");
            return;
        }

        if (!objectiveTextDisplay)
        {
            Debug.LogError("No objective text connected to the UI controller");
            return;
        }

        GlobalObjectiveController.KnightObjective receivedObjective = (GlobalObjectiveController.KnightObjective)newObjective;
        if (receivedObjective != null)
        {
            objectivePointer?.AssignObjective(receivedObjective.objectiveLocation);
            objectiveTextDisplay?.UpdateObjectiveText(receivedObjective.objectiveText);
        }
    }

    #endregion

    #region MoneyDisplay

    public void AcceptMoneyEvent(object money)
    {
        float newAmt = (float)money;
        UpdateMoneyText(newAmt);
    }

    public void UpdateMoneyText(float newAmount)
    {
        if (!moneyDisplayController)
        {
            Debug.LogError("Connect MoneyText controller to the UIController");
            return;
        }
        moneyDisplayController?.UpdateAmount(newAmount);
    }

    #endregion

    #region UI Audio

    public void PlayClipAsUIAudio(AudioClip clip)
    {
        if (!uiAudioSource)
        {
            Debug.LogError("Connect AudioSource to the UI Controller");
            return;
        }
        uiAudioSource.clip = clip;
        uiAudioSource.Play();
        
    }

    #endregion

    #region Player Name

    public void GrabLocalPlayerName()
    {
        CurrentGameData gameDataHolder = FindObjectOfType<CurrentGameData>();
        if (gameDataHolder && gameDataHolder.CurrentData.playerData != null)
        {
            UpdatePlayerName(gameDataHolder.CurrentData.playerData.playerName);
        }
    }

    public void UpdatePlayerName(string name)
    {
        if (!playerNameText)
        {
            Debug.LogError("No player name text attached to the UIController");
            return;
        }

        playerNameText.text = name;
        if (!playerNameText.gameObject.activeSelf)
            playerNameText.gameObject.SetActive(true);
    }

    #endregion

}
