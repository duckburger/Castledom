using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightInteractionDetector : MonoBehaviour
{
    [SerializeField] ScriptableEvent onNearDialogueNPC;
    [SerializeField] ScriptableEvent onMovedAwayFromDialogueNPC;

    NPCDialogueHolder nearbyDialogueHolder;
    bool nearADialogueTrigger = false;


    private void Update()
    {
        if (nearADialogueTrigger && Input.GetKeyDown(KeyCode.F))
        {
            nearbyDialogueHolder?.ActivateDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NPCDialogueHolder dialogueHolder = collision.GetComponent<NPCDialogueHolder>();
        if (dialogueHolder && dialogueHolder.myDialogue && dialogueHolder.canTalk)
        {
            if (!onNearDialogueNPC || !onMovedAwayFromDialogueNPC)
            {
                Debug.LogError("One of the dialogue NPC events is not connected to the player's interaction detector");
            }
            onNearDialogueNPC?.RaiseWithData(collision.transform);
            nearbyDialogueHolder = dialogueHolder;
            nearADialogueTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        NPCDialogueHolder dialogueHolder = collision.GetComponent<NPCDialogueHolder>();
        if (dialogueHolder && dialogueHolder.myDialogue && dialogueHolder.canTalk)
        {
            if (!onNearDialogueNPC || !onMovedAwayFromDialogueNPC)
            {
                Debug.LogError("One of the dialogue NPC events is not connected to the player's interaction detector");
            }
            onNearDialogueNPC?.RaiseWithData(collision.transform);
            nearbyDialogueHolder = dialogueHolder;
            nearADialogueTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {        
        NPCDialogueHolder dialogueHolder = collision.GetComponent<NPCDialogueHolder>();
        if (dialogueHolder && dialogueHolder.myDialogue && dialogueHolder.canTalk)
        {
            if (!onNearDialogueNPC || !onMovedAwayFromDialogueNPC)
            {
                Debug.LogError("One of the dialogue NPC events is not connected to the player's interaction detector");
            }
            onMovedAwayFromDialogueNPC?.Raise();
            nearbyDialogueHolder = null;
            nearADialogueTrigger = false;
        }
    }
}
