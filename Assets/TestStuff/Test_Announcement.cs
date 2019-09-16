using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Announcement : MonoBehaviour
{
    [SerializeField] UIController uiController;

    #region Test Stuff

    public void SendTestAnnouncement()
    {
        AnnouncementBoardData newMessage = new AnnouncementBoardData();
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[0].title = "Entered:";
        newMessage.messages[0].body = "Castle Dom";
        newMessage.timeOnScreen = 3f;

        uiController.DisplayMessageInAnnouncementBoard(newMessage);
    }

    #endregion

    public void SendBeginningOfGameAnnouncement()
    {
        AnnouncementBoardData newMessage = new AnnouncementBoardData();
        newMessage.blackedOut = false;

        // 0
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[0].title = "";
        newMessage.messages[0].body = "You are a peasant somewhere in Eastern Europe. You've been living on your land off the beaten path for years.";
        // 
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[1].title = "";
        newMessage.messages[1].body = "But the word from the few passing merchants is that your neck of the woods pretty soon won't be that remote.";
        // 
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[2].title = "";
        newMessage.messages[2].body = "War has broken out with a neighbouring kingom, and the local ruler has decided to make some improvements to your village, and specifically - your personal road.";
        // 3
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[3].title = "";
        newMessage.messages[3].body = "One morning you wake up to two soldiers inquisitively strolling down your road...";

        uiController.DisplayMessageInAnnouncementBoard(newMessage);
    }
}
