using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMoneyText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI changeText;
    [SerializeField] RectTransform coinIcon;
    [SerializeField] RectTransform animatedMoneyIcon;

    CanvasGroup changeTextCG;
    bool animating = false;
    float lastAdjustment = 0;

    private void Start()
    {
        changeTextCG = changeText.GetComponent<CanvasGroup>();
    }

    public void UpdateAmount(float adjustmentAmount)
    {
        if (!text)
        {
            Debug.LogError("Connect text to the Money Text in UI");
            return;
        }

        Debug.Log("Updating money text");
        float currentAmount = GlobalVarsHolder.Instance.vars.playerGoldCoins;
        LeanTween.value(currentAmount - adjustmentAmount, currentAmount, 0.5f).setEase(LeanTweenType.easeOutSine)
            .setOnUpdate((float val) => 
            {
                text.text = val.ToString("F1");
            });
        changeText.text = $"+{adjustmentAmount}";
        
        if (!animating)
        {
            lastAdjustment = adjustmentAmount;
            AnimateCoin();
        }            
        else
        {
            lastAdjustment += adjustmentAmount;
            changeText.text = $"+{lastAdjustment}";
        }
        
    }

    public void AnimateCoin()
    {
        animating = true;
        GameObject spawnedCoin = Instantiate(animatedMoneyIcon.gameObject, animatedMoneyIcon.position, Quaternion.identity, transform);
        changeText.transform.position = new Vector3(changeText.transform.position.x, spawnedCoin.transform.position.y, 0);
        LeanTween.alphaCanvas(changeTextCG, 1, 0.1f)
            .setOnComplete(() => 
            {
                LeanTween.alphaCanvas(changeTextCG, 0, 0.35f).setDelay(0.2f)
                    .setOnComplete(() => 
                    {
                        animating = false;
                    });
            });
        LeanTween.move(changeText.gameObject, new Vector3(changeText.transform.position.x, coinIcon.position.y, 0), 0.28f).setEase(LeanTweenType.easeInOutBounce); ;        
        spawnedCoin.transform.SetAsFirstSibling();
        spawnedCoin.SetActive(true);
        LeanTween.move(spawnedCoin, coinIcon, 0.28f).setEase(LeanTweenType.easeInOutBounce)
            .setOnComplete(() => 
            {
                spawnedCoin.SetActive(false);
                spawnedCoin.transform.position = new Vector3(spawnedCoin.transform.position.x, coinIcon.transform.position.y - 65f, 0);
            });
        LeanTween.scale(spawnedCoin, Vector3.one * 1.2f, 0.25f).setEase(LeanTweenType.easeInOutBounce);
        LeanTween.scale(coinIcon.gameObject, Vector2.one * 1.5f, 0.33f).setLoopPingPong(1).setEase(LeanTweenType.easeInOutBounce)
            .setOnComplete(() =>
           {
               Destroy(spawnedCoin);
               LeanTween.scale(coinIcon.gameObject, Vector2.one, 0.33f).setEase(LeanTweenType.easeInOutBounce);
            });

    }
}
