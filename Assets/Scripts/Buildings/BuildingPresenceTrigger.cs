using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPresenceTrigger : MonoBehaviour
{
    [SerializeField] SpriteRenderer rendererToFade;

    BuildingEntranceTrigger[] entranceTriggers;
    BuildingExitTrigger[] exitTriggers;

    private void Start()
    {
        entranceTriggers = GetComponentsInChildren<BuildingEntranceTrigger>();
        exitTriggers = GetComponentsInChildren<BuildingExitTrigger>();

        if (entranceTriggers.Length > 0)
        {
            for (int i = 0; i < entranceTriggers.Length; i++)
            {
                entranceTriggers[i].entranceTriggered += FadeOut;
            }
        }

        if (exitTriggers.Length > 0)
        {
            for (int i = 0; i < exitTriggers.Length; i++)
            {
                exitTriggers[i].exitTriggered += FadeIn;
            }
        }
    }

    public void FadeIn()
    {
        if (!rendererToFade)
        {
            Debug.LogError("No renderer connected to the BuildingPresenceTrigger");
            return;
        }
        if (rendererToFade.color.a > 0)
            return;

        LeanTween.value(0, 1, 0.23f)
            .setOnUpdate((float val) => 
            {
                rendererToFade.color = new Color(rendererToFade.color.r, rendererToFade.color.g, rendererToFade.color.b, val);
            });
    }

    public void FadeOut()
    {
        if (!rendererToFade)
        {
            Debug.LogError("No renderer connected to the BuildingPresenceTrigger");
            return;
        }
        if (rendererToFade.color.a < 1)
            return;
        Debug.Log("Fading out!!!");
        LeanTween.value(1, 0, 0.23f)
            .setOnUpdate((float val) =>
            {
                rendererToFade.color = new Color(rendererToFade.color.r, rendererToFade.color.g, rendererToFade.color.b, val);
            });
    }
}
