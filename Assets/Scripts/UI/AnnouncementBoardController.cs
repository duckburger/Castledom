using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[Serializable]
public class AnnouncementBoardData
{
    public List<AnnouncementMessage> messages = new List<AnnouncementMessage>();
    public bool blackedOut = false;
    public float timeOnScreen = 0;

    [SerializeField] public class AnnouncementMessage
    {
        public string title;
        public string body;
    }
}

public class AnnouncementBoardController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI body;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] CanvasGroup blackBGCanvasGroup;
    [SerializeField] GameObject continueButton;
    [SerializeField] TextMeshProUGUI continueButtonText;
    [Space]
    [SerializeField] ScriptableEvent blockControls;
    Queue<AnnouncementBoardData.AnnouncementMessage> messageQueue = new Queue<AnnouncementBoardData.AnnouncementMessage>();
    Coroutine messageTimer;
    float currentOnScreenTimer = 0;

    private void Start()
    {
        AnimateOut();
    }    

    public void AnimateIn()
    {
        LeanTween.cancel(gameObject);
        LeanTween.alphaCanvas(canvasGroup, 1, 0.23f).setOnComplete(() => 
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        });

        blockControls?.Activate();
    }

    public void AnimateOut()
    {
        LeanTween.cancel(gameObject);
        LeanTween.alphaCanvas(canvasGroup, 0, 0.23f).setOnComplete(() => 
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            blockControls?.Deactivate();
        });
        LeanTween.alphaCanvas(blackBGCanvasGroup, 0, 0.12f).setOnComplete(() => 
        {
            blackBGCanvasGroup.blocksRaycasts = false;
            blackBGCanvasGroup.interactable = false;
        });

    }

    public void Populate(AnnouncementBoardData messageData)
    {
        messageQueue.Clear();
        for (int i = 0; i < messageData.messages.Count; i++)
        {
            messageQueue.Enqueue(messageData.messages[i]);
        }
        AnnouncementBoardData.AnnouncementMessage firstMessage = messageQueue.Dequeue();
        title.text = firstMessage.title;
        body.text = firstMessage.body;
        if (messageData.blackedOut)
        {
            LeanTween.alphaCanvas(blackBGCanvasGroup, 1, 0.12f).setOnComplete(() => 
            {
                blackBGCanvasGroup.blocksRaycasts = true;
                blackBGCanvasGroup.interactable = true;
            });
        }
        currentOnScreenTimer = messageData.timeOnScreen;
        if (messageTimer != null)
            StopCoroutine(messageTimer);
        if (currentOnScreenTimer > 0)
        {
            messageTimer = StartCoroutine(OnScreenTimer(messageData.timeOnScreen));
        }

        if (messageQueue.Count > 0)
        {
            ShowContinueButton(true);
        }
        else
        {
            ShowContinueButton(false);
        }
    }

    public void ShowNextMessage()
    {
        if (messageTimer != null)
            StopCoroutine(messageTimer);

        if (messageQueue.Count > 0)
        {
            AnnouncementBoardData.AnnouncementMessage firstMessage = messageQueue.Dequeue();
            title.text = firstMessage.title;
            body.text = firstMessage.body;     
            ShowContinueButton(true);  
        }
        else
        {
            AnimateOut();
        }
    }   

    IEnumerator OnScreenTimer(float onScreenTime)
    {
        yield return new WaitForSeconds(onScreenTime);
        if (messageQueue.Count <= 0)
        {
            AnimateOut();
        }
        else
        {
            ShowNextMessage();    
            if (currentOnScreenTimer > 0)
                messageTimer = StartCoroutine(OnScreenTimer(currentOnScreenTimer));
        }
    }


    void ShowContinueButton(bool enabled)
    {
        continueButton.SetActive(enabled);
    }
    

}
