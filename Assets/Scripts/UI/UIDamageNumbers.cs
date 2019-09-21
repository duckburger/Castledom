using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class CombatMoveInfo
{
    public float healthChange;
    public GameObject moveTarget;

    public CombatMoveInfo(float changeAmount, GameObject target)
    {
        this.healthChange = changeAmount;
        this.moveTarget = target;
    }
}

public class UIDamageNumbers : MonoBehaviour
{
    [SerializeField] GameObject infoTextPrefab;

    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void AcceptCombatMove(object move)
    {
        CombatMoveInfo acceptedInfo = (CombatMoveInfo)move;
        if (acceptedInfo != null)
        {
            SpawnDamageNumber(acceptedInfo);
        }
    }

    void SpawnDamageNumber(CombatMoveInfo moveInfo)
    {
        if (!infoTextPrefab)
        {
            Debug.LogError("No damage number prefab connected to the UIDamageNumbers");
            return;
        }

        if (!mainCam)
        {
            Debug.LogError("UIDamageNumbers couldn't find a main camera in the scene");
            return;
        }

        Vector2 newPosition = mainCam.WorldToScreenPoint(moveInfo.moveTarget.transform.position);
        TextMeshProUGUI newCombatInfo = Instantiate(infoTextPrefab, newPosition, Quaternion.identity, transform).GetComponent<TextMeshProUGUI>();
        if (moveInfo.healthChange > 0)
            newCombatInfo.text = $"+{moveInfo.healthChange}";
        else
            newCombatInfo.text = moveInfo.healthChange.ToString();


        LeanTween.moveLocalY(newCombatInfo.gameObject, newCombatInfo.rectTransform.localPosition.y + 50f, 0.45f).setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                Destroy(newCombatInfo.gameObject);
            });
    }
}
