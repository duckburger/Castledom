using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightInteractionDetector : MonoBehaviour
{
    [SerializeField] ScriptableEvent onNearDialogueNPC;
    [SerializeField] ScriptableEvent onMovedAwayFromDialogueNPC;

    KnightSounds knightSoundController;
    NPCDialogueHolder nearbyDialogueHolder;
    bool nearADialogueTrigger = false;

    private void Start()
    {
        knightSoundController = GetComponentInParent<KnightSounds>();
    }

    private void Update()
    {
        if (nearADialogueTrigger && Input.GetKeyDown(KeyCode.F))
        {
            nearbyDialogueHolder?.ActivateDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CheckForDialogueTriggers(collider);
        CheckForItemTriggers(collider);
    }    

    private void OnTriggerStay2D(Collider2D collider)
    {
        CheckForDialogueTriggers(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        CheckIfLeftDialogueTrigger(collider);
    }

    #region Dialogue Triggers

    void CheckForDialogueTriggers(Collider2D collider)
    {
        NPCDialogueHolder dialogueHolder = collider.GetComponent<NPCDialogueHolder>();
        if (dialogueHolder && dialogueHolder.myDialogue && dialogueHolder.canTalk)
        {
            if (!onNearDialogueNPC || !onMovedAwayFromDialogueNPC)
            {
                Debug.LogError("One of the dialogue NPC events is not connected to the player's interaction detector");
            }
            onNearDialogueNPC?.RaiseWithData(collider.transform);
            nearbyDialogueHolder = dialogueHolder;
            nearADialogueTrigger = true;
        }
    }

    void CheckIfLeftDialogueTrigger(Collider2D collider)
    {
        NPCDialogueHolder dialogueHolder = collider.GetComponent<NPCDialogueHolder>();
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

    #endregion

    #region Item Triggers

    void CheckForItemTriggers(Collider2D collider)
    {
        IPickuppable pickuppable = collider.GetComponent<IPickuppable>();
        if (pickuppable != null)
        {
            if (pickuppable.AutoPickupped())
            {
                // Pick it up automatically
                float moneyToPickup = (float)pickuppable.GetPickuppableObject();
                GlobalVarsHolder.Instance.UpdatePlayerMoney(moneyToPickup);
                knightSoundController?.PlaySound(pickuppable.PickupSound());
                // 1) Delete the pile
                Destroy(collider.gameObject);
                // 2) Spawn the coins to animate
                // 3) Fly them to the indicator

                // 4) Make a caching sound for each coin

            }
        }
    }

    #endregion
}
