using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIPlayerHealth : MonoBehaviour
{
    [Serializable]
    public class UIHealthData
    {
        public float currentValue;
        public float maxValue;

        public UIHealthData(float current, float max)
        {
            this.currentValue = current;
            this.maxValue = max;
        }

    }

    [SerializeField] Slider mainSlider;
    [SerializeField] Image secondaryFill;

    

    public void AcceptHealthUpdate(object newData)
    {
        UIHealthData newHealthData = (UIHealthData)newData;        
        UpdateHealthBar(newHealthData);
    }

    void UpdateHealthBar(UIHealthData healthChange)
    {
        // For now assuming that max health is always 100
        float newVal = Mathf.Clamp01(healthChange.currentValue / healthChange.maxValue);
        LeanTween.value(mainSlider.value, newVal, 0.75f).setEase(LeanTweenType.easeOutElastic)
            .setOnUpdate((float val) => 
            {
                mainSlider.value = val;
            });
        LeanTween.value(secondaryFill.fillAmount, newVal, 0.85f).setEase(LeanTweenType.easeOutElastic)
            .setOnUpdate((float val) =>
            {
                secondaryFill.fillAmount = val;
            }).setDelay(0.25f);
    }
}
