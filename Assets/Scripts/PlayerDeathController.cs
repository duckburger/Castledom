using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerDeathController : MonoBehaviour
{
    GlobalPlayerController playerController;
    [TextArea(3, 10)]
    [SerializeField] string firstDeathAnnouncement;
    [SerializeField] string breakOutAnnouncement = "Break Out!";
    [TextArea(3, 10)]
    [SerializeField] string secondDeathAnnouncement;
    [Space]
    [SerializeField] GameObject playerBodyPrefab;

    PlayerHouse currentHouseController;

    int deathCount = 0; // This resets every new wave

    private void Start()
    {
        playerController = GetComponent<GlobalPlayerController>();
        currentHouseController = FindObjectOfType<PlayerHouse>();

        if (!currentHouseController)
            Debug.LogError("Not player house found in the scene!");
    }

    public void OnPlayerDied()
    {
        playerController.TurnOffPlayerControls();
        if (!UIController.Instance)
        {
            Debug.LogError("No UIController found on scene");
            return;
        }

        if (deathCount == 0)
        {
            ProcessFirstDeath(); // This locks the player in the back room of their house
        }
        else if (deathCount > 1)
        {
            ProcessSecondDeath(); // This kills the player, and lets them load
        }

    }

    private async void ProcessFirstDeath()
    {
        // Show player knocked out, record a weapon they were carrying

        // Show message in UI
        // Fade out
        AnnouncementBoardData firstDeathMessage = new AnnouncementBoardData();
        firstDeathMessage.blackedOut = true;
        AnnouncementBoardData.AnnouncementMessage firstMessage = new AnnouncementBoardData.AnnouncementMessage();
        AnnouncementBoardData.AnnouncementMessage secondMessage = new AnnouncementBoardData.AnnouncementMessage();
        firstMessage.title = "You got knocked out";
        firstMessage.body = firstDeathAnnouncement;
        secondMessage.body = breakOutAnnouncement;
        firstDeathMessage.messages.Add(firstMessage);
        firstDeathMessage.messages.Add(secondMessage);
        await Task.Delay(3000);
        await UIController.Instance.DisplayMessageInAnnouncementBoard(firstDeathMessage);

        // Move player's body to the back room of the player's house
        playerController.MovePlayer(currentHouseController.HouseBackroomSpawn.position);
        // Restore player's health
        playerController.SetPlayerHealth(100);
        playerController.TurnOnPlayerObject();
        currentHouseController?.FadeMainRoof(false);
        // Set up guards in the player house and lock the back room door
        currentHouseController?.ControlBackDoor(false);
        CameraControls.Instance?.LerpZoomToValue(3.8f);
        CameraControls.Instance?.EnableZoomControls(true);
        for (int i = 0; i < currentHouseController.NpcPositionsForRespawn.Count; i++)
        {
            Instantiate(NPCRegistry.Instance.npcDatabase.GetRandomEnemy(1), currentHouseController.NpcPositionsForRespawn[i].position, Quaternion.identity, NPCRegistry.Instance.transform);
        }
        // Drop the empire's aggression a bit


        deathCount++;
    }

    private async void ProcessSecondDeath()
    {
        AnnouncementBoardData secondDeathMessage = new AnnouncementBoardData();
        secondDeathMessage.blackedOut = true;
        AnnouncementBoardData.AnnouncementMessage firstMessage = new AnnouncementBoardData.AnnouncementMessage();
        firstMessage.body = "You Are Dead!";
        secondDeathMessage.messages.Add(firstMessage);
        await Task.Delay(3000);
        await UIController.Instance.DisplayMessageInAnnouncementBoard(secondDeathMessage);

        deathCount = 0;
    }
  
}
