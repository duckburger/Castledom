using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMoneyText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RectTransform coinIcon;
    [SerializeField] RectTransform animatedMoneyIcon;

    public void UpdateAmount(float newAmount)
    {
        if (!text)
        {
            Debug.LogError("Connect text to the Money Text in UI");
            return;
        }

        Debug.Log("Updating money text");
        float currentAmount;
        float.TryParse(text.text, out currentAmount);
        LeanTween.value(currentAmount, newAmount, 0.5f).setEase(LeanTweenType.easeOutSine)
            .setOnUpdate((float val) => 
            {
                text.text = val.ToString("F1");
            });
        AnimateCoin();
    }

    public void AnimateCoin()
    {
        GameObject spawnedCoin = Instantiate(animatedMoneyIcon.gameObject, animatedMoneyIcon.position, Quaternion.identity, transform);
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
