using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField] Slider loadingBarSlider;

    CanvasGroup cg;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
        AnimateIn();
    }


    #region Animating In / Out

    public void AnimateIn()
    {
        if (!cg)
            return;

        cg.interactable = true;
        cg.blocksRaycasts = true;

        LeanTween.alphaCanvas(cg, 1, 0.12f);
    }

    public void AnimateOut()
    {
        if (!cg)
            return;

        cg.interactable = false;
        cg.blocksRaycasts = false;

        LeanTween.alphaCanvas(cg, 0, 0.18f);
    }

    #endregion

    public void UpdateLoadingValue(float newVal)
    {
        loadingBarSlider.value = newVal;
    }
}
