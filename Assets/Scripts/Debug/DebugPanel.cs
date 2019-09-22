using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (canvasGroup.alpha < 1)
            {
                AnimateIn();
            }
            else
            {
                AnimateOut();
            }
        }
    }

    public void AnimateIn()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        LeanTween.alphaCanvas(canvasGroup, 1, 0.23f);
    }


    public void AnimateOut()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        LeanTween.alphaCanvas(canvasGroup, 0, 0.18f);
    }
}
