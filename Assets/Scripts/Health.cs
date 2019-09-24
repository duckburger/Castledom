using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] ScriptableEvent onHealthAdjusted;
    [Space]
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100f;
    [Space]
    public bool armored = false;
    [Space(10)]
    [SerializeField] NPCStatusIcon statusIcon;

    public Action onDownToFirstHealthThreshold;
    public Action onDownToSecondHealthThreshold;
    public Action onHealthDecreased;
    public Action onDied;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void AdjustHealth(float amount, bool stunned = false)
    {
        currentHealth += amount;
        if (stunned)
        {
            GetStunned();
        }
        CombatMoveInfo moveInfo = new CombatMoveInfo(amount, gameObject, stunned);
        onHealthAdjusted?.RaiseWithData(moveInfo);
        if (gameObject.layer == 10)
            CameraControls.Instance?.ScreenShake();
        // TODO: AdjustHealthBar and/or appearance
        if (currentHealth <= 0)
        {
            // No health left
            onDied?.Invoke();
            return;
        }

        if (amount < 0)
            onHealthDecreased?.Invoke();

        if (currentHealth <= maxHealth / 2)
            onDownToFirstHealthThreshold?.Invoke();

        if (currentHealth <= maxHealth / 3)
            onDownToSecondHealthThreshold?.Invoke();
    }

    public void GetStunned()
    {
        statusIcon?.ShowStunnedStatus();
        // TODO: Write this + add stun duration and stun probability to this somehow
    }

}
