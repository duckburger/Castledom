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

        Vector2 newPosition = moveInfo.moveTarget.transform.position;
        TextMeshProUGUI newCombatInfo = Instantiate(infoTextPrefab, newPosition, Quaternion.identity, moveInfo.moveTarget.transform).GetComponent<TextMeshProUGUI>();

        if (moveInfo.healthChange > 0)
            newCombatInfo.text = $"+{moveInfo.healthChange}";
        else
            newCombatInfo.text = moveInfo.healthChange.ToString();

        if (moveInfo.moveTarget.layer == 10 || moveInfo.moveTarget.layer == 11)
        {
            newCombatInfo.color = GlobalVarsHolder.Instance.vars.friendlyCombatTextColour;
        }
        else if (moveInfo.moveTarget.layer == 12)
        {
            newCombatInfo.color = GlobalVarsHolder.Instance.vars.enemyCombatTextColour;
        }

        LeanTween.moveY(newCombatInfo.gameObject, newCombatInfo.rectTransform.position.y + 0.7f, 0.55f).setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                Destroy(newCombatInfo.gameObject);
            });
    }
}
