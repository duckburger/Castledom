using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBodyPartStatus : MonoBehaviour
{
    Health characterHealth;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        characterHealth = GetComponentInParent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (characterHealth)
        {
            characterHealth.onDownToFirstHealthThreshold += SwitchToFirstLevel;
            characterHealth.onDownToSecondHealthThreshold += SwitchToSecondLevel;
        }
    }


    void SwitchToSecondLevel()
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = GlobalVarsHolder.Instance.vars.fullDamagedColour;
        }
    }

    void SwitchToFirstLevel()
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = GlobalVarsHolder.Instance.vars.halfDamagedColour;
        }
    }



}
