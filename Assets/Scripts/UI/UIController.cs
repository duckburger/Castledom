using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] UIAnnouncementBoardController announcementBoard;
    [SerializeField] RectTransform pickupIcon;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void DisplayMessageInAnnouncementBoard(AnnouncementBoardData message)
    {
        announcementBoard?.Populate(message);
        announcementBoard?.AnimateIn();
    }


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
}
