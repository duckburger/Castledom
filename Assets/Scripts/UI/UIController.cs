using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] UIAnnouncementBoardController announcementBoard;

    public void DisplayMessageInAnnouncementBoard(AnnouncementBoardData message)
    {
        announcementBoard?.Populate(message);
        announcementBoard?.AnimateIn();
    }
}
