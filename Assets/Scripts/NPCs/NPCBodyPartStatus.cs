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
            characterHealth.onHealthDecreased += BlinkShader;
        }
    }

    public void BlinkShader()
    {
        LeanTween.value(spriteRenderer.material.GetFloat("_OverlayStrength"), 1, 0.1f).setLoopPingPong(1)
            .setOnUpdate((float val) => 
            {
                spriteRenderer.material.SetFloat("_OverlayStrength", val);
            });
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
