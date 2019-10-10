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
            visionController.onAlertCancelled += LoseAlertStatus;
        }
    }

    public void ShowStunnedStatus()
    {
        worldSpaceCanvas.enabled = true;
        animator.SetBool("isStunned", true);
    }

    public void ShowPreAlertedStatus()
    {
        worldSpaceCanvas.enabled = true;
        animator.SetTrigger("preAlert");
    }

    public void LoseAlertStatus()
    {
        worldSpaceCanvas.enabled = false;
        animator.SetBool("isAlerted", false);
        animator.Play("Idle");
    }
    

    public void ShowAlertedStatus()
    {
        worldSpaceCanvas.enabled = true;
        animator.SetBool("isAlerted", true);
    }


    public void Disable()
    {
        animator.SetBool("isStunned", false);
        worldSpaceCanvas.enabled = false;
    }
}
