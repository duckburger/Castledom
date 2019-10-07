using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;
public class UIController : MonoBehaviour
{
    public static UIController Instance;

    #region Data Model

    [Serializable]
    public class ItemProximityEvent
    {
        [Serializable]
        public enum ProximityEventType
        {
            Pickuppable, 
            Dialogue,
            Interactive
        }

        public Transform itemTransform;
        public ProximityEventType type;

        public ItemProximityEvent(Transform targetTransform, ProximityEventType eventType)
        {
            this.itemTransform = targetTransform;
            this.type = eventType;
        }
    }

    #endregion

    [SerializeField] UIAnnouncementBoardController announcementBoard;
    [Space]
    [SerializeField] Image interactionIcon;
    [SerializeField] Sprite talkIcon;
    [SerializeField] Sprite pickUpIcon;
    [SerializeField] Sprite useIcon;
    [Space]
    [SerializeField] UIObjectivePointer objectivePointer;
    [SerializeField] UIObjectiveTextDisplay objectiveTextDisplay;
    [SerializeField] AudioSource uiAudioSource;
    [SerializeField] UIMoneyText moneyDisplayController;
    [SerializeField] TextMeshProUGUI playerNameText;

    Camera mainCam;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    #region Announcement Board

    public async Task DisplayMessageInAnnouncementBoard(AnnouncementBoardData message, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        announcementBoard?.AnimateIn(ct);
        await announcementBoard?.Populate(message);
    }

    #endregion

    #region Interaction Icon

    public void AcceptPlayerProximityEvent(object eventData)
    {
        if (!interactionIcon)
        {
            Debug.LogError("No interaction icon connected to the UIController");
            return;
        }

        ItemProximityEvent receivedEvent = (ItemProximityEvent)eventData;
        if (receivedEvent == null)
        {
            Debug.LogError("Error in converting ItemProximityEvent data inside UIController");
            return;
        }

        if (!interactionIcon.gameObject.activeSelf)
            interactionIcon.gameObject.SetActive(true);

        switch (receivedEvent.type)
        {
            case ItemProximityEvent.ProximityEventType.Pickuppable:
                PositionPickupIcon(receivedEvent);
                break;
            case ItemProximityEvent.ProximityEventType.Dialogue:
                PositionSpeakToIcon(receivedEvent);
                break;
            case ItemProximityEvent.ProximityEventType.Interactive:
                PositionGenInteractionIcon(receivedEvent);
                break;
            default:
                break;
        }
    }

    public void TurnOffInteractionIcon()
    {
        if (interactionIcon.gameObject.activeSelf)
            interactionIcon.gameObject.SetActive(false);
    }

    #endregion

    #region Item Pick Up Icon

    public void PositionPickupIcon(ItemProximityEvent data)
    {
        if (!useIcon)
            return;

        interactionIcon.sprite = pickUpIcon;
        interactionIcon.rectTransform.position = mainCam.WorldToScreenPoint(data.itemTransform.position);
    }
   
    #endregion

    #region Speak To Icon

    public void PositionSpeakToIcon(ItemProximityEvent data)
    {
        interactionIcon.sprite = talkIcon;
        interactionIcon.rectTransform.position = mainCam.WorldToScreenPoint(data.itemTransform.position) + Vector3.up * 15f;
    }

    #endregion

    #region General Interaction Icon

    public void PositionGenInteractionIcon(ItemProximityEvent data)
    {
        interactionIcon.sprite = useIcon;
        interactionIcon.rectTransform.position = mainCam.WorldToScreenPoint(data.itemTransform.position);
    }

    #endregion

    #region Objective UI

    public void UpdateAllObjectiveUI(object newObjective)
    {
        if (!objectivePointer)
        {
            Debug.LogError("No objective pointer connected to the UI controller");
            return;
        }

        if (!objectiveTextDisplay)
        {
            Debug.LogError("No objective text connected to the UI controller");
            return;
        }

        GlobalObjectiveController.KnightObjective receivedObjective = (GlobalObjectiveController.KnightObjective)newObjective;
        if (receivedObjective != null)
        {
            objectivePointer?.AssignObjective(receivedObjective.objectiveLocation);
            objectiveTextDisplay?.UpdateObjectiveText(receivedObjective.objectiveText);
        }
    }

    #endregion

    #region MoneyDisplay

    public void AcceptMoneyEvent(object money)
    {
        float newAmt = (float)money;
        UpdateMoneyText(newAmt);
    }

    public void UpdateMoneyText(float newAmount)
    {
        if (!moneyDisplayController)
        {
            Debug.LogError("Connect MoneyText controller to the UIController");
            return;
        }
        moneyDisplayController?.UpdateAmount(newAmount);
    }

    #endregion

    #region UI Audio

    public void PlayClipAsUIAudio(AudioClip clip)
    {
        if (!uiAudioSource)
        {
            Debug.LogError("Connect AudioSource to the UI Controller");
            return;
        }
        uiAudioSource.clip = clip;
        uiAudioSource.Play();
        
    }

    #endregion

    #region Player Name

    public void GrabLocalPlayerName()
    {
        CurrentGameData gameDataHolder = FindObjectOfType<CurrentGameData>();
        if (gameDataHolder && gameDataHolder.CurrentData.playerData != null)
        {
            UpdatePlayerName(gameDataHolder.CurrentData.playerData.playerName);
        }
    }

    public void UpdatePlayerName(string name)
    {
        if (!playerNameText)
        {
            Debug.LogError("No player name text attached to the UIController");
            return;
        }

        playerNameText.text = name;
        if (!playerNameText.gameObject.activeSelf)
            playerNameText.gameObject.SetActive(true);
    }

    #endregion

}
