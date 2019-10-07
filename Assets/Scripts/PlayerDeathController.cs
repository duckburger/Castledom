using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

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

    UIEscapeMenu escapeMenu;
    PlayerHouse currentHouseController;
    CancellationTokenSource tokenSource;

    int deathCount = 0; // This resets every new wave

    private void Start()
    {
        playerController = GetComponent<GlobalPlayerController>();
        currentHouseController = FindObjectOfType<PlayerHouse>();

        if (!currentHouseController)
            Debug.LogError("Not player house found in the scene!");

        escapeMenu = FindObjectOfType<UIEscapeMenu>();
    }

    private void OnApplicationQuit()
    {
        if (tokenSource != null && tokenSource.Token != null)
            tokenSource.Cancel();
    }


    public void OnPlayerDied()
    {
        playerController.TurnOffPlayerControls();
        if (!UIController.Instance)
        {
            Debug.LogError("No UIController found on scene");
            return;
        }

        tokenSource = new CancellationTokenSource();

        if (deathCount == 0)
        {
            ProcessFirstDeath(tokenSource.Token); // This locks the player in the back room of their house
        }
        else if (deathCount > 0)
        {
            ProcessSecondDeath(tokenSource.Token); // This kills the player, and lets them create an offspring
        }

    }

    private async void ProcessFirstDeath(CancellationToken ct)
    {
        // Show player knocked out, record a weapon they were carrying

        // Show message in UI
        // Fade out
        ct.ThrowIfCancellationRequested();        
        try
        {
            AnnouncementBoardData firstDeathMessage = new AnnouncementBoardData();
            firstDeathMessage.blackedOut = true;
            AnnouncementBoardData.AnnouncementMessage firstMessage = new AnnouncementBoardData.AnnouncementMessage();
            AnnouncementBoardData.AnnouncementMessage secondMessage = new AnnouncementBoardData.AnnouncementMessage();
            firstMessage.title = "You got knocked out";
            firstMessage.body = firstDeathAnnouncement;
            secondMessage.body = breakOutAnnouncement;
            firstDeathMessage.messages.Add(firstMessage);
            firstDeathMessage.messages.Add(secondMessage);
            await Task.Delay(3000, ct);
            await UIController.Instance.DisplayMessageInAnnouncementBoard(firstDeathMessage, ct);

            // Move player's body to the back room of the player's house
            playerController.MovePlayer(currentHouseController.HouseBackroomSpawn.position);
            // Restore player's health
            playerController.SetPlayerHealth(100);
            playerController.TurnOnPlayerObject();
            currentHouseController?.FadeMainRoof(false);
            // Set up guards in the player house and lock the back room door
            await currentHouseController?.ControlBackDoor(false, ct);
            CameraControls.Instance?.LerpZoomToValue(3.8f);
            CameraControls.Instance?.EnableZoomControls(true);
            for (int i = 0; i < currentHouseController.NpcPositionsForRespawn.Count; i++)
            {
                Instantiate(NPCRegistry.Instance.npcDatabase.GetRandomEnemy(1), currentHouseController.NpcPositionsForRespawn[i].position, Quaternion.identity, NPCRegistry.Instance.transform);
            }
        }
        finally
        {
            tokenSource.Dispose();
        }
        
        // Drop the empire's aggression a bit

        deathCount++;
    }

    private async void ProcessSecondDeath(CancellationToken ct)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            // Show message explaining the situation with offspring
            AnnouncementBoardData secondDeathMessage = new AnnouncementBoardData();
            secondDeathMessage.blackedOut = true;
            AnnouncementBoardData.AnnouncementMessage firstMessage = new AnnouncementBoardData.AnnouncementMessage();
            firstMessage.body = "You Are Dead!";
            AnnouncementBoardData.AnnouncementMessage secondMessage = new AnnouncementBoardData.AnnouncementMessage();
            secondMessage.body = "Little did you know, you have an offspring you didn't know about.";
            AnnouncementBoardData.AnnouncementMessage thirdMessage = new AnnouncementBoardData.AnnouncementMessage();
            thirdMessage.body = "Your offspring heard of your demise, and will continue what you started...";
            secondDeathMessage.messages.Add(firstMessage);
            secondDeathMessage.messages.Add(secondMessage);
            secondDeathMessage.messages.Add(thirdMessage);
            await Task.Delay(3000, ct);
            await UIController.Instance.DisplayMessageInAnnouncementBoard(secondDeathMessage, ct);
            // Show "Create offspring" screen
            // Move old player data into ancestor list
            escapeMenu?.OpenOffspringGenerator();
            // Put a random weapoin into a chest in your house + a percentage of player's money
            // Spawn offspring - new player - in the house
            // Explain the situation

        }
        finally
        {
            tokenSource.Dispose();
        }        

        deathCount = 0;
    }
  
}
