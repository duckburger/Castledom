using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] AnnouncementBoardController announcementBoard;

    public void DisplayMessageInAnnouncementBoard(AnnouncementBoardData message)
    {
        announcementBoard?.Populate(message);
        announcementBoard?.AnimateIn();
    }
}
