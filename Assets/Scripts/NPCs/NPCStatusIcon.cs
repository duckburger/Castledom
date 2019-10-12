using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatusIcon : MonoBehaviour
{
    Canvas worldSpaceCanvas;
    Animator animator;

    NPCVision visionController;

    private void Awake()
    {
        worldSpaceCanvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();
        worldSpaceCanvas.worldCamera = Camera.main;
        visionController = GetComponentInParent<NPCVision>();
        if (visionController)
        {
            visionController.onPreAlerted += ShowPreAlertedStatus;
            visionController.onAlerted += ShowAlertedStatus;
            visionController.onAlertCancelled += Disable;
            visionController.onLostSightOfTarget += ShowPreAlertedStatus;
        }
    }

    public void ShowStunnedStatus()
    {
        worldSpaceCanvas.enabled = true;
        animator?.Play("StatusIcon_Stunned");
    }

    public void ShowPreAlertedStatus()
    {
        worldSpaceCanvas.enabled = true;
        animator?.Play("StatusIcon_PreAlert");
    }    

    public void ShowAlertedStatus()
    {
        worldSpaceCanvas.enabled = true;
        animator?.Play("StatusIcon_Alerted");
    }


    public void Disable()
    {
        animator?.Play("Idle");
        worldSpaceCanvas.enabled = false;
    }
}
