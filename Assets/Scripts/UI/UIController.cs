using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] UIAnnouncementBoardController announcementBoard;
    [SerializeField] RectTransform pickupIcon;
    [SerializeField] RectTransform speakIcon;
    [SerializeField] UIObjectivePointer objectivePointer;
    [SerializeField] UIObjectiveTextDisplay objectiveTextDisplay;
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

}
