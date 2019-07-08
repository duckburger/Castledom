using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Announcement : MonoBehaviour
{
    [SerializeField] UIController uiController;


    public void SendTestAnnouncement()
    {
        AnnouncementBoardData newMessage = new AnnouncementBoardData();
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[0].title = "Entered:";
        newMessage.messages[0].body = "Castle Dom";
        newMessage.timeOnScreen = 3f;

        uiController.DisplayMessageInAnnouncementBoard(newMessage);
    }


    public void SendBeginningOfGameAnnouncement()
    {
        AnnouncementBoardData newMessage = new AnnouncementBoardData();
        newMessage.blackedOut = true;

        // 0
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[0].title = "";
        newMessage.messages[0].body = "You are a grandson of a local tradesman in a castledom, and you've been helping him run the business since you were a child.";
        // 1
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[1].title = "";
        newMessage.messages[1].body = "Business is going well, however the lord and barons of the castle have grown corrupt. You've heard of a few local businesses giving regular kicksbacks to the barons.";
        // 2
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[2].title = "";
        newMessage.messages[2].body = "Your grandfather has always insisted that he hasn't paid a coin yet.";
        // 3
        newMessage.messages.Add(new AnnouncementBoardData.AnnouncementMessage());
        newMessage.messages[3].title = "";
        newMessage.messages[3].body = "Today is just another day on the job";

        uiController.DisplayMessageInAnnouncementBoard(newMessage);
    }
}
