using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightControls : MonoBehaviour
{
    [Header("Global light")]
    [SerializeField] Light2D globalLight;
    [SerializeField] float sneakingLightIntensity = 1f;
    [SerializeField] float regularLightIntensity = 1.3f;

    int globalLightTweenID;

    public void ActivateStealthMode()
    {
        if (globalLight)
        {
            if (globalLightTweenID > 0)
                LeanTween.cancel(globalLightTweenID);
            globalLightTweenID = LeanTween.value(globalLight.intensity, sneakingLightIntensity, 0.15f).setEase(LeanTweenType.easeInOutSine)
                .setOnUpdate((float val) => globalLight.intensity = val).id;            
        }
    }

    public void DeactivateStealthMode()
    {
        if (globalLight)
        {
            if (globalLightTweenID > 0)
                LeanTween.cancel(globalLightTweenID);
            globalLightTweenID = LeanTween.value(globalLight.intensity, regularLightIntensity, 0.15f).setEase(LeanTweenType.easeInOutSine)
                .setOnUpdate((float val) => globalLight.intensity = val).id;
        }
    }
}
