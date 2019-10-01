using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatusIcon : MonoBehaviour
{
    Canvas worldSpaceCanvas;
    Animator animator;

    private void Awake()
    {
        worldSpaceCanvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();
        worldSpaceCanvas.worldCamera = Camera.main;
    }

    public void ShowStunnedStatus()
    {
        worldSpaceCanvas.enabled = true;
        animator.SetBool("isStunned", true);
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
