using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightInteractionDetector : MonoBehaviour
{
    [SerializeField] ScriptableEvent onNearIneractiveObject;

    KnightInventory playerInventory;
    KnightSounds playerSoundController;
    NPCDialogueHolder nearbyDialogueHolder;
    IInteractiveObject nearbyInteractiveObject;
    IPickuppable nearbyPickuppableObject;

    private void Start()
    {
        playerSoundController = GetComponentInParent<KnightSounds>();
        playerInventory = GetComponentInParent<KnightInventory>();
    }

    private void Update()
    {
        if (nearbyDialogueHolder != null && Input.GetKeyDown(KeyCode.F))
        {
            nearbyDialogueHolder?.ActivateDialogue();
        }
        if (nearbyInteractiveObject != null && Input.GetKeyDown(KeyCode.F))
        {
            nearbyInteractiveObject?.Interact();
        }
        if (nearbyPickuppableObject != null && Input.GetKeyDown(KeyCode.F))
        {
            object pickedUpObject = nearbyPickuppableObject.GetPickuppableObject();
            switch (pickedUpObject)
            {
                case WeaponStatFile w:
                    playerInventory?.EquipWeapon(w, nearbyPickuppableObject?.GetWorldObject());
                    break;
                default:
                    break;
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {     
        if (!onNearIneractiveObject)
        {
            Debug.LogError("One of the interactive events is not connected to the interactionDetector");
            return;
        }
        CheckForItemTriggers(collider);
        CheckForDialogueTriggers(collider);
    }    

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!onNearIneractiveObject)
        {
            Debug.LogError("One of the interactive events is not connected to the interactionDetector");
            return;
        }
        CheckForItemTriggers(collider);
        CheckForDialogueTriggers(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!onNearIneractiveObject)
        {
            Debug.LogError("One of the interactive events is not connected to the interactionDetector");
            return;
        }
        if (nearbyDialogueHolder)
            CheckIfLeftDialogueTrigger(collider);
        if (nearbyInteractiveObject != null)
            CheckIfLeftInteractiveObject(collider);
        if (nearbyPickuppableObject != null)
            CheckIfLeftPickuppable(collider);
    }

    #region Dialogue Triggers

    void CheckForDialogueTriggers(Collider2D collider)
    {
        NPCDialogueHolder dialogueHolder = collider.GetComponent<NPCDialogueHolder>();
        if (dialogueHolder && dialogueHolder.myDialogue && dialogueHolder.canTalk)
        {
            UIController.ItemProximityEvent proximityEvent = new UIController.ItemProximityEvent(collider.transform, UIController.ItemProximityEvent.ProximityEventType.Dialogue);
            onNearIneractiveObject?.ActivateWithData(proximityEvent);
            nearbyDialogueHolder = dialogueHolder;
        }
    }

    void CheckIfLeftDialogueTrigger(Collider2D collider)
    {
        NPCDialogueHolder dialogueHolder = collider.GetComponent<NPCDialogueHolder>();
        if (dialogueHolder && dialogueHolder.myDialogue && dialogueHolder.canTalk)
        {
            onNearIneractiveObject?.Deactivate();
            nearbyDialogueHolder = null;
        }
    }

    #endregion

    #region Item Triggers

    void CheckForItemTriggers(Collider2D collider)
    {
        CheckPickuppable(collider);
        CheckInteractive(collider);
    }

    private void CheckPickuppable(Collider2D collider)
    {
        IPickuppable pickuppable = collider.GetComponent<IPickuppable>();
        if (pickuppable != null)
        {
            if (pickuppable.AutoPickupped())
            {
                // Pick it up automatically
                float moneyToPickup = (float)pickuppable.GetPickuppableObject();
                GlobalVarsHolder.Instance.UpdatePlayerMoney(moneyToPickup);
                playerSoundController?.PlaySound(pickuppable.PickupSound());
                Destroy(pickuppable.GetWorldObject());
                return;
            }
            else
            {
                nearbyPickuppableObject = pickuppable;
                UIController.ItemProximityEvent newEvent = new UIController.ItemProximityEvent(collider.transform, UIController.ItemProximityEvent.ProximityEventType.Pickuppable);
                onNearIneractiveObject?.ActivateWithData(newEvent);
            }
        }        
    }

    void CheckInteractive(Collider2D collider)
    {
        IInteractiveObject interactiveObject = collider.GetComponent<IInteractiveObject>();
        if (interactiveObject != null)
        {
            UIController.ItemProximityEvent newEvent = new UIController.ItemProximityEvent(collider.transform, UIController.ItemProximityEvent.ProximityEventType.Interactive);
            onNearIneractiveObject?.ActivateWithData(newEvent);
            nearbyInteractiveObject = interactiveObject;
        }
    }

    void CheckIfLeftPickuppable(Collider2D collider)
    {
        IPickuppable pickuppableObject = collider.GetComponent<IPickuppable>();
        if (nearbyPickuppableObject == pickuppableObject)
        {
            nearbyPickuppableObject = null;
            onNearIneractiveObject?.Deactivate();
        }
    }

    void CheckIfLeftInteractiveObject(Collider2D collider)
    {
        IInteractiveObject interactiveObject = collider.GetComponent<IInteractiveObject>();
        if (interactiveObject == nearbyInteractiveObject)
        {
            nearbyInteractiveObject = null;
            onNearIneractiveObject?.Deactivate();
        }
    }

    #endregion


}
